using System;
using System.Collections.Generic;
using Godot;

namespace SlayTheSpireLike.scripts.global;

public partial class ResourceLoaderManager : SingletonNode<ResourceLoaderManager>
{
    private readonly Dictionary<string, Resource> _loadedResources = new();
    private static readonly Dictionary<string, Lazy<PackedScene>> SceneLoaders = new();
    // 缓存“工厂函数”，方便高频实例化
    private readonly Dictionary<string, Delegate> _sceneFactories = new();
    private readonly Dictionary<string, Delegate> _resourceFactories = new();


    #region 基础加载方法

    public T LoadResource<T>(string path) where T : Resource
    {
        if (string.IsNullOrEmpty(path))
        {
            GD.PrintErr("LoadResource: path is null or empty");
            return null;
        }

        if (_loadedResources.TryGetValue(path, out var cached))
            return cached as T;

        var loaded = GD.Load<T>(path);
        if (loaded != null)
        {
            _loadedResources[path] = loaded;
            return loaded;
        }

        GD.PrintErr($"Failed to load resource at path: {path}");
        return null;
    }

    public Lazy<PackedScene> GetSceneLoader(string path)
    {
        if (SceneLoaders.TryGetValue(path, out var loader))
            return loader;

        var sceneLoader = new Lazy<PackedScene>(() => LoadResource<PackedScene>(path));
        SceneLoaders[path] = sceneLoader;
        return sceneLoader;
    }

    #endregion

    #region 便利的实例工厂/直接实例化

    /// <summary>
    /// 直接根据路径实例化并返回 T（如果失败返回 null）。
    /// 推荐：开发阶段少量使用，频繁实例化时使用 RegisterSceneFactory + factory 调用。
    /// </summary>
    public T CreateInstance<T>(string path) where T : Node
    {
        var scene = GetSceneLoader(path).Value;
        if (scene == null)
        {
            GD.PrintErr($"CreateInstance<{typeof(T).Name}> failed: scene null at {path}");
            return null;
        }

        var inst = scene.Instantiate<T>();
        if (inst == null)
            GD.PrintErr($"CreateInstance<{typeof(T).Name}> failed to instantiate at {path}");

        return inst;
    }

    /// <summary>
    /// 注册并返回一个可重复使用的工厂函数 
    /// （内部会缓存 Delegate，下一次会直接返回同一个工厂）
    /// </summary>
    public Func<T> GetOrRegisterSceneFactory<T>(string path) where T : Node
    {
        if (string.IsNullOrEmpty(path))
            return null;

        if (_sceneFactories.TryGetValue(path, out var d))
            return d as Func<T>;

        // 创建工厂 lambda（延迟每次实例化）
        var factory = () =>
        {
            var scene = GetSceneLoader(path).Value;
            if (scene == null) throw new InvalidOperationException($"Scene not loaded: {path}");
            var node = scene.Instantiate<T>();
            return node ?? throw new InvalidOperationException($"Failed to instantiate {typeof(T).Name} from {path}");
        };

        _sceneFactories[path] = factory;
        return factory;
    }
    /// <summary>
    /// 返回一个 Func，每次调用会返回一个资源实例或副本（取决于 duplicate）。
    /// T 必须继承 Godot.Resource（如 Material、Texture、AudioStream 等）
    /// </summary>
    public Func<T> GetOrRegisterResourceFactory<T>(string path, bool duplicate = true) where T : Resource
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentNullException(nameof(path));

        if (_resourceFactories.TryGetValue(path, out var d))
            return d as Func<T>;

        Func<T> factory = () =>
        {
            // 优先用已缓存的 Resource（LoadResource 会缓存到 _loadedResources）
            var res = LoadResource<T>(path);
            if (res == null) throw new InvalidOperationException($"Failed to load resource at {path}");

            if (!duplicate)
            {
                return res;
            }

            // 尝试复制资源，Godot.Resource.Duplicate() 返回 Resource
            var dupObj = res.Duplicate();
            if (dupObj is T typedDup)
                return typedDup;

            // 兜底：如果 Duplicate 不适用或失败，直接返回原始资源（注意：可能会共享状态）
            return res;
        };

        _resourceFactories[path] = factory;
        return factory;
    }
    #endregion

    #region 缓存管理 / 卸载 / 预加载

    public void PreloadCommonResources(IEnumerable<string> paths)
    {
        foreach (var p in paths)
        {
            // 如果是场景，调用 GetSceneLoader 来懒加载 PackedScene
            GetSceneLoader(p);
            // 也可以直接 LoadResource 如果想马上把 Resource 加载进内存
            LoadResource<Resource>(p);
        }
    }

    public void UnloadResource(string path)
    {
        if (_loadedResources.Remove(path))
        {
            GD.Print($"Unloaded resource: {path}");
        }

        if (SceneLoaders.Remove(path))
        {
            GD.Print($"Removed scene loader cache: {path}");
        }

        _sceneFactories.Remove(path);
    }

    public void ClearAllCaches()
    {
        _loadedResources.Clear();
        SceneLoaders.Clear();
        _sceneFactories.Clear();
        GD.Print("Cleared all resource caches in ResourceLoaderManager.");
    }

    #endregion
}
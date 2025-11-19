using System;
using System.Collections.Generic;
using Godot;

namespace SlayTheSpireLike.scripts.global;

/// <summary>
/// 资源加载与管理器，用于统一处理 Godot 中的资源加载、缓存、实例化工厂等操作。
/// 支持资源预加载、卸载以及工厂函数注册以提升性能。
/// 继承自单例节点 SingletonNode，确保全局唯一访问点。
/// </summary>
public partial class ResourceLoaderManager : SingletonNode<ResourceLoaderManager>
{
    /// <summary>
    /// 已加载的资源缓存字典，键为资源路径，值为对应的 Resource 实例。
    /// </summary>
    private readonly Dictionary<string, Resource> _loadedResources = new();

    /// <summary>
    /// 场景加载器缓存字典，使用 Lazy 模式实现懒加载 PackedScene。
    /// 键为场景路径，值为包装后的 Lazy<PackedScene> 对象。
    /// </summary>
    private static readonly Dictionary<string, Lazy<PackedScene>> SceneLoaders = new();

    /// <summary>
    /// 场景实例化工厂缓存字典，键为路径，值为封装好的委托（Func&lt;T&gt;），用于快速创建场景实例。
    /// </summary>
    private readonly Dictionary<string, Delegate> _sceneFactories = new();

    /// <summary>
    /// 资源实例化工厂缓存字典，键为路径，值为封装好的委托（Func&lt;T&gt;），用于快速获取资源副本或引用。
    /// </summary>
    private readonly Dictionary<string, Delegate> _resourceFactories = new();


    #region 基础加载方法

    /// <summary>
    /// 根据指定路径加载一个资源，并将其缓存以便后续复用。
    /// 若资源已存在则直接从缓存中返回；若加载失败将打印错误信息并返回 null。
    /// </summary>
    /// <typeparam name="T">要加载的资源类型，必须继承自 Resource。</typeparam>
    /// <param name="path">资源在项目中的相对路径。</param>
    /// <returns>成功时返回对应类型的资源对象，否则返回 null。</returns>
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

    /// <summary>
    /// 获取一个场景加载器（Lazy&lt;PackedScene&gt;），支持按需加载场景文件。
    /// 若该路径已被缓存，则直接返回已有加载器。
    /// </summary>
    /// <param name="path">场景资源的路径。</param>
    /// <returns>返回一个 Lazy&lt;PackedScene&gt; 对象，表示延迟加载的场景。</returns>
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
    /// 直接根据路径实例化并返回 T 类型的节点（如果失败返回 null）。
    /// 推荐：开发阶段少量使用，频繁实例化时应使用 RegisterSceneFactory + factory 调用提高效率。
    /// </summary>
    /// <typeparam name="T">目标节点类型，必须继承自 Node。</typeparam>
    /// <param name="path">场景资源的路径。</param>
    /// <returns>成功时返回实例化的节点对象，否则返回 null。</returns>
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
    /// 注册并返回一个可重复使用的工厂函数，用于高效地多次实例化同一场景。
    /// 内部会缓存 Delegate，下次调用相同路径时直接返回已存在的工厂。
    /// </summary>
    /// <typeparam name="T">目标节点类型，必须继承自 Node。</typeparam>
    /// <param name="path">场景资源的路径。</param>
    /// <returns>返回一个 Func&lt;T&gt; 工厂函数，可用于反复实例化节点。</returns>
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
    /// 返回一个 Func，每次调用会返回一个资源实例或副本（取决于 duplicate 参数）。
    /// T 必须继承 Godot.Resource（如 Material、Texture、AudioStream 等）。
    /// </summary>
    /// <typeparam name="T">资源类型，必须继承自 Resource。</typeparam>
    /// <param name="path">资源的路径。</param>
    /// <param name="duplicate">是否需要复制资源，默认为 true。设为 false 则可能共享原始资源状态。</param>
    /// <returns>返回一个 Func&lt;T&gt; 工厂函数，用于反复获取资源实例。</returns>
    public Func<T> GetOrRegisterResourceFactory<T>(string path, bool duplicate = true) where T : Resource
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentNullException(nameof(path));

        if (_resourceFactories.TryGetValue(path, out var d))
            return d as Func<T>;

        var factory = () =>
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

    /// <summary>
    /// 预加载一组常用资源，包括场景和普通资源。
    /// 使用 GetSceneLoader 和 LoadResource 进行懒加载或立即加载。
    /// </summary>
    /// <param name="paths">资源路径集合。</param>
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

    /// <summary>
    /// 卸载指定路径的资源及其相关缓存项（包括资源本身、场景加载器及工厂函数）。
    /// </summary>
    /// <param name="path">要卸载的资源路径。</param>
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

    /// <summary>
    /// 清空所有资源相关的缓存数据，包括已加载资源、场景加载器和各类工厂函数。
    /// </summary>
    public void ClearAllCaches()
    {
        _loadedResources.Clear();
        SceneLoaders.Clear();
        _sceneFactories.Clear();
        GD.Print("Cleared all resource caches in ResourceLoaderManager.");
    }

    #endregion
}

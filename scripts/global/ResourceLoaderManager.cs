using System;
using System.Collections.Generic;
using Godot;

namespace SlayTheSpireLike.scripts.global;

/// <summary>
/// 资源加载管理器，用于统一管理游戏中所有资源的加载和缓存
/// 通过单例模式提供全局访问点
/// </summary>
public partial class ResourceLoaderManager : SingletonNode<ResourceLoaderManager>
{
    // 使用字典缓存已加载的资源，避免重复加载
    private readonly Dictionary<string, Resource> _loadedResources = new();
    
    // 使用Lazy延迟加载场景，提高性能
    private static readonly Dictionary<string, Lazy<PackedScene>> SceneLoaders = new();

    /// <summary>
    /// 加载指定路径的资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <returns>加载的资源实例</returns>
    public T LoadResource<T>(string path) where T : Resource
    {
        // 检查资源是否已经加载过
        if (_loadedResources.TryGetValue(path, out var resource))
        {
            return resource as T;
        }

        // 加载资源并缓存
        var loadedResource = GD.Load<T>(path);
        if (loadedResource != null)
        {
            _loadedResources[path] = loadedResource;
        }
        else
        {
            GD.PrintErr($"Failed to load resource at path: {path}");
        }

        return loadedResource;
    }

    /// <summary>
    /// 获取指定路径的场景加载器
    /// 使用Lazy模式确保场景只在首次访问时加载
    /// </summary>
    /// <param name="path">场景路径</param>
    /// <returns>场景加载器</returns>
    public Lazy<PackedScene> GetSceneLoader(string path)
    {
        // 检查场景加载器是否已经创建
        if (SceneLoaders.TryGetValue(path, out var loader))
        {
            return loader;
        }

        // 创建新的场景加载器并缓存
        var sceneLoader = new Lazy<PackedScene>(() => LoadResource<PackedScene>(path));
        SceneLoaders[path] = sceneLoader;
        return sceneLoader;
    }

    /// <summary>
    /// 从指定路径加载场景并实例化
    /// </summary>
    /// <typeparam name="T">场景根节点类型</typeparam>
    /// <param name="path">场景路径</param>
    /// <returns>实例化的场景根节点</returns>
    public T InstantiateScene<T>(string path) where T : Node
    {
        var scene = GetSceneLoader(path).Value;
        if (scene != null)
        {
            return scene.Instantiate<T>();
        }

        GD.PrintErr($"Failed to instantiate scene at path: {path}");
        return null;
    }
    
    /// <summary>
    /// 预加载常用资源
    /// 在游戏启动时调用以提高运行时性能
    /// </summary>
    public void PreloadCommonResources()
    {
        // 在这里可以预加载常用资源
        // 例如: LoadResource<PackedScene>("res://scenes/ui/card_ui.tscn");
    }
}
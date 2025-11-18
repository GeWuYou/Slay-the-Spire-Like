using Godot;

namespace SlayTheSpireLike.scripts.global;

/// <summary>
/// 单例节点基类，继承自Node
/// 任何继承此类的节点都可以自动获得单例访问能力
/// </summary>
/// <typeparam name="T">派生类的具体类型</typeparam>
public partial class SingletonNode<T> : Node where T : SingletonNode<T>, new()
{
    /// <summary>
    /// 单例实例的公共静态访问属性
    /// </summary>
    public static T Instance { get; private set; }

    /// <summary>
    /// 在节点准备就绪时设置单例实例
    /// </summary>
    public override void _Ready()
    {
        Instance = (T)this;
    }

    /// <summary>
    /// 在节点退出树时清理单例实例引用
    /// </summary>
    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
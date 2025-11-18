using global::SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.extensions;

/// <summary>
///     提供全局类扩展方法的静态类
/// </summary>
public static class GlobalClassExtensions
{
    /// <summary>
    ///     获取全局事件管理器实例的扩展方法
    /// </summary>
    /// <param name="context">扩展方法的上下文节点</param>
    /// <returns>返回全局Events节点的实例</returns>
    public static Events Events(this Node context)
    {
        return context.GetNode<Events>("/root/Events");
    }
}
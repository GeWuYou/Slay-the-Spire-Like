using Godot;

namespace SlayTheSpireLike.scripts.extensions;

/// <summary>
/// 节点扩展方法类，提供对Godot节点的扩展功能
/// </summary>
public static class NodeExtensions
{
    /// <summary>
    /// 安全地将节点加入删除队列，在下一帧开始时释放节点资源
    /// </summary>
    /// <param name="node">要释放的节点实例</param>
    public static void QueueFreeX(this Node node)
    {
        // 检查节点是否为空
        if (node is null)
        {
            return;
        }

        // 检查节点实例是否有效
        if (!GodotObject.IsInstanceValid(node))
        {
            return;
        }

        // 检查节点是否已经加入删除队列
        if (node.IsQueuedForDeletion())
        {
            return;
        }

        // 延迟调用QueueFree方法，避免在当前帧中直接删除节点
        node.CallDeferred(Node.MethodName.QueueFree);
    }

    /// <summary>
    /// 立即释放节点资源，不等待下一帧
    /// </summary>
    /// <param name="node">要立即释放的节点实例</param>
    public static void FreeX(this Node node)
    {
        // 检查节点是否为空
        if (node is null)
        {
            return;
        }

        // 检查节点实例是否有效
        if (!GodotObject.IsInstanceValid(node))
        {
            return;
        }

        // 检查节点是否已经加入删除队列
        if (node.IsQueuedForDeletion())
        {
            return;
        }

        // 立即释放节点资源
        node.Free();
    }
}
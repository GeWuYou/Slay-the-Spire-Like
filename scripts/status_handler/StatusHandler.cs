using System.Linq;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.status_handler;

/// <summary>
/// 状态处理器类，用于管理角色身上的各种状态效果（如增益、减益等）。
/// 提供添加状态、应用状态以及查询状态等功能，并支持按类型批量处理状态。
/// </summary>
public partial class StatusHandler : GridContainer
{
    /// <summary>
    /// 拥有该状态的角色节点引用。
    /// </summary>
    [Export] public Node2D StatusOwner { get; set; }

    /// <summary>
    /// 应用状态之间的间隔时间（秒），默认为 0.25 秒。
    /// </summary>
    [Export] public float StatusApplyInterval { get; set; } = 0.25f;
    
    /// <summary>
    /// 当某一类型的状态全部被应用后触发的信号。
    /// 参数：type - 被应用的状态类型。
    /// </summary>
    [Signal]
    public delegate void StatusesAppliedEventHandler(Status.StatusType type);

    /// <summary>
    /// 根据指定的状态类型依次应用所有匹配的状态效果。
    /// 使用 Tween 控制执行顺序与间隔时间。
    /// 不会处理事件驱动型（EventBased）状态。
    /// </summary>
    /// <param name="type">要应用的状态类型。</param>
    public void ApplyStatusesByType(Status.StatusType type)
    {
        // 忽略事件驱动型状态
        if (type == Status.StatusType.EventBased)
        {
            return;
        }

        // 获取当前容器中所有符合类型的 Status 实例
        var statusQueue = GetAllStatuses().Where(status => status.Type == type).ToArray();
        
        // 如果没有对应类型的状态则直接发出完成信号
        if (statusQueue.Length == 0)
        {
            EmitSignal(SignalName.StatusesApplied, (int)type);
            return;
        }

        // 创建 Tween 来控制状态的应用流程
        var tween = CreateTween();
        foreach (var status in statusQueue)
        {
            tween.TweenCallback(Callable.From(() => status.ApplyStatus(StatusOwner)));
            tween.TweenInterval(StatusApplyInterval);
        }

        // 添加一个空的时间点确保最后能正确触发 Finished 事件
        tween.TweenInterval(0.0f);
        tween.Finished += () => EmitSignal(SignalName.StatusesApplied, (int)type);
    }

    /// <summary>
    /// 向状态处理器中添加一个新的状态实例。
    /// 若已存在相同 ID 的状态且可叠加，则根据叠加规则更新其数值；
    /// 否则创建新的 UI 元素并初始化状态。
    /// </summary>
    /// <param name="status">需要添加的状态对象。</param>
    public void AddStatus(Status status)
    {
        var stackable = status.StatusStackType != Status.StackType.None;

        // 如果尚未拥有此状态，则新建 UI 并加入场景树
        if (!HasStatus(status.Id))
        {
            var newStatusUi = ResourceFactories.StatusUiFactory();
            newStatusUi.Status = status;
            newStatusUi.Status.StatusApplied += OnStatusApplied;
            newStatusUi.Status.InitializeStatus(StatusOwner);
            AddChild(newStatusUi);
            return;
        }

        // 已存在的不可叠加也不可刷新的状态直接忽略
        if (!status.CanExpire && !stackable)
        {
            return;
        }

        // 可刷新持续时间的状态进行叠加
        if (status.CanExpire && status.StatusStackType == Status.StackType.Duration)
        {
            GetStatus(status.Id).Duration += status.Duration;
            return;
        }

        // 强度叠加型状态增加层数
        if (status.StatusStackType == Status.StackType.Intensity)
        {
            GetStatus(status.Id).Stacks += status.Stacks;
        }
    }

    /// <summary>
    /// 当某个状态被成功应用时调用的方法。
    /// 主要作用是减少该状态的剩余持续回合数（如果可以过期的话）。
    /// </summary>
    /// <param name="status">刚刚被应用的状态对象。</param>
    private void OnStatusApplied(Status status)
    {
        if (status.CanExpire)
        {
            status.Duration -= 1;
        }
    }

    /// <summary>
    /// 获取当前容器内所有的状态对象列表。
    /// 遍历子节点中的 StatusUi 组件提取其中的状态数据。
    /// </summary>
    /// <returns>包含所有状态对象的数组。</returns>
    private Array<Status> GetAllStatuses()
    {
        var statuses = new Array<Status>();
        foreach (var child in GetChildren())
        {
            if (child is StatusUi statusUi)
            {
                statuses.Add(statusUi.Status);
            }
        }

        return statuses;
    }

    /// <summary>
    /// 根据给定的状态 ID 查找对应的 Status 对象。
    /// </summary>
    /// <param name="statusId">目标状态的唯一标识符。</param>
    /// <returns>找到的 Status 对象；若未找到返回 null。</returns>
    private Status GetStatus(string statusId)
    {
        foreach (var node in GetChildren())
        {
            if (node is StatusUi statusUi && statusUi.Status.Id == statusId)
            {
                return statusUi.Status;
            }
        }

        return null;
    }

    /// <summary>
    /// 判断是否已经包含了具有特定 ID 的状态。
    /// </summary>
    /// <param name="statusId">待检查的状态 ID。</param>
    /// <returns>如果存在返回 true，否则返回 false。</returns>
    private bool HasStatus(string statusId)
    {
        foreach (var node in GetChildren())
        {
            if (node is StatusUi statusUi && statusUi.Status.Id == statusId)
            {
                return true;
            }
        }

        return false;
    }
}

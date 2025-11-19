using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.extensions;

namespace SlayTheSpireLike.scripts.ui.state;

/// <summary>
///     表示卡牌处于瞄准状态时的逻辑处理类。该状态下卡牌会响应鼠标移动，并根据鼠标位置决定是否触发状态转换。
/// </summary>
public partial class CardAimingState : CardState
{
    private Events _events;

    /// <summary>
    ///     鼠标 Y 轴位置的阈值，当鼠标 Y 坐标超过此值时将触发返回基础状态的转换。
    /// </summary>
    [Export]
    public float MouseYSnapbackThreshold { private set; get; } = 138f;

    /// <summary>
    ///     在节点准备就绪时调用。用于初始化事件系统引用。
    /// </summary>
    public override void _Ready()
    {
        _events = Events.Instance;
    }

    /// <summary>
    ///     进入瞄准状态时调用。设置卡牌 UI 的颜色和文本提示，清空目标列表，并播放动画将卡牌移动到指定位置。
    ///     同时关闭 DropPointDetector 的监测功能，并发出 CardAimingStarted 事件。
    /// </summary>
    public override void Enter()
    {
        // 设置卡牌UI的颜色与状态文字
        CardUi.Targets.Clear();

        // 获取父级控件并计算卡牌应移动到的位置
        var parent = CardUi.Parent;
        var parentSize = parent.Size;

        // 计算卡片UI的偏移位置，使其居中显示在父容器上方
        var offset = new Vector2(parentSize.X / 2, -CardUi.Size.Y / 2);
        offset.X -= CardUi.Size.X / 2;

        // 触发卡片UI的位置动画信号，将其移动到计算好的位置
        CardUi.AnimateToPosition(parent.GlobalPosition + offset, 0.2f);

        // 关闭卡片UI的掉落点检测器监控功能
        CardUi.DropPointDetector.Monitoring = false;

        // 发出卡片瞄准开始事件信号
        _events.EmitSignal(Events.SignalName.CardAimingStarted, CardUi);
    }

    /// <summary>
    ///     处理输入事件。在瞄准状态下，根据鼠标移动或按键操作判断是否需要切换状态：
    ///     - 如果鼠标 Y 坐标超过阈值或按下右键，则切换回基础状态；
    ///     - 如果释放或按下左键，则切换到释放状态。
    /// </summary>
    /// <param name="event">当前输入事件对象。</param>
    public override void OnInput(InputEvent @event)
    {
        // 判断鼠标是否位于底部区域或按下右键以决定是否返回基础状态
        var mouseAtBottom = CardUi.GetGlobalMousePosition().Y > MouseYSnapbackThreshold;

        if (mouseAtBottom || @event.IsActionPressed("right_mouse"))
        {
            EmitSignal(CardState.SignalName.TransitionRequested, this, (int)State.Base);
        }
        else if (@event.IsActionReleased("left_mouse") || @event.IsActionPressed("left_mouse"))
        {
            GetViewport().SetInputAsHandled();
            EmitSignal(CardState.SignalName.TransitionRequested, this, (int)State.Released);
        }
    }

    /// <summary>
    ///     退出瞄准状态时调用。发出 CardAimingEnded 事件通知其他系统该卡牌已结束瞄准。
    /// </summary>
    public override void Exit()
    {
        _events.EmitSignal(Events.SignalName.CardAimingEnded, CardUi);
    }
}
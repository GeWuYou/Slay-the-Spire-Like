using DeckBuilderTutorial.scripts.extensions;
using Godot;

namespace DeckBuilderTutorial.scripts.ui.state;

/// <summary>
/// 卡牌拖拽状态类，继承自CardState基类
/// 负责处理卡牌在拖拽过程中的行为和状态转换
/// </summary>
public partial class CardDraggingState : CardState
{
    /// <summary>
    /// 获取或设置拖拽操作的最小阈值距离
    /// </summary>
    /// <remarks>
    /// 用于判断用户是否开始了有效的拖拽操作的距离阈值
    /// </remarks>
    [Export] public float DragMinimumThreshold { private set; get; } = 0.05f;

    /// <summary>
    /// 获取或设置最小拖拽时间是否已过期
    /// </summary>
    /// <remarks>
    /// 用于判断拖拽操作是否满足最小时间要求的标志位
    /// </remarks>
    private bool MinimumDragTimeElapsed { set; get; }

    /// <summary>
    /// 进入拖拽状态时执行的初始化操作
    /// 将卡牌UI重新父级到UI层，并设置拖拽状态的视觉效果
    /// </summary>
    public override void Enter()
    {
        // 获取UI层节点并将卡牌UI重新父级到该层
        var uiLayer = GetTree().GetFirstNodeInGroup("ui_layer");
        if (uiLayer != null)
        {
            CardUi.Reparent(uiLayer);
        }

        // 设置拖拽状态的视觉样式
        CardUi.ColorRect.Color = Colors.NavyBlue;
        CardUi.StateText.Text = "拖拽";
        
        // 初始化最小拖拽时间标记为false，表示尚未达到最小拖拽时间要求
        MinimumDragTimeElapsed = false;
        
        // 创建一个计时器，用于检测是否达到最小拖拽时间阈值
        // 当计时器超时时，将最小拖拽时间标记设置为true
        var timer = GetTree().CreateTimer(DragMinimumThreshold, false);
        timer.Timeout+= () => MinimumDragTimeElapsed = true;

    }

    /// <summary>
    /// 处理输入事件，包括鼠标移动、确认和取消操作
    /// 根据不同的输入事件触发相应的状态转换
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public override void OnInput(InputEvent @event)
    {
        // 检查各种输入条件
        var isSingleTargeted = CardUi.Card.CardTarget.IsSingleTargeted();
        var isMouseMotion = @event is InputEventMouseMotion;
        var cancel = @event.IsActionPressed("right_mouse");
        var confirm = @event.IsActionReleased("left_mouse") || @event.IsActionPressed("left_mouse");

        // 如果当前目标是敌人且有可用目标并且是鼠标移动事件，则切换到瞄准状态
        if (isSingleTargeted && isMouseMotion && CardUi.Targets.Count > 0)
        {
            EmitSignal(CardState.SignalName.TransitionRequested, this, State.Aiming.GetCardStateValue());
            return;
        }

        // 处理鼠标移动事件，更新卡牌位置
        if (isMouseMotion)
        {
            CardUi.GlobalPosition = CardUi.GetGlobalMousePosition() - CardUi.PivotOffset;
        }

        // 处理取消操作或确认操作的状态转换
        if (cancel)
        {
            // 当用户取消操作时，触发状态转换信号，将卡片状态转换为基础状态
            EmitSignal(CardState.SignalName.TransitionRequested, this, State.Base.GetCardStateValue());
        }
        else if (confirm && MinimumDragTimeElapsed)
        {
            // 当用户确认操作且最小拖拽时间已过时，触发状态转换信号，将卡片状态转换为释放状态
            GetViewport().SetInputAsHandled();
            EmitSignal(CardState.SignalName.TransitionRequested, this, State.Released.GetCardStateValue());
        }
    }
}

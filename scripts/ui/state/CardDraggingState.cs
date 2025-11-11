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
        CardUi.StateText.Text = "Dragging";
    }

    /// <summary>
    /// 处理输入事件，包括鼠标移动、确认和取消操作
    /// 根据不同的输入事件触发相应的状态转换
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public override void OnInput(InputEvent @event)
    {
        // 检查各种输入条件
        var isMouseMotion = @event is InputEventMouseMotion;
        var cancel = @event.IsActionPressed("right_mouse");
        var confirm = @event.IsActionReleased("left_mouse") || @event.IsActionPressed("left_mouse");
        
        // 处理鼠标移动事件，更新卡牌位置
        if (isMouseMotion)
        {
            CardUi.GlobalPosition = CardUi.GetGlobalMousePosition() - CardUi.PivotOffset;
        }

        // 处理取消操作或确认操作的状态转换
        if (cancel)
        {
            EmitSignal(CardState.SignalName.TransitionRequested, this, State.Base.GetCardStateValue());
        }
        else if (confirm)
        {
            GetViewport().SetInputAsHandled();
            EmitSignal(CardState.SignalName.TransitionRequested, this, State.Released.GetCardStateValue());
        }
    }
}

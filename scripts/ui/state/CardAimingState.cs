using DeckBuilderTutorial.scripts.extensions;
using Godot;

namespace DeckBuilderTutorial.scripts.ui.state;

/// <summary>
/// CardAimingState
/// </summary>
public partial class CardAimingState: CardState
{
    public override void Enter()
    {
        CardUi.ColorRect.Color = Colors.Yellow;
        CardUi.StateText.Text = "Aiming";
    }

    public override void OnInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
        {
            // 在瞄准状态下跟随鼠标移动
            CardUi.GlobalPosition = CardUi.GetGlobalMousePosition() - CardUi.PivotOffset;
        }
        
        if (@event.IsActionPressed("left_mouse"))
        {
            // 切换到释放状态
            EmitSignal(CardState.SignalName.TransitionRequested, this, State.Released.GetCardStateValue());
        }
        else if (@event.IsActionPressed("right_mouse"))
        {
            // 取消瞄准回到基础状态
            EmitSignal(CardState.SignalName.TransitionRequested, this, State.Base.GetCardStateValue());
        }
    }
}
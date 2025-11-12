
using DeckBuilderTutorial.scripts.extensions;
using Godot;

namespace DeckBuilderTutorial.scripts.ui.state;

/// <summary>

/// </summary>
public partial class CardReleasedState: CardState
{
    [Export]
    public bool Played { private set; get; }

    public override void Enter()
    {
        CardUi.ColorRect.Color = Colors.DarkViolet;
        CardUi.StateText.Text = "释放";
        Played = false;
        if (CardUi.Targets.Count == 0)
        {
            return;
        }
        Played = true;
        GD.Print($"play card for target {CardUi.Targets}");
    }

    public override void OnInput(InputEvent @event)
    {
        if (Played)
        {
            return;
        }

        EmitSignal(CardState.SignalName.TransitionRequested, this, State.Base.GetCardStateValue());
    }
}
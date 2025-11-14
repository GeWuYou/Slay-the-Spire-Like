
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
        GD.Print("进入发布状态!");
        Played = false;
        if (CardUi.Targets.Count == 0)
        {
            return;
        }
        
        GD.Print("打出");
        Played = true;
        CardUi.Play();
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
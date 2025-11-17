using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.extensions;

namespace SlayTheSpireLike.scripts.ui.state;

/// <summary>

/// </summary>
public partial class CardReleasedState: CardState
{
    private Events _events;

    public override void _Ready()
    {
        _events = Events.Instance;
    }
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
        _events.EmitSignal(SlayTheSpireLike.scripts.global.Events.SignalName.CardToolTipHideRequest);
    }

    public override void OnInput(InputEvent @event)
    {
        if (Played)
        {
            return;
        }

        EmitSignal(SlayTheSpireLike.scripts.ui.state.CardState.SignalName.TransitionRequested, this, State.Base.GetCardStateValue());
    }
}
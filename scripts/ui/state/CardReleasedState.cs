using global::SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.ui.state;

/// <summary>
/// </summary>
public partial class CardReleasedState : CardState
{
    private Events _events;

    [Export] public bool Played { private set; get; }

    public override void _Ready()
    {
        _events = Events.Instance;
    }

    public override void Enter()
    {
        Played = false;
        if (CardUi.Targets.Count == 0) return;
        Played = true;
        CardUi.Play();
        _events.RaiseCardToolTipHideRequest();

    }

    public override void PostEnter()
    {
        EmitSignal(CardState.SignalName.TransitionRequested, this, (int)State.Base);
    }
}
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts;

public partial class BattleUi : CanvasLayer
{
    public CharacterStats PlayerStats { get; set; }
    
    [Export] public ManaUi ManaUi { get; set; }
    [Export] public Hand Hand { get; set; }
    [Export] public Button EndTurnButton { get; set; }
    private Events _events;
    public override void _Ready()
    {
        _events = Events.Instance;
        ManaUi.CharacterStats = PlayerStats;
        Hand.PlayerStats = PlayerStats;
        _events.PlayerHandDrawn += OnPlayerHandDrawn;
        EndTurnButton.Pressed+=OnEndTurnButtonPressed;
    }

    private void OnEndTurnButtonPressed()
    {
        EndTurnButton.Disabled = true;
        _events.EmitSignal(Events.SignalName.PlayerTurnEnded);
    }

    private void OnPlayerHandDrawn()
    {
       EndTurnButton.Disabled = false;
    }
}
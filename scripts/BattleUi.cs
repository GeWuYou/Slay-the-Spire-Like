using DeckBuilderTutorial.scripts.resources;
using DeckBuilderTutorial.scripts.ui;
using global::DeckBuilderTutorial.scripts.global;
using Godot;

namespace DeckBuilderTutorial.scripts;

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
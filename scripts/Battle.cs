using DeckBuilderTutorial.scripts.player;
using DeckBuilderTutorial.scripts.resources;
using global::DeckBuilderTutorial.scripts.global;
using Godot;

namespace DeckBuilderTutorial.scripts;

public partial class Battle : Node2D
{
    [Export] public CharacterStats PlayerStats { get; set; }
    private BattleUi _battleUi;
    [Export]
    public PlayerHandler PlayerHandler { get; set; }
    
    [Export]
    public Player Player { get; set; }

    private Events _events;
    public override void _EnterTree()
    {
        _battleUi = GetNode<BattleUi>("BattleUI");
        var newStats = PlayerStats.CreateInstance();
        Player.Stats = newStats;
        _battleUi.PlayerStats = newStats;
    }

    public override void _Ready()
    {
        _events = Events.Instance;
        _events.PlayerTurnEnded += PlayerHandler.EndTurn;
        _events.PlayerHandDiscarded += PlayerHandler.StartTurn;
        StartBattle(_battleUi.PlayerStats);
    }

    private void StartBattle(CharacterStats newStats)
    {
       PlayerHandler.StartBattle(newStats);
    }
}
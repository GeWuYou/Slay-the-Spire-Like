using DeckBuilderTutorial.scripts.player;
using DeckBuilderTutorial.scripts.resources;
using Godot;

namespace DeckBuilderTutorial.scripts;

public partial class Battle : Node2D
{
    [Export] public CharacterStats PlayerStats { get; set; }
    private BattleUi _battleUi;
    [Export]
    public PlayerHandler PlayerHandler { get; set; }
    public override void _EnterTree()
    {
        _battleUi = GetNode<BattleUi>("BattleUI");
        var newStats = PlayerStats.CreateInstance();
        _battleUi.PlayerStats = newStats;
    }

    public override void _Ready()
    {
        StartBattle(_battleUi.PlayerStats);
    }

    private void StartBattle(CharacterStats newStats)
    {
       PlayerHandler.StartBattle(newStats);
    }
}
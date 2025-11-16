using DeckBuilderTutorial.scripts.resources;
using DeckBuilderTutorial.scripts.ui;
using Godot;

namespace DeckBuilderTutorial.scripts;

public partial class BattleUi : CanvasLayer
{
    public CharacterStats PlayerStats { get; set; }

    [Export] public ManaUi ManaUi { get; set; }
    [Export] public Hand Hand { get; set; }

    public override void _Ready()
    {
        ManaUi.CharacterStats = PlayerStats;
        Hand.PlayerStats = PlayerStats;
    }
}
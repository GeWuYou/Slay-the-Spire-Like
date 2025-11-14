using DeckBuilderTutorial.scripts.player;
using DeckBuilderTutorial.scripts.resources;
using Godot;

namespace DeckBuilderTutorial.scripts.ui;

public partial class ManaUi : Panel
{
    [Export] private Player _player;
    private CharacterStats _characterStats;
    
    public CharacterStats CharacterStats
    {
        get => _characterStats;
        private set
        {
            _characterStats = value;
            if (!CharacterStats.IsConnected(Stats.SignalName.StatsChanged, Callable.From(OnStatsChanged)))
            {
                CharacterStats.StatsChanged += OnStatsChanged;
            }

            if (!IsNodeReady())
            {
                Callable.From(OnStatsChanged);
            }
            else
            {
                OnStatsChanged();
            }
        }
    }

    private void OnStatsChanged()
    {
        ManaLabel.Text = $"{CharacterStats.Mana}/{CharacterStats.MaxMana}";
    }

    [Export] public Label ManaLabel { get; private set; }
    public override void _Ready()
    {
        CharacterStats = _player.Stats;
    }
}
using DeckBuilderTutorial.scripts.resources;
using Godot;

namespace DeckBuilderTutorial.scripts.ui;

public partial class ManaUi : Panel
{
    private CharacterStats _characterStats;

    [Export]
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
}
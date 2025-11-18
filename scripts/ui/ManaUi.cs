using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class ManaUi : Panel
{
    private CharacterStats _characterStats;

    public CharacterStats CharacterStats
    {
        get => _characterStats;
        set
        {
            _characterStats = value;
            if (!CharacterStats.IsConnected(Stats.SignalName.StatsChanged, Callable.From(OnStatsChanged)))
                CharacterStats.StatsChanged += OnStatsChanged;

            if (!IsNodeReady())
                Callable.From(OnStatsChanged);
            else
                OnStatsChanged();
        }
    }

    [Export] public Label ManaLabel { get; private set; }

    private void OnStatsChanged()
    {
        ManaLabel.Text = $"{CharacterStats.Mana}/{CharacterStats.MaxMana}";
    }
}
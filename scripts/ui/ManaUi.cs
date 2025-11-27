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
            CallDeferred(nameof(OnStatsChanged));
        }
    }

    [Export] public Label ManaLabel { get; private set; }

    public override void _ExitTree()
    {
        if (_characterStats != null && _characterStats.IsConnected(Stats.SignalName.StatsChanged, Callable.From(OnStatsChanged)))
        {
            _characterStats.StatsChanged -= OnStatsChanged;
        }
    }

    private void OnStatsChanged()
    {
        if (!IsInstanceValid(this) || !IsInstanceValid(ManaLabel)) return;
        
        ManaLabel.Text = $"{CharacterStats.Mana}/{CharacterStats.MaxMana}";
    }
}
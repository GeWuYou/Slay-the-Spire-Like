using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class ManaUi : Panel
{
    private CharacterStats _characterStats;
    private Callable _onStatsChangedCallable;

    public CharacterStats CharacterStats
    {
        get => _characterStats;
        set
        {
            _characterStats = value;
            if (!_characterStats.IsConnected(Stats.SignalName.StatsChanged, _onStatsChangedCallable))
            {
                _characterStats.Connect(Stats.SignalName.StatsChanged, _onStatsChangedCallable);
            }
            CallDeferred(nameof(OnStatsChanged));
        }
    }

    [Export] public Label ManaLabel { get; private set; }

    public override void _Ready()
    {
        _onStatsChangedCallable = Callable.From(OnStatsChanged);
    }
    

    private void OnStatsChanged()
    {
        if (!IsInstanceValid(this) || !IsInstanceValid(ManaLabel)) return;
        
        ManaLabel.Text = $"{CharacterStats.Mana}/{CharacterStats.MaxMana}";
    }
}
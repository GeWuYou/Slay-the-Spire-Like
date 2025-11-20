using Godot;
using SlayTheSpireLike.scripts.global;

namespace SlayTheSpireLike.scripts.enemies.battle;

public partial class BattleReward : Control
{
    [Export]
    public Button GoBackButton { get; set; }

    public override void _Ready()
    {
        GoBackButton.Pressed += () => Events.Instance.RaiseBattleRewardExited();
    }
}
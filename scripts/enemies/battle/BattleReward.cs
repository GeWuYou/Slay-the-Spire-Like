using Godot;
using SlayTheSpireLike.scripts.global;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.enemies.battle;

public partial class BattleReward : Control
{
    [Export]
    public Button GoBackButton { get; set; }

    [Export]
    public RunStats RunStats { get; set; }
    [Export]
    public VBoxContainer RewardContainer { get; set; }
    public override void _Ready()
    {
        GoBackButton.Pressed += () => Events.Instance.RaiseBattleRewardExited();
        foreach (var child in RewardContainer.GetChildren())
        {
            child.QueueFree();
        }
    }
    public void AddGoldReward(int amount)
    {
        var goldReward = ResourceFactories.RewardButtonFactory();
        goldReward.RewardIcon = ResourceFactories.GoldTextureFactory();
        goldReward.RewardText = string.Format(GameConstants.Texts.Gold,amount);
        goldReward.Pressed += () => OnGoldRewardTaken(amount);
        RewardContainer.CallDeferred(Node.MethodName.AddChild, goldReward);

    }

    private void OnGoldRewardTaken(int amount)
    {
        if (RunStats is null)
        {
            return;
        }
        RunStats.Gold += amount;
    }
}   
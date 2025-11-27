using Godot;
using SlayTheSpireLike.scripts.global;

namespace SlayTheSpireLike.scripts.resources;

[GlobalClass]
public partial class BattleStats : Resource
{
    [Export(PropertyHint.Range, "0,2")] public int BattleTier { get; set; }

    [Export(PropertyHint.Range, "0.0,10.0")]
    public float Weight { get; set; }

    [Export] public int GoldRewardMin { get; set; }
    [Export] public int GoldRewardMax { get; set; }
    [Export] public PackedScene Enemies { get; set; }
    public float AccumulatedWeight { get; set; }

    public int RollGoldReward()
    {
        return GlobalBean.RandomNumberGenerator.RandiRange(GoldRewardMin, GoldRewardMax);
    }
}
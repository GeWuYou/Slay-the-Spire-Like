using System.Linq;
using SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// 战斗统计信息池，用于管理不同等级的战斗统计数据，并支持根据权重随机选取战斗配置。
/// </summary>
[GlobalClass]
public partial class BattleStatsPool : Resource
{
    private Array<float> _totalWeightsByTier =
    [
        0.0f,
        0.0f,
        0.0f
    ];

    [Export] public Array<BattleStats> Pool { get; set; }

    /// <summary>
    /// 获取指定等级的所有战斗统计信息
    /// </summary>
    /// <param name="tier">战斗等级</param>
    /// <returns>包含指定等级所有战斗统计信息的数组</returns>
    public Array<BattleStats> GetAllBattleStatsForTier(int tier)
    {
        // 从池中筛选出战斗等级等于指定等级的统计信息，并转换为数组
        return new Array<BattleStats>(Pool.Where(stats => stats.BattleTier == tier).ToArray());
    }

    /// <summary>
    /// 为指定等级的战斗统计信息设置累积权重，以便后续进行加权随机选择
    /// </summary>
    /// <param name="tier">需要设置权重的战斗等级</param>
    public void SetupWeightForTier(int tier)
    {
        var battles = GetAllBattleStatsForTier(tier);
        _totalWeightsByTier[tier] = 0.0f;
        foreach (var battle in battles)
        {
            _totalWeightsByTier[tier] += battle.Weight;
            battle.AccumulatedWeight = _totalWeightsByTier[tier];
        }
    }

    /// <summary>
    /// 根据权重随机获取指定等级的一个战斗统计信息
    /// </summary>
    /// <param name="tier">目标战斗等级</param>
    /// <returns>随机选中的战斗统计信息；如果未找到则返回默认值</returns>
    public BattleStats GetRandomBattleStatsForTier(int tier)
    {
        var roll = GlobalBean.RandomNumberGenerator.RandfRange(0.0f, _totalWeightsByTier[tier]);
        var battles = GetAllBattleStatsForTier(tier);
        return battles.FirstOrDefault(battleStats => battleStats.AccumulatedWeight > roll);
    }

    /// <summary>
    /// 初始化所有等级（0-2）的战斗统计信息权重
    /// </summary>
    public void Setup()
    {
        for (var i = 0; i < 3; i++)
        {
            SetupWeightForTier(i);
        }
    }
}

namespace SlayTheSpireLike.scripts.enums;

/// <summary>
/// 遗物类型枚举，定义了遗物在游戏中的触发时机和行为类型
/// </summary>
public enum RelicType
{
    /// <summary>
    /// 回合开始时触发
    /// </summary>
    StartOfTurn,
    /// <summary>
    /// 战斗开始时触发
    /// </summary>
    StartOfCombat,
    /// <summary>
    /// 回合结束时触发
    /// </summary>
    EndOfTurn,
    /// <summary>
    /// 战斗结束时触发
    /// </summary>
    EndOfCombat,
    /// <summary>
    /// 基于事件触发
    /// </summary>
    EventBased
}

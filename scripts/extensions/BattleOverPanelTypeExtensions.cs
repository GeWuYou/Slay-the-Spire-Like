using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.extensions;

/// <summary>
///     BattleOverPanelTypeExtensions
/// </summary>
public static class BattleOverPanelTypeExtensions
{
    /// <summary>
    ///     判断战斗结束面板类型是否为胜利类型
    /// </summary>
    /// <param name="type">要判断的战斗结束面板类型</param>
    /// <returns>如果是胜利类型返回true，否则返回false</returns>
    public static bool IsWinType(this BattleOverPanel.Type type)
    {
        return type == BattleOverPanel.Type.Win;
    }

    /// <summary>
    ///     判断战斗结束面板类型是否为失败类型
    /// </summary>
    /// <param name="type">要判断的战斗结束面板类型</param>
    /// <returns>如果是失败类型返回true，否则返回false</returns>
    public static bool IsLoseType(this BattleOverPanel.Type type)
    {
        return type == BattleOverPanel.Type.Lose;
    }
}
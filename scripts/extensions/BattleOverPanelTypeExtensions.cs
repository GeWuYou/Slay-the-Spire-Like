using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.extensions;

/// <summary>
///     BattleOverPanelTypeExtensions
/// </summary>
public static class BattleOverPanelTypeExtensions
{
    /// <summary>
    ///     将战斗结束面板类型枚举转换为对应的整数值
    /// </summary>
    /// <param name="type">要转换的战斗结束面板类型枚举值</param>
    /// <returns>返回与战斗结束面板类型对应的整数值，未知类型返回0</returns>
    public static int GetBattleOverPanelTypeValue(this BattleOverPanel.Type type)
    {
        return type switch
        {
            BattleOverPanel.Type.Win => 0,
            BattleOverPanel.Type.Lose => 1,
            _ => 0
        };
    }

    /// <summary>
    ///     将整数值转换为对应的战斗结束面板类型枚举
    /// </summary>
    /// <param name="value">要转换的整数值</param>
    /// <returns>返回与整数对应的战斗结束面板类型枚举值，无效值返回Win类型</returns>
    public static BattleOverPanel.Type GetBattleOverPanelType(this int value)
    {
        return value switch
        {
            0 => BattleOverPanel.Type.Win,
            1 => BattleOverPanel.Type.Lose,
            _ => BattleOverPanel.Type.Win
        };
    }

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
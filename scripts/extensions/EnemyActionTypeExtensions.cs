using SlayTheSpireLike.scripts.enemies;

namespace SlayTheSpireLike.scripts.extensions;

/// <summary>
///     EnemyActionTypeExtensions
/// </summary>
public static class EnemyActionTypeExtensions
{
    /// <summary>
    ///     将敌人行动类型枚举转换为对应的整数值
    /// </summary>
    /// <param name="type">要转换的敌人行动类型枚举值</param>
    /// <returns>返回与敌人行动类型对应的整数值，未知类型返回0</returns>
    public static int GetEnemyActionTypeValue(this EnemyAction.Type type)
    {
        return type switch
        {
            EnemyAction.Type.Conditional => 0,
            EnemyAction.Type.ChanceBased => 1,
            _ => 0
        };
    }

    /// <summary>
    ///     将整数值转换为对应的敌人行动类型枚举
    /// </summary>
    /// <param name="value">要转换的整数值</param>
    /// <returns>返回与整数对应的敌人行动类型枚举值，无效值返回Conditional类型</returns>
    public static EnemyAction.Type GetEnemyActionType(this int value)
    {
        return value switch
        {
            0 => EnemyAction.Type.Conditional,
            1 => EnemyAction.Type.ChanceBased,
            _ => EnemyAction.Type.Conditional
        };
    }

    /// <summary>
    ///     判断敌人行动类型是否为条件类型
    /// </summary>
    /// <param name="type">要判断的敌人行动类型</param>
    /// <returns>如果是条件类型返回true，否则返回false</returns>
    public static bool IsConditionalType(this EnemyAction.Type type)
    {
        return type == EnemyAction.Type.Conditional;
    }

    /// <summary>
    ///     判断敌人行动类型是否为概率类型
    /// </summary>
    /// <param name="type">要判断的敌人行动类型</param>
    /// <returns>如果是概率类型返回true，否则返回false</returns>
    public static bool IsChanceBasedType(this EnemyAction.Type type)
    {
        return type == EnemyAction.Type.ChanceBased;
    }
}
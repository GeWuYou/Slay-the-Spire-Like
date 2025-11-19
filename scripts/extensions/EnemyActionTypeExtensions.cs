using SlayTheSpireLike.scripts.enemies;

namespace SlayTheSpireLike.scripts.extensions;

/// <summary>
///     EnemyActionTypeExtensions
/// </summary>
public static class EnemyActionTypeExtensions
{
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
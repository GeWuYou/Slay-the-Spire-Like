using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.extensions;

/// <summary>
///     CardTargetExtensions
/// </summary>
public static class CardTargetExtensions
{
    /// <summary>
    ///     判断目标是否为自身
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是自身返回true，否则返回false</returns>
    public static bool IsSelfTarget(this Card.Target target)
    {
        return target == Card.Target.Self;
    }

    /// <summary>
    ///     判断目标是否为敌人
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是敌人返回true，否则返回false</returns>
    public static bool IsEnemyTarget(this Card.Target target)
    {
        return target == Card.Target.Enemy;
    }

    /// <summary>
    ///     判断目标是否为单个目标（包括敌人或自己）
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是单个敌人或自己则返回true，否则返回false</returns>
    public static bool IsSingleTargeted(this Card.Target target)
    {
        // 检查是否为敌方目标或己方目标
        return IsEnemyTarget(target)
               // || IsAllyTarget(target)
               || IsSelfTarget(target)
            ;
    }


    /// <summary>
    ///     判断目标是否为全体
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是全体返回true，否则返回false</returns>
    public static bool IsAllTarget(this Card.Target target)
    {
        return target == Card.Target.All;
    }

    /// <summary>
    ///     判断目标是否为随机
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是随机返回true，否则返回false</returns>
    public static bool IsRandomTarget(this Card.Target target)
    {
        return target == Card.Target.Random;
    }

    /// <summary>
    ///     判断目标是否为随机敌人
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是随机敌人返回true，否则返回false</returns>
    public static bool IsRandomEnemyTarget(this Card.Target target)
    {
        return target == Card.Target.RandomEnemy;
    }

    /// <summary>
    ///     判断目标是否为随机盟友
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是随机盟友返回true，否则返回false</returns>
    public static bool IsRandomAllyTarget(this Card.Target target)
    {
        return target == Card.Target.RandomAlly;
    }

    /// <summary>
    ///     判断目标是否为所有敌人
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是所有敌人返回true，否则返回false</returns>
    public static bool IsAllEnemiesTarget(this Card.Target target)
    {
        return target == Card.Target.AllEnemies;
    }

    /// <summary>
    ///     判断目标是否为盟友
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是盟友返回true，否则返回false</returns>
    public static bool IsAllyTarget(this Card.Target target)
    {
        return target == Card.Target.Ally;
    }

    /// <summary>
    ///     判断目标是否为所有盟友
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是所有盟友返回true，否则返回false</returns>
    public static bool IsAllAlliesTarget(this Card.Target target)
    {
        return target == Card.Target.AllAllies;
    }

    /// <summary>
    ///     判断目标是否为己方角色
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是己方则返回true，否则返回false</returns>
    public static bool IsAllSelfTarget(this Card.Target target)
    {
        return target == Card.Target.AllSelf;
    }

    /// <summary>
    ///     判断指定的目标是否为随机自身目标
    /// </summary>
    /// <param name="target">要判断的卡片目标</param>
    /// <returns>如果目标是随机自身目标则返回true，否则返回false</returns>
    public static bool IsRandomSelfTarget(this Card.Target target)
    {
        return target == Card.Target.RandomSelf;
    }
}
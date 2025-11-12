using DeckBuilderTutorial.scripts.resources;

namespace DeckBuilderTutorial.scripts.extensions;

/// <summary>
/// CardTargetExtensions
/// </summary>
public static class CardTargetExtensions
{
    /// <summary>
    /// 将目标枚举转换为对应的整数值
    /// </summary>
    /// <param name="target">要转换的目标枚举值</param>
    /// <returns>返回与目标对应的整数值，未知目标返回0</returns>
    public static int GetTargetValue(this Card.Target target)
    {
        return target switch
        {
            Card.Target.Self => 0,
            Card.Target.Enemy => 1,
            Card.Target.All => 2,
            Card.Target.Random => 3,
            Card.Target.RandomEnemy => 4,
            Card.Target.RandomAlly => 5,
            Card.Target.AllEnemies => 6,
            Card.Target.Ally => 7,
            _ => 0
        };
    }

    /// <summary>
    /// 将整数值转换为对应的目标枚举
    /// </summary>
    /// <param name="value">要转换的整数值</param>
    /// <returns>返回与整数对应的目标枚举值，无效值返回Self目标</returns>
    public static Card.Target GetTarget(this int value)
    {
        return value switch
        {
            0 => Card.Target.Self,
            1 => Card.Target.Enemy,
            2 => Card.Target.All,
            3 => Card.Target.Random,
            4 => Card.Target.RandomEnemy,
            5 => Card.Target.RandomAlly,
            6 => Card.Target.AllEnemies,
            7 => Card.Target.Ally,
            _ => Card.Target.Self
        };
    }
    
    /// <summary>
    /// 判断目标是否为自身
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是自身返回true，否则返回false</returns>
    public static bool IsSelfTarget(this Card.Target target)
    {
        return target == Card.Target.Self;
    }
    
    /// <summary>
    /// 判断目标是否为敌人
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是敌人返回true，否则返回false</returns>
    public static bool IsEnemyTarget(this Card.Target target)
    {
        return target == Card.Target.Enemy;
    }

    public static bool IsSingleTargeted(this Card.Target target)
    {
        return IsEnemyTarget(target)||IsSelfTarget(target)||IsAllyTarget(target);
    }
    
    /// <summary>
    /// 判断目标是否为全体
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是全体返回true，否则返回false</returns>
    public static bool IsAllTarget(this Card.Target target)
    {
        return target == Card.Target.All;
    }
    
    /// <summary>
    /// 判断目标是否为随机
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是随机返回true，否则返回false</returns>
    public static bool IsRandomTarget(this Card.Target target)
    {
        return target == Card.Target.Random;
    }
    
    /// <summary>
    /// 判断目标是否为随机敌人
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是随机敌人返回true，否则返回false</returns>
    public static bool IsRandomEnemyTarget(this Card.Target target)
    {
        return target == Card.Target.RandomEnemy;
    }
    
    /// <summary>
    /// 判断目标是否为随机盟友
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是随机盟友返回true，否则返回false</returns>
    public static bool IsRandomAllyTarget(this Card.Target target)
    {
        return target == Card.Target.RandomAlly;
    }
    
    /// <summary>
    /// 判断目标是否为所有敌人
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是所有敌人返回true，否则返回false</returns>
    public static bool IsAllEnemiesTarget(this Card.Target target)
    {
        return target == Card.Target.AllEnemies;
    }
    
    /// <summary>
    /// 判断目标是否为盟友
    /// </summary>
    /// <param name="target">要判断的目标类型</param>
    /// <returns>如果目标是盟友返回true，否则返回false</returns>
    public static bool IsAllyTarget(this Card.Target target)
    {
        return target == Card.Target.Ally;
    }
}
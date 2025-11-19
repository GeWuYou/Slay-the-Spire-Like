using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.extensions;

/// <summary>
///     CardTypeExtensions
/// </summary>
public static class CardTypeExtensions
{
    /// <summary>
    ///     判断卡片类型是否为攻击类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是攻击类型返回true，否则返回false</returns>
    public static bool IsAttackCard(this Card.Type type)
    {
        return type == Card.Type.Attack;
    }

    /// <summary>
    ///     判断卡片类型是否为能力类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是能力类型返回true，否则返回false</returns>
    public static bool IsPowerCard(this Card.Type type)
    {
        return type == Card.Type.Power;
    }

    /// <summary>
    ///     判断卡片类型是否为技能类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是技能类型返回true，否则返回false</returns>
    public static bool IsSkillCard(this Card.Type type)
    {
        return type == Card.Type.Skill;
    }

    /// <summary>
    ///     判断卡片类型是否为状态类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是状态类型返回true，否则返回false</returns>
    public static bool IsStateCard(this Card.Type type)
    {
        return type == Card.Type.State;
    }

    /// <summary>
    ///     判断卡片类型是否为诅咒类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是诅咒类型返回true，否则返回false</returns>
    public static bool IsCurseCard(this Card.Type type)
    {
        return type == Card.Type.Curse;
    }
}
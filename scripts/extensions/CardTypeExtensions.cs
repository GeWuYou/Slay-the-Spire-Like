using DeckBuilderTutorial.scripts.resources;

namespace DeckBuilderTutorial.scripts.extensions;

/// <summary>
/// CardTypeExtensions
/// </summary>
public static class CardTypeExtensions
{
    /// <summary>
    /// 将卡片类型枚举转换为对应的整数值
    /// </summary>
    /// <param name="type">要转换的卡片类型枚举值</param>
    /// <returns>返回与卡片类型对应的整数值，未知类型返回0</returns>
    public static int GetCardTypeValue(this Card.Type type)
    {
        return type switch
        {
            Card.Type.Attack => 0,
            Card.Type.Power => 1,
            Card.Type.Skill => 2,
            Card.Type.State => 3,
            Card.Type.Curse => 4,
            _ => 0
        };
    }

    /// <summary>
    /// 将整数值转换为对应的卡片类型枚举
    /// </summary>
    /// <param name="value">要转换的整数值</param>
    /// <returns>返回与整数对应的卡片类型枚举值，无效值返回Attack类型</returns>
    public static Card.Type GetCardType(this int value)
    {
        return value switch
        {
            0 => Card.Type.Attack,
            1 => Card.Type.Power,
            2 => Card.Type.Skill,
            3 => Card.Type.State,
            4 => Card.Type.Curse,
            _ => Card.Type.Attack
        };
    }
    
    /// <summary>
    /// 判断卡片类型是否为攻击类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是攻击类型返回true，否则返回false</returns>
    public static bool IsAttackCard(this Card.Type type)
    {
        return type == Card.Type.Attack;
    }
    
    /// <summary>
    /// 判断卡片类型是否为能力类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是能力类型返回true，否则返回false</returns>
    public static bool IsPowerCard(this Card.Type type)
    {
        return type == Card.Type.Power;
    }
    
    /// <summary>
    /// 判断卡片类型是否为技能类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是技能类型返回true，否则返回false</returns>
    public static bool IsSkillCard(this Card.Type type)
    {
        return type == Card.Type.Skill;
    }
    
    /// <summary>
    /// 判断卡片类型是否为状态类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是状态类型返回true，否则返回false</returns>
    public static bool IsStateCard(this Card.Type type)
    {
        return type == Card.Type.State;
    }
    
    /// <summary>
    /// 判断卡片类型是否为诅咒类型
    /// </summary>
    /// <param name="type">要判断的卡片类型</param>
    /// <returns>如果是诅咒类型返回true，否则返回false</returns>
    public static bool IsCurseCard(this Card.Type type)
    {
        return type == Card.Type.Curse;
    }
}
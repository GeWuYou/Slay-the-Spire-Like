using SlayTheSpireLike.scripts.ui.state;

namespace SlayTheSpireLike.scripts.extensions;

/// <summary>
/// CardStateExtensions
/// </summary>
public static class CardStateExtensions
{
    /// <summary>
    /// 将卡片状态枚举转换为对应的整数值
    /// </summary>
    /// <param name="state">要转换的卡片状态枚举值</param>
    /// <returns>返回与卡片状态对应的整数值，未知状态返回0</returns>
    public static int GetCardStateValue(this CardState.State state)
    {
        // 使用模式匹配将枚举值映射到对应的整数
        return state switch
        {
            CardState.State.Base => 0,
            CardState.State.Clicked => 1,
            CardState.State.Dragging => 2,
            CardState.State.Aiming => 3,
            CardState.State.Released => 4,
            _ => 0
        };
    }

    /// <summary>
    /// 将整数值转换为对应的卡片状态枚举
    /// </summary>
    /// <param name="value">要转换的整数值</param>
    /// <returns>返回与整数对应的卡片状态枚举值，无效值返回Base状态</returns>
    public static CardState.State GetCardState(this int value)
    {
        // 使用模式匹配将整数值映射到对应的枚举值
        return value switch
        {
            0 => CardState.State.Base,
            1 => CardState.State.Clicked,
            2 => CardState.State.Dragging,
            3 => CardState.State.Aiming,
            4 => CardState.State.Released,
            _ => CardState.State.Base
        };
    }
}

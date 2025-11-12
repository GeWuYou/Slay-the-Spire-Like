using DeckBuilderTutorial.scripts.ui;
using Godot;

namespace DeckBuilderTutorial.scripts.global;

/// <summary>
/// 事件管理类，用于定义和处理游戏中的各种信号
/// 继承自Godot的Node类，可以在场景树中作为节点使用
/// </summary>
public partial class Events : Node
{
    /// <summary>
    /// 卡牌瞄准开始事件
    /// 当玩家开始瞄准一张卡牌时触发此事件
    /// </summary>
    /// <param name="cardUi">正在瞄准的卡牌UI对象</param>
    [Signal]
    public delegate void CardAimingStartedEventHandler(CardUi cardUi);

    /// <summary>
    /// 卡牌瞄准结束事件
    /// 当玩家结束瞄准一张卡牌时触发此事件
    /// </summary>
    /// <param name="cardUi">结束瞄准的卡牌UI对象</param>
    [Signal]
    public delegate void CardAimingEndedEventHandler(CardUi cardUi);
}
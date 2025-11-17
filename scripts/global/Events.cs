using Godot;
using SlayTheSpireLike.scripts.enemies;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.global;

/// <summary>
/// 事件管理类，用于定义和处理游戏中的各种信号
/// 继承自Godot的Node类，可以在场景树中作为节点使用
/// </summary>
public partial class Events : Node
{
    #region 实例代码

    public static Events Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    #endregion

    #region 卡牌事件

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

    /// <summary>
    /// 卡牌拖拽开始事件处理委托
    /// </summary>
    /// <param name="cardUi">正在被拖拽的卡牌UI对象</param>
    [Signal]
    public delegate void CardDraggingStartedEventHandler(CardUi cardUi);

    /// <summary>
    /// 卡牌拖拽结束事件处理委托
    /// </summary>
    /// <param name="cardUi">完成拖拽的卡牌UI对象</param>
    [Signal]
    public delegate void CardDraggingEndedEventHandler(CardUi cardUi);


    /// <summary>
    /// 卡牌打出事件处理委托
    /// </summary>
    /// <param name="card">被打出的卡牌对象</param>
    [Signal]
    public delegate void CardPlayedEventHandler(Card card);

    /// <summary>
    /// 卡片工具提示显示请求事件处理委托
    /// 当需要显示卡片工具提示时触发此事件
    /// </summary>
    /// <param name="icon">要显示在工具提示中的图标纹理</param>
    /// <param name="text">要显示在工具提示中的文本内容</param>
    [Signal]
    public delegate void CardToolTipShowRequestEventHandler(Texture icon, string text);


    /// <summary>
    /// 卡片工具提示隐藏请求事件处理委托
    /// 当需要隐藏卡片工具提示时触发此事件
    /// </summary>
    [Signal]
    public delegate void CardToolTipHideRequestEventHandler();

    #endregion

    #region 玩家

    /// <summary>
    /// 玩家手牌绘制事件处理委托
    /// </summary>
    /// <remarks>
    /// 当玩家手牌被绘制时触发此事件
    /// </remarks>
    [Signal]
    public delegate void PlayerHandDrawnEventHandler();

    /// <summary>
    /// 玩家手牌弃置事件处理器委托
    /// </summary>
    [Signal]
    public delegate void PlayerHandDiscardedEventHandler();

    /// <summary>
    /// 玩家回合结束事件处理器委托
    /// </summary>
    [Signal]
    public delegate void PlayerTurnEndedEventHandler();

    #endregion

    #region 敌人

    /// <summary>
    /// 敌人行为完成事件处理委托
    /// </summary>
    /// <param name="enemy">执行完行为的敌人对象</param>
    [Signal]
    public delegate void EnemyActionCompletedEventHandler(Enemy enemy);

    /// <summary>
    /// 敌人回合结束事件处理器委托
    /// </summary>
    /// <remarks>
    /// 该委托用于定义敌人回合结束时触发的事件处理方法签名。
    /// 当敌人的行动回合完成时，会调用此委托关联的所有事件处理方法。
    /// </remarks>
    [Signal]
    public delegate void EnemyTurnEndedEventHandler();

    #endregion
}
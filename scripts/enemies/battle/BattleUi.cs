using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts;

/// <summary>
///     战斗界面类，负责管理战斗过程中的UI元素和玩家交互
///     继承自CanvasLayer，作为战斗场景的UI层
/// </summary>
public partial class BattleUi : CanvasLayer
{
    /// <summary>
    ///     全局事件管理器实例，用于处理游戏内各种事件的订阅和触发
    /// </summary>
    private Events _events;

    /// <summary>
    ///     玩家角色属性数据
    /// </summary>
    public CharacterStats PlayerStats { get; set; }

    /// <summary>
    ///     法力值UI组件，用于显示和管理玩家的法力值
    /// </summary>
    [Export]
    public ManaUi ManaUi { get; set; }

    /// <summary>
    ///     手牌UI组件，用于显示和管理玩家的手牌
    /// </summary>
    [Export]
    public Hand Hand { get; set; }

    /// <summary>
    ///     结束回合按钮，玩家点击后结束当前回合
    /// </summary>
    [Export]
    public Button EndTurnButton { get; set; }
    
    [Export]
    public CardPileOpener DrawPileButton { get; set; }
    [Export]
    public CardPileOpener DiscardPileButton { get; set; }
    [Export]
    public CardPileView DrawPileView { get; set; }
    [Export]
    public CardPileView DiscardPileView { get; set; }

    /// <summary>
    ///     节点准备就绪时的回调方法
    ///     初始化各个UI组件的引用，设置事件监听器，绑定按钮点击事件
    /// </summary>
    public override void _Ready()
    {
        _events = Events.Instance;
        ManaUi.CharacterStats = PlayerStats;
        Hand.PlayerStats = PlayerStats;
        _events.PlayerHandDrawn += OnPlayerHandDrawn;
        EndTurnButton.Pressed += OnEndTurnButtonPressed;
        DiscardPileButton.Pressed += () =>  DiscardPileView.ShowCurrentView("弃牌堆");
        DrawPileButton.Pressed += () => DrawPileView.ShowCurrentView("抽牌堆", true);
    }
    public void InitCardPileUi()
    {
        DrawPileButton.CardPile = PlayerStats.DrawPile;
        DrawPileView.CardPile = PlayerStats.DrawPile;
        DiscardPileButton.CardPile = PlayerStats.Discard;
        DiscardPileView.CardPile = PlayerStats.Discard;
    }

    public override void _ExitTree()
    {
        if (_events != null)
        {
            _events.PlayerHandDrawn -= OnPlayerHandDrawn;
        }
        
        // 移除按钮事件监听器
        if (EndTurnButton != null)
        {
            EndTurnButton.Pressed -= OnEndTurnButtonPressed;
        }
    }

    /// <summary>
    ///     结束回合按钮按下时的处理方法
    ///     禁用结束回合按钮，防止重复点击，并发出玩家回合结束的信号
    /// </summary>
    private void OnEndTurnButtonPressed()
    {
        EndTurnButton.Disabled = true;
        _events.RaisePlayerTurnEnded();
    }

    /// <summary>
    ///     玩家手牌绘制完成时的回调方法
    ///     当玩家手牌绘制完成后，启用结束回合按钮
    /// </summary>
    private void OnPlayerHandDrawn()
    {
        EndTurnButton.Disabled = false;
    }
}
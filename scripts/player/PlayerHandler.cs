using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.player;

/// <summary>
/// 玩家处理器类，负责管理玩家的手牌、回合开始逻辑以及抽卡机制。
/// </summary>
public partial class PlayerHandler : Node
{
    [Export] private float _handDrawInterval = 0.25f;
    [Export] private float _handDiscardInterval = 0.25f;
    [Export] public Hand Hand { get; private set; }

    private Events _events;
    private CharacterStats _playerStats;

    /// <summary>
    /// 初始化方法，在节点准备就绪时调用。获取全局事件实例。
    /// </summary>
    public override void _Ready()
    {
        _events = Events.Instance;
        _events.CardPlayed+= OnCardPlayed;
    }

    /// <summary>
    /// 当卡片被使用时的回调处理函数
    /// </summary>
    /// <param name="card">被使用的卡片对象</param>
    private void OnCardPlayed(Card card)
    {
       // 将使用过的卡片添加到弃牌堆中
       _playerStats.Discard.AddCard(card);
    }


    /// <summary>
    /// 开始战斗，初始化玩家状态和牌堆。
    /// </summary>
    /// <param name="newStats">角色统计数据对象</param>
    public void StartBattle(CharacterStats newStats)
    {
        _playerStats = newStats;
        // 初始化抽牌堆并洗牌
        _playerStats.DrawPile = newStats.Deck.Duplicate(true) as CardPile;
        _playerStats.DrawPile?.Shuffle();
        // 初始化弃牌堆和移除牌堆
        _playerStats.Discard = new CardPile();
        _playerStats.RemovedDeck = new CardPile();
        StartTurn();
    }

    /// <summary>
    /// 开始新的回合，重置护盾值和法力值，并抽取指定数量的卡牌。
    /// </summary>
    public void StartTurn()
    {
        _playerStats.Block = 0;
        _playerStats.ResetMana();
        DrawCards(_playerStats.CardsPerTurn);
    }

    /// <summary>
    /// 结束当前回合的操作
    /// </summary>
    /// <remarks>
    /// 该函数负责处理回合结束时的逻辑，包括禁用手牌区域和执行弃牌操作
    /// </remarks>
    public void EndTurn()
    {
        Hand.DisableHand();
        DisableCards();
    }

    /// <summary>
    /// 禁用所有手牌并执行弃牌动画
    /// </summary>
    /// <remarks>
    /// 遍历手牌容器中的所有子元素，对每个CardUi组件执行以下操作：
    /// 1. 将卡片添加到玩家的弃牌堆中
    /// 2. 从手牌区域移除该卡片
    /// 3. 在每个操作之间添加时间间隔以创建动画效果
    /// </remarks>
    private void DisableCards()
    {
        var tween = CreateTween();
        
        // 遍历手牌容器中的所有子元素并执行弃牌动画
        foreach (var child in Hand.GetChildren())
        {
            if (child is not CardUi cardUi)
            {
                continue;
            }

            tween.TweenCallback(Callable.From(() => _playerStats.Discard.AddCard(cardUi.Card)));
            tween.TweenCallback(Callable.From(() => Hand.DiscardCard(cardUi)));
            tween.TweenInterval(_handDiscardInterval);
        }
        tween.Finished += () => { _events.EmitSignal(SlayTheSpireLike.scripts.global.Events.SignalName.PlayerHandDiscarded); };
    }


    /// <summary>
    /// 抽取指定数量的卡牌，并在完成后发出信号。
    /// </summary>
    /// <param name="cardsPerTurn">需要抽取的卡牌数量</param>
    private void DrawCards(int cardsPerTurn)
    {
        var tween = CreateTween();
        // 使用Tween逐张绘制卡牌，每张卡牌之间有间隔时间
        for (var i = 0; i < cardsPerTurn; i++)
        {
            tween.TweenCallback(Callable.From(DrawCard));
            tween.TweenInterval(_handDrawInterval);
        }

        // 所有卡牌绘制完成后的回调处理
        tween.Finished += () => { _events.EmitSignal(SlayTheSpireLike.scripts.global.Events.SignalName.PlayerHandDrawn); };
    }

    /// <summary>
    /// 从抽牌堆中抽取一张卡牌并添加到手牌中。
    /// </summary>
    private void DrawCard()
    {
        ReshuffleDeckFromDiscard();
        Hand.AddCard(_playerStats.DrawPile.DrawCard());
        ReshuffleDeckFromDiscard();
    }

    /// <summary>
    /// 当抽牌堆为空时，将弃牌堆中的所有卡牌重新洗牌后放入抽牌堆
    /// </summary>
    private void ReshuffleDeckFromDiscard()
    {
        // 如果抽牌堆不为空，则无需重洗牌
        if (!_playerStats.DrawPile.IsEmpty())
        {
            return;
        }

        // 将弃牌堆中的所有卡牌移动到抽牌堆中
        while (!_playerStats.Discard.IsEmpty())
        {
            _playerStats.DrawPile.AddCard(_playerStats.Discard.DrawCard());
        }
        
        // 对抽牌堆进行洗牌
        _playerStats.DrawPile.Shuffle();
    }

}
using DeckBuilderTutorial.scripts.resources;
using DeckBuilderTutorial.scripts.ui;
using global::DeckBuilderTutorial.scripts.global;
using Godot;

namespace DeckBuilderTutorial.scripts.player;

/// <summary>
/// 玩家处理器类，负责管理玩家的手牌、回合开始逻辑以及抽卡机制。
/// </summary>
public partial class PlayerHandler : Node
{
    [Export]
    private float _handDrawInterval = 0.25f;
    [Export]
    public Hand Hand { get; private set; }

    private Events _events;
    private CharacterStats _playerStats;

    /// <summary>
    /// 初始化方法，在节点准备就绪时调用。获取全局事件实例。
    /// </summary>
    public override void _Ready()
    {
        _events = Events.Instance;
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
    private void StartTurn()
    {
        _playerStats.Block = 0;
        _playerStats.ResetMana();
        DrawCards(_playerStats.CardsPerTurn);
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
        tween.Finished+=()=>
        {
            _events.EmitSignal(Events.SignalName.PlayerHandDrawn);
        };
    }

    /// <summary>
    /// 从抽牌堆中抽取一张卡牌并添加到手牌中。
    /// </summary>
    private void DrawCard()
    {
        Hand.AddCard(_playerStats.DrawPile.DrawCard());
    }
}

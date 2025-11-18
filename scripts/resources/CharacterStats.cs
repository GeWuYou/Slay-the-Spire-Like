using Godot;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
///     角色统计信息类，继承自Stats基类
///     管理角色的游戏数据，包括卡牌堆、法力值等战斗相关属性
/// </summary>
[GlobalClass]
public partial class CharacterStats : Stats
{
    private int _mana;

    /// <summary>
    ///     起始卡组，通过Godot导出属性可在编辑器中配置
    /// </summary>
    [Export]
    public CardPile StartingDeck { private set; get; }

    /// <summary>
    ///     每回合抽卡数量，默认为5张
    /// </summary>
    [Export]
    public int CardsPerTurn { private set; get; } = 5;

    /// <summary>
    ///     最大法力值，默认为3点
    /// </summary>
    [Export]
    public int MaxMana { private set; get; } = 3;

    /// <summary>
    ///     当前法力值属性
    ///     设置时会触发StatsChanged信号通知UI更新
    /// </summary>
    public int Mana
    {
        get => _mana;
        set
        {
            _mana = value;
            EmitSignal(Stats.SignalName.StatsChanged);
        }
    }

    /// <summary>
    ///     当前游戏中的完整卡组
    /// </summary>
    public CardPile Deck { get; set; }

    /// <summary>
    ///     弃牌堆
    /// </summary>
    public CardPile Discard { get; set; }

    /// <summary>
    ///     抽牌堆
    /// </summary>
    public CardPile DrawPile { get; set; }

    /// <summary>
    ///     移除牌堆（被移出游戏的卡牌）
    /// </summary>
    public CardPile RemovedDeck { get; set; }

    /// <summary>
    ///     重置法力值为最大值
    ///     通常在新回合开始时调用
    /// </summary>
    public void ResetMana()
    {
        Mana = MaxMana;
    }

    /// <summary>
    ///     判断是否可以使用指定卡牌
    ///     通过比较当前法力值与卡牌消耗来判断
    /// </summary>
    /// <param name="card">要判断的卡牌</param>
    /// <returns>如果当前法力值足够支付卡牌消耗则返回true，否则返回false</returns>
    public bool CanPlayCard(Card card)
    {
        return Mana >= card.Cost;
    }

    /// <summary>
    ///     创建角色统计数据的实例副本
    ///     初始化各个卡牌堆并重置法力值
    /// </summary>
    /// <returns>新的CharacterStats实例，如果创建失败则返回null</returns>
    public override CharacterStats CreateInstance()
    {
        var instance = base.CreateInstance<CharacterStats>();
        // 初始化游戏状态相关属性
        instance.ResetMana();
        instance.Deck = StartingDeck.Duplicate() as CardPile;
        instance.Discard = new CardPile();
        instance.DrawPile = new CardPile();
        instance.RemovedDeck = new CardPile();
        return instance;
    }
}
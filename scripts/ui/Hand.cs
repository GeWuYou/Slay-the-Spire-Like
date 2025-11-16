using DeckBuilderTutorial.scripts.resources;
using global::DeckBuilderTutorial.scripts.global;
using Godot;

namespace DeckBuilderTutorial.scripts.ui;

/// <summary>
/// 手牌容器类，用于管理手牌中的卡片UI元素
/// 继承自HBoxContainer，自动水平排列子节点
/// </summary>
public partial class Hand : HBoxContainer
{
    private int _cardsPlayedThisTurn;
    public CharacterStats PlayerStats { get; set; }
    private Events _events;
    
    /// <summary>
    /// 当节点准备就绪时调用此方法
    /// 遍历所有子节点，为卡片UI组件建立事件连接并设置父节点引用
    /// </summary>
    public override void _Ready()
    {
        _events = Events.Instance; 
        _events.CardPlayed += OnCardPlayed;
    }
    
    /// <summary>
    /// 禁用手牌中的所有卡牌
    /// </summary>
    /// <remarks>
    /// 遍历手牌容器中的所有子元素，将CardUi类型的卡牌组件设置为禁用状态
    /// </remarks>
    public void DisableHand()
    {
        // 遍历所有子卡牌并禁用CardUi组件
        foreach (var card in GetChildren())
        {
            if(card is not CardUi cardUi)
            {
                continue;
            }
            cardUi.Disabled = true;
            
        }
    }

    public static void DiscardCard(CardUi card)
    {
        card.QueueFree();
    }

    /// <summary>
    /// 向当前容器中添加一张卡片UI元素
    /// </summary>
    /// <param name="card">要添加的卡片数据对象</param>
    public void AddCard(Card card)
    {
        // 创建卡片UI实例并添加到子元素中
        var cardUi = CardUi.CreateInstance();
        AddChild(cardUi);
        
        // 注册卡片重定位请求事件
        cardUi.ReparentRequested += OnCardReparentRequested;
        
        // 设置卡片UI的相关属性
        cardUi.Card = card;
        cardUi.Parent = this;
        cardUi.CharacterStats = PlayerStats;
    }


    /// <summary>
    /// 卡片被使用时的回调方法，增加本回合已使用的卡牌计数
    /// </summary>
    /// <param name="card">被使用的卡片实例</param>
    private void OnCardPlayed(Card card)
    {
        _cardsPlayedThisTurn++;
    }

    /// <summary>
    /// 处理卡片重定位请求的回调方法
    /// 当卡片请求重新设置父节点时，将其重新定位到当前手牌容器中，并调整其在容器内的位置
    /// </summary>
    /// <param name="cardUi">请求重定位的卡片UI组件</param>
    public void OnCardReparentRequested(CardUi cardUi)
    {
        cardUi.Reparent(this);
        var newIndex = Mathf.Clamp(cardUi.OriginalIndex - _cardsPlayedThisTurn, 0, GetChildCount());
        MoveChild(cardUi,newIndex);
    }
}

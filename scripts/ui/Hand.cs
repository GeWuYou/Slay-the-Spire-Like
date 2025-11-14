using DeckBuilderTutorial.scripts.player;
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
    [Export] private Player _player;

    /// <summary>
    /// 当节点准备就绪时调用此方法
    /// 遍历所有子节点，为卡片UI组件建立事件连接并设置父节点引用
    /// </summary>
    public override void _Ready()
    {
        Events.Instance.CardPlayed += OnCardPlayed;
        
        // 遍历所有子节点，为卡片UI组件建立事件连接
        foreach (var child in GetChildren())
        {
            // 检查子节点是否为CardUi类型，如果不是则跳过
            if (child is not CardUi cardUi)
            {
                continue;
            }
            
            // 连接卡片的重定位请求信号到处理方法
            cardUi.Connect(CardUi.SignalName.ReparentRequested,
                Callable.From((CardUi card) => OnCardReparentRequested(card)));
            
            // 设置卡片的父节点引用为当前手牌容器
            cardUi.Parent = this;
            cardUi.CharacterStats = _player.Stats;
        }
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

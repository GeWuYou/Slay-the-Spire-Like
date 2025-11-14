using DeckBuilderTutorial.scripts.player;
using Godot;

namespace DeckBuilderTutorial.scripts.ui;

/// <summary>
/// 手牌容器类，用于管理手牌中的卡片UI元素
/// 继承自HBoxContainer，自动水平排列子节点
/// </summary>
public partial class Hand : HBoxContainer
{
    [Export] private Player _player;
    /// <summary>
    /// 当节点准备就绪时调用此方法
    /// 遍历所有子节点，为卡片UI组件建立事件连接并设置父节点引用
    /// </summary>
    public override void _Ready()
    {
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
    /// 处理卡片重定位请求的回调方法
    /// 当卡片请求重新设置父节点时，将其重新定位到当前手牌容器中
    /// </summary>
    /// <param name="cardUi">请求重定位的卡片UI组件</param>
    public void OnCardReparentRequested(CardUi cardUi)
    {
        cardUi.Reparent(this);
    }
}

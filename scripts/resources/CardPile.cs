using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Godot.NativeInterop;

namespace DeckBuilderTutorial.scripts.resources;

/// <summary>
/// 表示一个卡牌堆资源，用于管理一组卡片的集合
/// 提供抽卡、添加卡牌、洗牌等基本操作
/// </summary>
public partial class CardPile : Resource
{
    /// <summary>
    /// 当卡牌堆大小发生变化时触发的信号
    /// </summary>
    /// <param name="cardsAmount">当前卡牌堆中的卡牌数量</param>
    [Signal]
    public delegate void CardPileSizeChangedEventHandler(int cardsAmount);

    /// <summary>
    /// 存储卡牌的数组，可通过编辑器导出设置
    /// </summary>
    [Export]
    public Array<Card> Cards { private set; get; } = [];

    /// <summary>
    /// 检查卡牌堆是否为空
    /// </summary>
    /// <returns>如果卡牌堆中没有卡牌则返回true，否则返回false</returns>
    public bool IsEmpty()
    {
        return Cards.Count == 0;
    }

    /// <summary>
    /// 从卡牌堆顶部抽取一张卡牌
    /// </summary>
    /// <returns>抽取的卡牌对象，如果卡牌堆为空则返回null</returns>
    public Card DrawCard()
    {
        // 检查卡牌堆是否为空
        if (IsEmpty())
        {
            return null;
        }

        var card = Cards[0];
        Cards.RemoveAt(0);
        EmitSignal(SignalName.CardPileSizeChanged, Cards.Count);
        return card;
    }

    /// <summary>
    /// 向卡牌堆中添加一张卡牌
    /// </summary>
    /// <param name="card">要添加的卡牌对象</param>
    public void AddCard(Card card)
    {
        Cards.Add(card);
        EmitSignal(SignalName.CardPileSizeChanged, Cards.Count);
    }

    /// <summary>
    /// 随机打乱卡牌堆中卡牌的顺序
    /// </summary>
    public void Shuffle()
    {
        Cards.Shuffle();
    }

    /// <summary>
    /// 清空卡牌堆中的所有卡牌
    /// </summary>
    public void Clear()
    {
        Cards.Clear();
        EmitSignal(SignalName.CardPileSizeChanged, Cards.Count);
    }

    /// <summary>
    /// 将卡牌堆中的所有卡牌信息转换为字符串表示
    /// </summary>
    /// <returns>包含所有卡牌信息的字符串，每张卡牌占一行</returns>
    public string CardPileToString()
    {
        // 将每张卡牌转换为带序号的字符串形式
        var cardStrings = Cards.Select((card, i) => $"{i + 1}: {card}").ToList();
        return string.Join("\n", cardStrings);
    }
}
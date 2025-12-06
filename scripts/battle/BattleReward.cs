using System.Linq;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.extensions;
using SlayTheSpireLike.scripts.global;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.battle;

/// <summary>
/// 战斗奖励界面控制类。用于展示战斗胜利后可获得的奖励（如金币、卡牌等），
/// 并处理玩家选择奖励后的逻辑。
/// </summary>
public partial class BattleReward : Control
{
    [Export]
    public Button GoBackButton { get; set; }

    [Export]
    public RunStats RunStats { get; set; }
    
    [Export]
    public CharacterStats PlayerStats { get; set; }
    
    [Export]
    public VBoxContainer RewardContainer { get; set; }
    
    [Export]
    public RelicHandler RelicHandler { get; set; }

    /// <summary>
    /// 所有卡牌稀有度对应的权重总和。
    /// </summary>
    private float _cardRewardTotalWeight;

    /// <summary>
    /// 各个稀有度卡牌在随机时所占的累积概率权重字典。
    /// 键为卡牌稀有度，值为其累计到当前稀有度为止的概率上限。
    /// </summary>
    private Dictionary<Card.Rarity, float> _cardRewardWeights = new()
    {
        {Card.Rarity.Common, 0f},
        {Card.Rarity.Uncommon, 0f},
        {Card.Rarity.Rare, 0f}
    };

    /// <summary>
    /// 初始化节点，在场景加载完成后执行。
    /// 绑定“返回”按钮点击事件，并清空奖励容器中的所有子元素。
    /// </summary>
    public override void _Ready()
    {
        GoBackButton.Pressed += () => Events.Instance.RaiseBattleRewardExited();
        foreach (var child in RewardContainer.GetChildren())
        {
            child.QueueFree();
        }
    }

    /// <summary>
    /// 添加一个金币奖励项至奖励列表中。
    /// </summary>
    /// <param name="amount">要添加的金币数量。</param>
    public void AddGoldReward(int amount)
    {
        if (amount == 0)
        {
            return;
        }
        var goldReward = ResourceFactories.RewardButtonFactory();
        goldReward.RewardIcon = ResourceFactories.GoldTextureFactory();
        goldReward.RewardText = string.Format(GameConstants.Texts.Gold,amount);
        goldReward.Pressed += () => OnGoldRewardTaken(amount);
        RewardContainer.CallDeferred(Node.MethodName.AddChild, goldReward);
    }

    /// <summary>
    /// 添加一个新的卡牌奖励选项至奖励列表中。
    /// 点击该选项将弹出可供选取的新卡牌列表。
    /// </summary>
    public void AddCardReward()
    {
        var cardReward = ResourceFactories.RewardButtonFactory();
        cardReward.RewardIcon = ResourceFactories.CardTextureFactory();
        cardReward.RewardText = GameConstants.Texts.AddNewCard;
        cardReward.Pressed +=ShowCardRewards;
        RewardContainer.CallDeferred(Node.MethodName.AddChild, cardReward);
    }
    
    public void AddRelicReward(Relic relic)
    {
        var relicReward = ResourceFactories.RewardButtonFactory();
        relicReward.RewardIcon = relic.Icon;
        relicReward.RewardText = relic.RelicName;
        relicReward.Pressed += () => { OnRelicRewardTaken(relic);};
        RewardContainer.CallDeferred(Node.MethodName.AddChild, relicReward);
    }

    private void OnRelicRewardTaken(Relic relic)
    {
        if (PlayerStats is null || RelicHandler is null)
        {
            return;
        }
        RelicHandler.AddRelic(relic);
    }

    /// <summary>
    /// 显示可供选择的一组新卡牌奖励。
    /// 根据运行状态与角色信息从可用卡池中按权重抽取指定数量的卡牌作为奖励。
    /// </summary>
    private void ShowCardRewards()
    {
        // 若缺少必要的统计数据或角色数据则直接退出
        if (RunStats is null || PlayerStats is null)
        {
            return;
        }

        var cardRewards = ResourceFactories.CardRewardsFactory();
        AddChild(cardRewards);
        cardRewards.CardRewardSelected += OnCardRewardTaken;

        var cardRewardArray = new Array<Card>();
        
        // 复制一份可用卡牌副本以避免修改原始数据
        var availableCards =  PlayerStats.DraftablePile.Cards.Duplicate(true);
        // 抽取若干张卡牌作为本次奖励
        for (var i = 0; i < RunStats.BaseCardRewards; i++)
        {
            SetUpCardChances();

            // 随机生成一个浮点数来决定抽取哪种稀有度的卡牌
            var roll = GlobalBean.RandomNumberGenerator.RandfRange(0.0f, _cardRewardTotalWeight);

            foreach (var rarity in _cardRewardWeights.Keys)
            {
                // 当前roll未落在该稀有度区间内则跳过
                if (_cardRewardWeights[rarity] <= roll)
                {
                    continue;
                }

                // 调整后续抽卡权重
                ModifyWeight(rarity);

                // 获取一张符合要求的随机卡牌并加入结果集
                var pickedCard = GetRandomAvailableCard(availableCards, rarity);
                cardRewardArray.Add(pickedCard);
                availableCards.Remove(pickedCard);
                break;
            }
        }

        cardRewards.Rewards = cardRewardArray;
        cardRewards.Show();
    }

    /// <summary>
    /// 从给定的卡牌集合中根据指定稀有度随机挑选一张卡牌。
    /// </summary>
    /// <param name="availableCards">可供筛选的卡牌数组。</param>
    /// <param name="rarityKey">目标卡牌的稀有度类型。</param>
    /// <returns>符合条件的一个随机卡牌对象。</returns>
    private Card GetRandomAvailableCard(Array<Card> availableCards, Card.Rarity rarityKey)
    {
        var allPossibleCards =  availableCards.Where(card => card.CardRarity == rarityKey).ToArray();
        
        // 如果该稀有度没有可用卡牌，尝试从所有剩余卡牌中随机选择
        if (allPossibleCards.Length == 0)
        {
            GD.PushWarning($"没有找到稀有度为 {rarityKey} 的卡牌，从所有剩余卡牌中随机选择");
            return availableCards.Count > 0 ? availableCards.PickRandom() : null;
        }
        
        return allPossibleCards.PickRandom();
    }

    /// <summary>
    /// 修改下一次卡牌抽取时各稀有度的权重配置。
    /// 特别地，如果刚抽出的是稀有卡，则重置稀有权重；否则提升稀有卡出现几率。
    /// </summary>
    /// <param name="rarityKey">刚刚被选中的卡牌稀有度。</param>
    private void ModifyWeight(Card.Rarity rarityKey)
    {
        if (rarityKey == Card.Rarity.Rare)
        {
            RunStats.BaseRareWeight = RunStats.BaseCommonWeightValue;
        }
        else
        {
            RunStats.BaseRareWeight = (float)Mathf.Clamp(RunStats.BaseRareWeight + 0.3, RunStats.BaseRareWeightValue, 5.0f);
        }
    }

    /// <summary>
    /// 设置每种稀有度卡牌在本轮抽奖中的累计权重分布。
    /// </summary>
    private void SetUpCardChances()
    {
        _cardRewardTotalWeight = RunStats.BaseCommonWeight+RunStats.BaseUncommonWeight+RunStats.BaseRareWeight;
        _cardRewardWeights[Card.Rarity.Common] = RunStats.BaseCommonWeight;
        _cardRewardWeights[Card.Rarity.Uncommon] = RunStats.BaseCommonWeight+RunStats.BaseUncommonWeight;
        _cardRewardWeights[Card.Rarity.Rare] = _cardRewardTotalWeight;
    }

    /// <summary>
    /// 处理玩家选择了某张卡牌奖励的操作：将其添加进角色卡组。
    /// </summary>
    /// <param name="card">玩家选择的卡牌对象。</param>
    private void OnCardRewardTaken(Card card)
    {
        if (PlayerStats is null || card is null)
        {
            return;
        }
        PlayerStats.Deck.AddCard(card);
    }

    /// <summary>
    /// 处理玩家领取了金币奖励的操作：增加其拥有的金币总量。
    /// </summary>
    /// <param name="amount">领取的金币数量。</param>
    private void OnGoldRewardTaken(int amount)
    {
        if (RunStats is null)
        {
            return;
        }
        RunStats.Gold += amount;
    }
}

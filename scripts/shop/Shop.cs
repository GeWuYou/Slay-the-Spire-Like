using System.Linq;
using SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.extensions;
using SlayTheSpireLike.scripts.modifier_handler;
using SlayTheSpireLike.scripts.random;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.relic;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.shop;
/// <summary>
/// 商店界面控制类，用于展示可购买的卡牌与遗物，并处理玩家在商店中的交互逻辑。
/// </summary>
public partial class Shop : Control
{
    /// <summary>
    /// 可供出售的遗物列表（由编辑器导出）
    /// </summary>
    [Export]
    public Array<Relic> ShopRelics{get; set;}

    /// <summary>
    /// 玩家角色状态数据（由编辑器导出）
    /// </summary>
    [Export]
    public CharacterStats PlayerStats { get; set; }

    /// <summary>
    /// 当前游戏运行统计数据（如金币等）（由编辑器导出）
    /// </summary>
    [Export]
    public RunStats RunStats { get; set; }

    /// <summary>
    /// 遗物管理器，负责添加新获得的遗物（由编辑器导出）
    /// </summary>
    [Export]
    public RelicHandler RelicHandler { get; set; }

    /// <summary>
    /// 返回按钮控件（由编辑器导出）
    /// </summary>
    [Export]
    public TextureButton GoBackButton { get; set; }

    /// <summary>
    /// 显示商店中卡牌的容器节点（由编辑器导出）
    /// </summary>
    [Export]
    public HBoxContainer CardsContainer { get; set; }

    /// <summary>
    /// 显示商店中遗物的容器节点（由编辑器导出）
    /// </summary>
    [Export]
    public HBoxContainer RelicsContainer { get; set; }

    /// <summary>
    /// 卡牌提示弹窗组件（由编辑器导出）
    /// </summary>
    [Export]
    public CardTooltipPopup CardTooltipPopup { get; set; }
    [Export]
    public AnimationPlayer AnimationPlayer { get; set; }

    [Export]
    public ModifierHandler ModifierHandler { get; set; }
    [Export]
    public Timer Timer { get; set; }
    /// <summary>
    /// 初始化商店界面，在_ready时绑定事件、清理旧子项并注册输入监听。
    /// </summary>
    public override void _Ready()
    {
        // 绑定返回按钮点击事件：触发退出商店事件
        GoBackButton.Pressed += () => Events.Instance.RaiseShopExited();

        // 清理卡片和遗物容器内的所有现有子节点
        foreach (var child in CardsContainer.GetChildren())
        {
            child.QueueFreeX();
        }
        foreach (var child in RelicsContainer.GetChildren())
        {
            child.QueueFreeX();
        }

        // 注册全局GUI输入事件回调
        GuiInput += OnGuiInput;

        // 订阅卡牌/遗物被购买的事件
        Events.Instance.ShopCardBought += OnShopCardBought;
        Events.Instance.ShopRelicBought += OnShopRelicBought;
        BlinkTimerSetup();
        Timer.Timeout+=OnTimeout;
    }

    private void OnTimeout()
    {
        AnimationPlayer.Play("眨眼");
        BlinkTimerSetup();
    }

    private void BlinkTimerSetup()
    {
        Timer.WaitTime = RandomNumberProvider.Instance.RandomNumberGenerator.RandfRange(1.0f, 5.0f);
        Timer.Start();
    }

    public override void _ExitTree()
    {
        Events.Instance.ShopCardBought -= OnShopCardBought;
        Events.Instance.ShopRelicBought -= OnShopRelicBought;
    }

    /// <summary>
    /// 处理商店中卡牌被购买后的逻辑：将卡加入玩家卡组、扣除金币并刷新UI显示。
    /// </summary>
    /// <param name="card">被购买的卡牌对象</param>
    /// <param name="goldCost">该卡牌的价格</param>
    private void OnShopCardBought(Card card, int goldCost)
    {
        PlayerStats.Deck.AddCard(card);       // 将卡牌加入玩家卡组
        RunStats.Gold -= goldCost;            // 扣除对应金币数量
        UpdateItem();                         // 刷新商店物品价格显示
    }
    
    /// <summary>
    /// 处理商店中遗物被购买后的逻辑：添加到遗物系统、扣除金币并刷新UI显示。
    /// </summary>
    /// <param name="relic">被购买的遗物对象</param>
    /// <param name="goldCost">该遗物的价格</param>
    private void OnShopRelicBought(Relic relic, int goldCost)
    {
        RelicHandler.AddRelic(relic);         // 添加遗物至玩家持有列表
        RunStats.Gold -= goldCost;            // 扣除对应金币数量
        if (relic is Coupons coupons)
        {
            coupons.AddShopModifier(this);
            UpdateItemCosts();
        }
        UpdateItem();                         // 刷新商店物品价格显示
    }

    /// <summary>
    /// 监听鼠标左键按下以隐藏卡牌提示框。
    /// </summary>
    /// <param name="event">当前输入事件</param>
    private void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            CardTooltipPopup.HideTooltip();   // 左键点击任意处则关闭提示窗口
        }
    }

    /// <summary>
    /// 全局按键输入处理方法，当按下取消键且提示框可见时隐藏它。
    /// </summary>
    /// <param name="event">当前输入事件</param>
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && CardTooltipPopup.Visible)
        {
            CardTooltipPopup.HideTooltip();   // 按下ESC或取消键时关闭提示窗口
        }
    }
    
    /// <summary>
    /// 填充商店内容，包括生成随机卡牌和遗物供玩家选择购买。
    /// </summary>
    public void PopulateShop()
    {
        GenerateShopCards();                  // 生成商店卡牌
        GenerateShopRelics();                 // 生成商店遗物
    }

    /// <summary>
    /// 根据玩家当前可用卡池生成三个随机商店卡牌。
    /// </summary>
    private void GenerateShopCards()
    {
        // 获取玩家当前可以抽取的卡牌副本并打乱顺序
        var availableCards = PlayerStats.DraftablePile.DuplicateCards();
        availableCards.Shuffle();

        // 截取前三张作为商店卡牌
        var shopCardArray = availableCards[..3];

        // 实例化并配置每一张商店卡牌UI元素
        foreach (var card in shopCardArray)
        {
            var newShopCard = ResourceFactories.ShopCardFactory();     // 创建新的商店卡牌实例
            CardsContainer.AddChild(newShopCard);                     // 加入UI容器
            newShopCard.Card = card;                                  // 设置对应的卡牌数据

            // 连接卡牌视觉组件的提示请求信号到全局提示弹窗
            newShopCard.CurrentCardMenuUi.CardVisuals.Connect(
                CardVisuals.SignalName.TooltipRequested,
                new Callable(CardTooltipPopup, CardTooltipPopup.MethodName.ShowTooltip));
            newShopCard.GoldCost = GetUpdatedShopCost(newShopCard.GoldCost);

            newShopCard.Update(RunStats);                             // 更新其显示信息（例如价格）
        }
    }

    /// <summary>
    /// 根据商店提供的遗物池筛选并生成三个随机商店遗物。
    /// </summary>
    private void GenerateShopRelics()
    {
        // 筛选出满足出现条件并且尚未拥有的遗物
        var availableCards = new Array<Relic>(
            ShopRelics.Where(relic =>
                relic.CanAppearAsReward(PlayerStats) &&
                !RelicHandler.HasRelic(relic.Id)).ToArray());
        // 打乱顺序         
        RandomNumberProvider.Instance.ArrayShuffle(availableCards);               
        var shopRelicArray = availableCards[..3];                     // 取前三个作为商店遗物

        // 实例化并配置每一个商店遗物UI元素
        foreach (var relic in shopRelicArray)
        {
            var newShopRelic = ResourceFactories.ShopRelicFactory();  // 创建新的商店遗物实例
            RelicsContainer.AddChild(newShopRelic);                   // 加入UI容器
            newShopRelic.Relic = relic;                               // 设置对应的遗物数据
            newShopRelic.GoldCost = GetUpdatedShopCost(newShopRelic.GoldCost);
            newShopRelic.Update(RunStats);                            // 更新其显示信息（例如价格）
        }
    }

    private void UpdateItemCosts()
    {
        foreach (var child in CardsContainer.GetChildren())
        {
            if (child is not ShopCard shopCard)
            {
                continue;
            }

            shopCard.GoldCost = GetUpdatedShopCost(shopCard.GoldCost);
        }

        foreach (var child in RelicsContainer.GetChildren())
        {
            if (child is not ShopRelic shopRelic)
            {
                continue;
            }

            shopRelic.GoldCost = GetUpdatedShopCost(shopRelic.GoldCost);
        }
    }
    private int GetUpdatedShopCost(int goldCost)
    {
        return ModifierHandler.GetModifiedValue(Modifier.ModifierType.ShopCost, goldCost);
    }

    /// <summary>
    /// 更新商店内所有商品的状态（主要用于更新价格是否足够购买）。
    /// </summary>
    private void UpdateItem()
    {
        // 遍历并更新所有商店卡牌的价格状态
        foreach (var child in CardsContainer.GetChildren())
        {
            if (child is not ShopCard shopCard)
            {
                continue;
            }
            shopCard.Update(RunStats);
        }

        // 遍历并更新所有商店遗物的价格状态
        foreach (var child in RelicsContainer.GetChildren())
        {
            if (child is not ShopRelic shopRelic)
            {
                continue;
            }
            shopRelic.Update(RunStats);
        }
    }
}

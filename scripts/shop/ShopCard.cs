using System.Threading.Tasks;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.extensions;
using SlayTheSpireLike.scripts.random;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.shop;

/// <summary>
/// 商店中的卡牌展示与购买控件。
/// 负责显示一张可购买的卡牌，并提供购买按钮及价格显示功能。
/// </summary>
public partial class ShopCard : VBoxContainer
{
    private Button _buyButton;
    private CenterContainer _cardContainer;
    private HBoxContainer _price;
    private Label _priceLabel;
    public int GoldCost{get; set;} = RandomNumberProvider.Instance.RandomNumberGenerator.RandiRange(100, 300);
    private Card _card;
    [Export] public CardMenuUi CurrentCardMenuUi { get; set; }

    /// <summary>
    /// 获取或设置当前商店中展示的卡牌对象。
    /// 设置时会延迟调用 SetCard 方法以更新界面。
    /// </summary>
    [Export]
    public Card Card
    {
        get => _card;
        set
        {
            _card = value;
            _ = SetCard();
        }
    }

    /// <summary>
    /// 初始化节点引用并绑定事件处理函数。
    /// 在场景加载完成后自动执行。
    /// </summary>
    public override void _Ready()
    {
        _buyButton = GetNode<Button>("购买");
        _cardContainer = GetNode<CenterContainer>("卡牌容器");
        _price = GetNode<HBoxContainer>("价格容器");
        _priceLabel = GetNode<Label>("价格容器/价格标签");
        _buyButton.Pressed += OnBuyButtonPressed;
    }

    /// <summary>
    /// 根据玩家当前金币数量更新UI状态（如价格颜色、按钮是否可用）。
    /// </summary>
    /// <param name="runStats">包含玩家当前游戏运行数据的对象，用于获取金币数量。</param>
    public void Update(RunStats runStats)
    {
        // 检查必要组件是否存在
        if (_cardContainer.IsInvalidNode() || _price.IsInvalidNode()|| _buyButton.IsInvalidNode())
        {
            return;
        }
        
        _priceLabel.Text = GoldCost.ToString();

        // 判断玩家是否有足够金币购买该卡牌
        if (runStats.Gold > GoldCost)
        {
            _priceLabel.RemoveThemeColorOverride("font_color");
            _buyButton.Disabled = false;
        }
        else
        {
            _priceLabel.AddThemeColorOverride("font_color", Colors.Red);
            _buyButton.Disabled = true;
        }
    }

    /// <summary>
    /// 当购买按钮被按下时触发此方法。
    /// 触发全局事件通知卡牌已被购买，并释放相关子节点资源。
    /// </summary>
    private void OnBuyButtonPressed()
    {
        Events.Instance.RaiseShopCardBought(Card, GoldCost);

        // 释放所有子控件资源
        _cardContainer.QueueFreeX();
        _price.QueueFreeX();
        _buyButton.QueueFreeX();
    }

    /// <summary>
    /// 更新卡牌容器内的卡牌显示内容。
    /// 清除旧的卡牌UI并创建新的卡牌菜单UI实例进行替换。
    /// </summary>
    public async Task SetCard()
    {
        await this.WaitUntilReady();
        // 清理已有子元素
        foreach (var child in _cardContainer.GetChildren())
        {
            child.QueueFreeX();
        }

        // 创建新卡牌UI并添加到容器中
        var cardMenuUi = ResourceFactories.CardMenuUiFactory();
        CurrentCardMenuUi = cardMenuUi;
        cardMenuUi.Card = Card;
        _cardContainer.AddChild(cardMenuUi);
    }
}
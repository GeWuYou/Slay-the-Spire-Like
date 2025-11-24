using SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
/// 负责展示卡牌奖励界面，并允许玩家选择一张卡牌或跳过奖励。
/// </summary>
public partial class CardRewards : ColorRect
{
    private Array<Card> _rewards = [];
    private Card _selectedCard;

    /// <summary>
    /// 当玩家选择了某个卡牌奖励时触发该信号。
    /// </summary>
    /// <param name="card">被选中的卡牌实例；若为null表示跳过了本次奖励。</param>
    [Signal]
    public delegate void CardRewardSelectedEventHandler(Card card);

    /// <summary>
    /// 获取或设置当前显示的卡牌奖励列表。当设置此属性时会自动更新UI。
    /// </summary>
    [Export]
    public Array<Card> Rewards
    {
        private set
        {
            _rewards = value;
            CallDeferred(nameof(SetRewards),value);
        }
        get => _rewards;
    }

    /// <summary>
    /// 用于放置所有可选卡牌的容器节点。
    /// </summary>
    [Export]
    public HBoxContainer Cards { get; set; }

    /// <summary>
    /// 允许玩家跳过卡牌奖励的选择按钮。
    /// </summary>
    [Export]
    public Button SkipCardRewardButton { get; set; }

    /// <summary>
    /// 显示卡牌详细信息的弹出提示框。
    /// </summary>
    [Export]
    public CardTooltipPopup CardTooltip { get; set; }

    /// <summary>
    /// 确认选取所悬停/选定卡牌的按钮。
    /// </summary>
    [Export]
    public Button TakeButton { get; set; }

    /// <summary>
    /// 初始化组件并绑定事件处理逻辑。
    /// 绑定TakeButton点击后发出已选择卡牌的信号并销毁自身。
    /// 绑定SkipCardRewardButton点击后发出空选择信号并销毁自身。
    /// </summary>
    public override void _Ready()
    {
        ClearRewards();
        TakeButton.Pressed += () =>
        {
            EmitSignal(SignalName.CardRewardSelected, _selectedCard);
            QueueFree();
        };
        SkipCardRewardButton.Pressed += () =>
        {
            EmitSignal(SignalName.CardRewardSelected, null);
            QueueFree();
        };
    }

    /// <summary>
    /// 清除当前界面上的所有卡牌及其相关状态（如悬停提示）。
    /// </summary>
    private void ClearRewards()
    {
        foreach (var child in Cards.GetChildren())
        {
            child.QueueFree();
        }
        CardTooltip.HideTooltip();
        _selectedCard = null;
    }

    /// <summary>
    /// 处理输入事件，例如按下取消键来关闭工具提示。
    /// </summary>
    /// <param name="event">传入的游戏输入事件对象。</param>
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            CardTooltip.HideTooltip();
        }
    }
    
    /// <summary>
    /// 展示指定卡牌的信息提示框，并将该卡设为当前选中项。
    /// </summary>
    /// <param name="card">需要显示详情的卡牌对象。</param>
    public void ShowTooltip(Card card)
    {
        _selectedCard = card;
        CardTooltip.ShowTooltip(card);
    }

    /// <summary>
    /// 根据提供的卡牌数组刷新奖励界面的内容。
    /// 每张卡都会创建一个对应的菜单UI元素并加入到Cards容器中，
    /// 同时注册其请求显示提示的事件回调。
    /// </summary>
    /// <param name="rewards">要展示在界面上的一组卡牌。</param>
    public void SetRewards(Array<Card> rewards)
    {
        ClearRewards();
        foreach (var card in rewards)
        {
           var cardMenuUi =  ResourceFactories.CardMenuUiFactory();
           cardMenuUi.Card = card;
           Cards.AddChild(cardMenuUi);
           cardMenuUi.CardVisuals.TooltipRequested += ShowTooltip;
        }
    }
}

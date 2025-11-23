using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
/// 卡牌可视化控件类，负责显示卡牌的视觉元素并处理用户交互
/// </summary>
public partial class CardVisuals : Control
{
    private Card _card;
    /// <summary>
    /// 获取或设置当前显示的卡牌数据
    /// </summary>
    /// <value>卡牌对象</value>
    [Export]
    public Card Card
    {
        get => _card;
        set
        {
            _card = value;  
            CallDeferred(nameof(SetCard), value);
        }
    }
    /// <summary>
    /// 获取或设置卡牌背景面板控件
    /// </summary>
    [Export] public Panel Panel { get; set; }
    /// <summary>
    /// 获取或设置显示卡牌费用的标签控件
    /// </summary>
    [Export] public Label Cost { get; set; }
    /// <summary>
    /// 获取或设置显示卡牌图标的纹理控件
    /// </summary>
    [Export] public TextureRect Icon { get; set; }
    /// <summary>
    /// 获取或设置卡牌的稀有度图标控件
    /// </summary>
    [Export] public TextureRect Rarity { get; set; }
    /// <summary>
    /// 工具提示请求事件委托
    /// </summary>
    /// <param name="card">需要显示工具提示的卡牌</param>
    [Signal]
    public delegate void TooltipRequestedEventHandler(Card card);
    
    /// <summary>
    /// 设置卡牌的视觉显示内容
    /// </summary>
    /// <param name="card">要显示的卡牌数据</param>
    private void SetCard(Card card)
    {
        Cost.Text = card.Cost.ToString();
        Icon.Texture = card.Icon as Texture2D;
        Rarity.Modulate = Card.RarityColors[card.CardRarity];
    }
    
    /// <summary>
    /// 控件初始化完成时调用，注册鼠标和输入事件处理函数
    /// </summary>
    public override void _Ready()
    {
        MouseEntered+=OnMouseEntered;
        MouseExited+=OnMouseExited;
        GuiInput+=OnGuiInput;
    }
    
    /// <summary>
    /// 鼠标离开控件区域时的处理函数，恢复默认样式
    /// </summary>
    private void OnMouseExited()
    {
        Panel.AddThemeStyleboxOverride("panel",ResourceFactories.CardBaseStyleBoxFactory());
    }

    /// <summary>
    /// 鼠标进入控件区域时的处理函数，应用悬停样式
    /// </summary>
    private void OnMouseEntered()
    {
        Panel.AddThemeStyleboxOverride("panel",ResourceFactories.CardHoverStyleBoxFactory());
    }

    /// <summary>
    /// 处理控件的GUI输入事件，当检测到鼠标左键点击时触发工具提示请求信号
    /// </summary>
    /// <param name="event">输入事件对象</param>
    private void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            EmitSignal(SignalName.TooltipRequested,Card);
        }
    }
}

using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
/// 卡牌提示弹窗控件，用于显示卡牌的详细信息和描述
/// </summary>
public partial class CardTooltipPopup : Control
{
    [Export] public CenterContainer TooltipCard { get; set; }
    [Export] public RichTextLabel CardDescription { get; set; }
    
    [Export] public Card Card { get; set; }

    /// <summary>
    /// 控件初始化完成时调用的方法
    /// 清理现有的子节点并绑定输入事件处理
    /// </summary>
    public override void _Ready()
    {
        // 清理TooltipCard容器中的所有子节点
        foreach (var child in TooltipCard.GetChildren())
        {
            child.QueueFree();
        }
        GuiInput += OnGuiInput;
    }

    /// <summary>
    /// 处理GUI输入事件的方法
    /// 当检测到鼠标左键按下时隐藏提示框
    /// </summary>
    /// <param name="event">输入事件对象</param>
    private void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            HideTooltip();
        }
    }

    /// <summary>
    /// 显示指定卡牌的提示信息
    /// </summary>
    /// <param name="card">要显示提示信息的卡牌对象</param>
    public void ShowTooltip(Card card)
    {
        // 创建新的卡牌UI实例并添加到容器中
        var newCard =  ResourceFactories.CardMenuUiFactory();
        TooltipCard.AddChild(newCard);
        newCard.Card = card;
        newCard.TooltipRequested+= _ => HideTooltip();
        CardDescription.Text = card.Description;
        Show();
    }

    /// <summary>
    /// 隐藏提示框并清理相关资源
    /// </summary>
    public void HideTooltip()
    {
        if (!Visible)
        {
            return;
        }

        // 清理TooltipCard容器中的所有子节点
        foreach (var child in TooltipCard.GetChildren())
        {
            child.QueueFree();
        }
        Hide();
    }
}

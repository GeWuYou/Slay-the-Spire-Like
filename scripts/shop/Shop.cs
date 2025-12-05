using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.shop;

public partial class Shop : Control
{
    [Export]
    public TextureButton GoBackButton { get; set; }
    [Export]
    public HBoxContainer CardsContainer { get; set; }
    [Export]
    public HBoxContainer RelicsContainer { get; set; }
    [Export]
    public CardTooltipPopup CardTooltipPopup { get; set; }

    public override void _Ready()
    {
        GoBackButton.Pressed += () => Events.Instance.RaiseShopExited();
        foreach (var child in CardsContainer.GetChildren())
        {
            if (child is not ShopCard shopCard)
            {
               continue;
            }

            shopCard.CurrentCardMenuUi.CardVisuals.Connect(CardVisuals.SignalName.TooltipRequested,
                new Callable(CardTooltipPopup,CardTooltipPopup.MethodName.ShowTooltip));
        }
        GuiInput+=OnGuiInput;
    }

    private void OnGuiInput(InputEvent @event)
    {
        if(@event.IsActionPressed("left_mouse"))
        {
            CardTooltipPopup.HideTooltip();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("ui_cancel")&& CardTooltipPopup.Visible)
        {
           CardTooltipPopup.HideTooltip();
        }
    }
    
}
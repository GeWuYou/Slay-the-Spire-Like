using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class CardTooltipPopup : Control
{
    [Export] public CenterContainer TooltipCard { get; set; }
    [Export] public RichTextLabel CardDescription { get; set; }
    
    [Export] public Card Card { get; set; }

    public override void _Ready()
    {
        foreach (var child in TooltipCard.GetChildren())
        {
            child.QueueFree();
        }
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            HideTooltip();
        }
    }

    public void ShowTooltip(Card card)
    {
        var newCard =  ResourceFactories.CardMenuUiFactory();
        TooltipCard.AddChild(newCard);
        newCard.Card = card;
        newCard.TooltipRequested+= _ => HideTooltip();
        CardDescription.Text = card.Description;
        Show();
    }

    public void HideTooltip()
    {
        if (!Visible)
        {
            return;
        }

        foreach (var child in TooltipCard.GetChildren())
        {
            child.QueueFree();
        }
        Hide();
    }
}
using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class CardPileView : Control
{
    public CardPile CardPile { get; set; }
    [Export]
    public Label Title { get; set; }
    [Export]
    public GridContainer Cards { get; set; }
    [Export]
    public CardTooltipPopup CardTooltip { get; set; }
    [Export]
    public Button BackButton { get; set; }

    public override void _Ready()
    {
        BackButton.Pressed += Hide;
        foreach (var child in Cards.GetChildren())
        {
            child.QueueFree();
        }
        CardTooltip.HideTooltip();
    }

    public override void _Input(InputEvent @event)
    {
        if (!@event.IsActionPressed("ui_cancel"))
        {
            return;
        }
        if (CardTooltip.Visible)
        {
            CardTooltip.HideTooltip();
        }else
        {
            Hide();
        }
    }

    public void ShowCurrentView(string title, bool randomize = false)
    {
        foreach (var child in Cards.GetChildren())
        {
            child.QueueFree();
        }
        CardTooltip.HideTooltip();
        Title.Text = title;
        CallDeferred(nameof(UpdateView),randomize);
    }
    public void UpdateView(bool randomize = false)
    {
        if (CardPile is null)
        {
            return;
        }

        var allCards = CardPile.Cards.Duplicate();
        if (randomize)
        {
            allCards.Shuffle();
        }
        foreach (var card in allCards)
        {
            var newCard = ResourceFactories.CardMenuUiFactory();
            newCard.Card = card;
            Cards.AddChild(newCard);
            newCard.TooltipRequested += CardTooltip.ShowTooltip;
        }
        Show();
    }
}
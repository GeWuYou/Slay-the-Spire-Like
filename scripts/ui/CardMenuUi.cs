using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class CardMenuUi : Control
{
    private Card _card;

    [Export]
    public CardVisuals CardVisuals { get; set; }

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
    
    private void SetCard(Card card)
    {
        CardVisuals.Card = card;
    }
}
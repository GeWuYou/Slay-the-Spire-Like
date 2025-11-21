using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class CardMenuUi : Control
{
    private Card _card;

    [Export]
    public Control Visuals { get; set; }

    [Export]
    public Card Card
    {
        get => _card;
        set => CallDeferred(nameof(SetCard),value);
    }

    [Export] public Panel Panel { get; set; }
    [Export] public Label CostLabel { get; set; }
    [Export] public TextureRect Icon { get; set; }
    [Signal]
    public delegate void TooltipRequestedEventHandler(Card card);
    
    public override void _Ready()
    {
        Visuals.MouseEntered+=OnVisualsMouseEntered;
        Visuals.MouseExited+=OnVisualsMouseExited;
        Visuals.GuiInput+=OnVisualsGuiInput;
    }
    private void SetCard(Card card)
    {
        _card = card;
        CostLabel.Text = card.Cost.ToString();
        Icon.Texture = card.Icon as Texture2D;
    }

    private void OnVisualsMouseExited()
    {
        Panel.AddThemeStyleboxOverride("panel",ResourceFactories.CardBaseStyleBoxFactory());
    }

    private void OnVisualsMouseEntered()
    {
        Panel.AddThemeStyleboxOverride("panel",ResourceFactories.CardHoverStyleBoxFactory());
    }

    private void OnVisualsGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            EmitSignal(SignalName.TooltipRequested,Card);
        }
    }
}
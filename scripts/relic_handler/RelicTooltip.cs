using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.relic_handler;

public partial class RelicTooltip : Control
{
    private TextureRect _icon;
    private Label _name;
    private RichTextLabel _description;
    private TextureButton _closeButton;
    
    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("VBoxContainer/RelicIcon");
        _name = GetNode<Label>("VBoxContainer/RelicLabel");
        _description = GetNode<RichTextLabel>("VBoxContainer/RelicDescription");
        _closeButton = GetNode<TextureButton>("CloseButton");
        _closeButton.Pressed+=Hide;
        GuiInput += OnGuiInput;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("ui_cancel"))
        {
            Hide();
        }
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            Hide();
        }
    }
    
    public void ShowTooltip(Relic relic)
    {
        _icon.Texture = relic.Icon as Texture2D;
        _name.Text = relic.RelicName;
        _description.Text = relic.GetTooltip();
        Show();
    }
}
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.relic_handler;

public partial class RelicUi : Control
{
    [Export]
    public Relic Relic
    {
        get => _relic;
        set
        {
            _relic = value;
            CallDeferred(nameof(SetRelic));
        }
    }

    private TextureRect _icon;
    private AnimationPlayer _animationPlayer;
    private Relic _relic;

    public override void _Ready()
    {
        GuiInput+=OnGuiInput;
        _icon = GetNode<TextureRect>("Icon");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void Flash()
    {
        _animationPlayer.Play("flash");
    }
    private void SetRelic()
    {
        _icon.Texture = (Texture2D)Relic.Icon;
    }

    private void OnGuiInput(InputEvent @event)
    {
        
    }
    
}
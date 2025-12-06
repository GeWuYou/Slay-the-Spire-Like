using System.Threading.Tasks;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.extensions;
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
            _ = SetRelic();
        }
    }

    private TextureRect _icon;
    private AnimationPlayer _animationPlayer;
    private Relic _relic;

    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("Icon");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        GuiInput += OnGuiInput;
    }

    public void Flash()
    {
        _animationPlayer.Play("flash");
    }

    private async Task SetRelic()
    {
        await this.WaitUntilReady();
        _icon.Texture = (Texture2D)Relic.Icon;
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse"))
        {
            Events.Instance.RaiseRelicTooltipRequested(Relic);
        }
    }
}
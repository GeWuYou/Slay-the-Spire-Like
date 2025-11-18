using global::SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.ui;

public partial class ToolTip : PanelContainer
{
    private Events _events;

    private bool _isVisible;

    private Tween _tween;
    [Export] public float FadeSeconds { get; private set; } = 0.2f;

    [Export] public TextureRect Icon { get; private set; }
    [Export] public RichTextLabel Text { get; private set; }

    public override void _Ready()
    {
        _events = Events.Instance;
        _events.CardToolTipShowRequest += ShowToolTip;
        _events.CardToolTipHideRequest += HideToolTip;
        Modulate = Colors.Transparent;
        Hide();
    }

    private void ShowToolTip(Texture icon, string text)
    {
        _isVisible = true;
        _tween?.Kill();
        ShowAnimation(icon, text);
    }

    private void ShowAnimation(Texture icon, string text)
    {
        Text.Text = text;
        Icon.Texture = (Texture2D)icon;

        _tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenCallback(Callable.From(Show));
        _tween.TweenProperty(this, "modulate", Colors.White, FadeSeconds);
    }

    private void HideToolTip()
    {
        _isVisible = false;
        _tween?.Kill();
        GetTree().CreateTimer(FadeSeconds, false).Timeout += HideAnimation;
    }

    private void HideAnimation()
    {
        if (_isVisible) return;
        _tween = CreateTween().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenProperty(this, "modulate", Colors.Transparent, FadeSeconds);
        _tween.TweenCallback(Callable.From(Hide));
    }
}
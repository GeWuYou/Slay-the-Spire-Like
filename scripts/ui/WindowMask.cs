using Godot;
using SlayTheSpireLike.scripts.global;

namespace SlayTheSpireLike.scripts.ui;

public partial class WindowMask : CanvasLayer
{
    [Export]
    public ColorRect ColorRect { get; set; }
    [Export]
    public Timer Timer { get; set; }

    public override void _Ready()
    {
        Events.Instance.PlayerHit+=OnPlayerHit;
        Timer.Timeout+=OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        ColorRect.Color = ColorRect.Color with { A = 0 };
    }

    private void OnPlayerHit()
    {
        ColorRect.Color = ColorRect.Color with { A = 0.4f };
        Timer.Start();
    }
}
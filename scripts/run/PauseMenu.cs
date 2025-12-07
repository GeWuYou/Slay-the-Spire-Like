using SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.run;
public partial class PauseMenu : CanvasLayer
{
    [Signal]
    public delegate void SaveAndQuiteEventHandler();
    [Export]
    public Button BackToGameButton { get; set; }
    [Export]
    public Button SaveAndQuiteButton { get; set; }

    public override void _Ready()
    {
        BackToGameButton.Pressed+=UnPause;
        SaveAndQuiteButton.Pressed+=OnSaveAndQuiteButtonPressed;
    }

    private void OnSaveAndQuiteButtonPressed()
    {
        GetTree().Paused = false;
        AudioPlayerManager.Instance.StopMusic();
        EmitSignal(SignalName.SaveAndQuite);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            if (Visible)
            {
                UnPause();
            }else
            {
                Pause();
            }
            GetViewport().SetInputAsHandled();
        }
    }

    private void UnPause()
    {
        Hide();
        GetTree().Paused = false;
    }

    private void Pause()
    {
        Show();
        GetTree().Paused = true;
    }
}
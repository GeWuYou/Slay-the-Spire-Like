using Godot;
namespace SlayTheSpireLike.scripts.ui;
public partial class MainMenu : Control
{
    [Export]
    public Button ContinueButton { get; set; }
    [Export]
    public Button NewGameButton { get; set; }
    [Export]
    public Button QuitButton { get; set; }

    public override void _Ready()
    {
        GetTree().Paused = false;
        ContinueButton.Pressed+=OnContinueButtonPressed;
        NewGameButton.Pressed+=OnNewGameButtonPressed;
        QuitButton.Pressed+=()=>GetTree().Quit();
    }
    
    private void OnNewGameButtonPressed()
    {
        throw new System.NotImplementedException();
    }

    private void OnContinueButtonPressed()
    {
        throw new System.NotImplementedException();
    }
}

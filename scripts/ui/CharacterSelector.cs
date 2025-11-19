using Godot;

namespace SlayTheSpireLike.scripts.ui;

public partial class CharacterSelector : Control
{
    [Export]
    public Button StartButton { get; set; }
    [Export]
    public Button WarriorButton { get; set; }
    [Export]
    public Button WizardButton { get; set; }
    [Export]
    public Button AssassinButton { get; set; }

    public override void _Ready()
    {
        StartButton.Pressed+=OnStartButtonPressed;
        WarriorButton.Pressed+=OnWarriorButtonPressed;
        WizardButton.Pressed+=OnWizardButtonPressed;
        AssassinButton.Pressed+=OnAssassinButtonPressed;
    }

    private void OnAssassinButtonPressed()
    {
        
        
    }

    private void OnWizardButtonPressed()
    {
        
        
    }

    private void OnWarriorButtonPressed()
    {
        
        
    }

    private void OnStartButtonPressed()
    {
        
    }
}
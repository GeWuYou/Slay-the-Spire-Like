using System.Threading.Tasks;
using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.campfire;

public partial class Campfire : Control
{
    [Export]
    public Button RestButton { get; set; }
    [Export]
    public CharacterStats PlayerStats { get; set; }
    
    [Export]
    public CanvasLayer UiLayer { get; set; }

    [Export]
    public CanvasLayer ParticlesLayer { get; set; }
    public override void _Ready()
    {
        RestButton.Pressed +=async () =>await OnRestButtonPressed();
    }

    private async Task OnRestButtonPressed()
    {
        PlayerStats.Heal(Mathf.CeilToInt(PlayerStats.MaxHeath*0.3));
        await SceneTransitionManager.Instance.PerformFadeEffect(OnFadeOutComplete);
    }
    
    public void OnFadeOutComplete()
    {
        UiLayer.Hide();
        ParticlesLayer.Hide();
        Events.Instance.RaiseCampfireExited();
    }
}
using System.Threading.Tasks;
using System.Transactions;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.extensions;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.win;

public partial class WinScreen : Control
{
    [Export] public Button MainMenuButton { get; set; }

    [Export] public Label MessageLabel { get; set; }

    [Export] public TextureRect CharacterPortrait { get; set; }

    public CharacterStats PlayerStats
    {
        get => _playerStats;
        set
        {
            _playerStats = value;
            _ = SetCharterStats();
        }
    }

    private async Task SetCharterStats()
    {
        await this.WaitUntilReady();
        MessageLabel.Text = string.Format(_messageTemplate, PlayerStats.Name);
        CharacterPortrait.Texture = PlayerStats.Portrait as Texture2D;
    }

    private string _messageTemplate = "恭喜你作为{0}\n拥抱战斗的的荣耀！\n获得了胜利。";
    private CharacterStats _playerStats;

    public override void _Ready()
    {
        MainMenuButton.Pressed += async () =>await OnMainMenuButtonPressed();
    }

    private static async Task OnMainMenuButtonPressed()
    {
        await SceneTransitionManager.Instance.TransitionToScene(ResourceLoaderManager.Instance
            .GetSceneLoader(GameConstants.ResourcePaths.MainMenuScene).Value);
    }
}
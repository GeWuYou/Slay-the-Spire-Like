using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class CharacterSelector : Control
{
    [Export] public Button StartButton { get; set; }
    [Export] public Button WarriorButton { get; set; }
    [Export] public Button WizardButton { get; set; }
    [Export] public Button AssassinButton { get; set; }

    [Export] [ExportGroup("标题")] public Label Title { get; set; }
    [Export] [ExportGroup("描述")] public Label Description { get; set; }
    [Export] [ExportGroup("角色图片")] public TextureRect CharacterPortrait { get; set; }
    private CharacterStats _characterStats;

    public override void _Ready()
    {
        _characterStats = ResourceFactories.WarriorStatsFactory();
        StartButton.Pressed += OnStartButtonPressed;
        WarriorButton.Pressed += OnWarriorButtonPressed;
        WizardButton.Pressed += OnWizardButtonPressed;
        AssassinButton.Pressed += OnAssassinButtonPressed;
    }

    /// <summary>
    /// 更新角色选择界面显示
    /// </summary>
    /// <param name="characterStats">选中的角色属性</param>
    private void UpdateCharacterDisplay(CharacterStats characterStats)
    {
        _characterStats = characterStats;
        CharacterPortrait.Texture = characterStats.Portrait as Texture2D;
        Title.Text = characterStats.Name;
        Description.Text = characterStats.Description;
    }

    private void OnAssassinButtonPressed()
    {
        UpdateCharacterDisplay(ResourceFactories.AssassinStatsFactory());
    }

    private void OnWizardButtonPressed()
    {
        UpdateCharacterDisplay(ResourceFactories.WizardStatsFactory());
    }

    private void OnWarriorButtonPressed()
    {
        UpdateCharacterDisplay(ResourceFactories.WarriorStatsFactory());
    }


    private void OnStartButtonPressed()
    {
        
    }
}
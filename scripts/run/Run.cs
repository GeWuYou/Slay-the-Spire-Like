using System;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.run;

public partial class Run : Node
{
    [Export]
    public Node CurrentView { get; set; }
    [Export]
    public Button BattleButton { get; set; }
    [Export]
    public Button ShopButton { get; set; }
    [Export]
    public Button CampfireButton { get; set; }
    [Export]
    public Button TreasureButton { get; set; }
    [Export]
    public Button MapButton { get; set; }
    [Export]
    public Button RewardsButton { get; set; }
    public CharacterStats PlayerStats { get; set; }
    private Events.BattleWonEventHandler _setBattleRewardScene;
    public override void _Ready()
    {
        StartRun();
    }

    private void StartRun()
    {
        SetupEventConnections();
    }

    private void SetupEventConnections()
    {
        var events = Events.Instance;
        _setBattleRewardScene = ()
            => ChangeView(ResourceLoaderManager
                .Instance.GetSceneLoader(GameConstants.ResourcePaths.BattleRewardScene).Value);
        events.BattleWon += _setBattleRewardScene;

    }
    
    private void ChangeView(PackedScene newScene)
    {
        if (CurrentView.GetChildCount() > 0)
        {
            CurrentView.GetChild(0).QueueFree();
        }

        GetTree().Paused = false;
       var newView =  newScene.Instantiate();
       CurrentView.AddChild(newView);
    }
}
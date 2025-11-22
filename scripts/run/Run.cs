using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.run;

public partial class Run : Node
{
    [Export] public Node CurrentView { get; set; }
    [Export] public Button BattleButton { get; set; }
    [Export] public Button ShopButton { get; set; }
    [Export] public Button CampfireButton { get; set; }
    [Export] public Button TreasureButton { get; set; }
    [Export] public Button MapButton { get; set; }
    [Export] public Button RewardsButton { get; set; }
    [Export] public RunStartup RunStartup { get; set; }
    [Export] public CardPileView DeckPileView { get; set; }
    [Export] public CardPileOpener DeckButton { get; set; }
    public CharacterStats PlayerStats { get; set; }
    private readonly List<Action> _disposables = new();

    public override void _Ready()
    {
       
        if (RunStartup.RunType == RunStartup.Type.NewRun)
        {
            PlayerStats = RunStartup.PlayerStats.CreateInstance();
            StartRun();
        }else
        {
            GD.Print("todo 还未实现");
        }
    }

    private void StartRun()
    {
        SetupEventConnections();
        SetupTopBar();
    }

    private void SetupTopBar()
    {
        DeckButton.CardPile = PlayerStats.Deck;
        DeckPileView.CardPile = PlayerStats.Deck;
        DeckButton.Pressed+=() => DeckPileView.ShowCurrentView("抽牌堆");
    }

    /// <summary>
    /// 设置事件连接，将游戏事件与对应的场景切换逻辑进行绑定
    /// </summary>
    /// <remarks>
    /// 该方法负责建立游戏核心流程中的事件监听机制，包括战斗胜利、奖励界面退出、
    /// 营火界面退出、商店界面退出和宝箱房间退出等事件的处理。
    /// 所有事件订阅都会被添加到_disposables集合中，用于后续的资源清理和事件解绑。
    /// </remarks>
    private void SetupEventConnections()
    {
        var events = Events.Instance;
        var resourceLoaderManager = ResourceLoaderManager.Instance;

        // 战斗胜利事件处理：切换到战斗奖励场景
        var battleRewardSceneFun = () =>
            ChangeView(resourceLoaderManager
                .GetSceneLoader(GameConstants.ResourcePaths.BattleRewardScene).Value);
        events.BattleWon += battleRewardSceneFun;
        _disposables.Add(() => events.BattleWon -= battleRewardSceneFun);

        // 多个界面退出事件的统一处理：返回地图场景
        var mapSceneFun = () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.MapScene).Value);
        events.BattleRewardExited += mapSceneFun;
        _disposables.Add(() => events.BattleRewardExited -= mapSceneFun);
        events.CampfireExited += mapSceneFun;
        _disposables.Add(() => events.CampfireExited -= mapSceneFun);
        events.ShopExited += mapSceneFun;
        _disposables.Add(() => events.ShopExited -= mapSceneFun);
        events.TreasureRoomExited += mapSceneFun;
        _disposables.Add(() => events.TreasureRoomExited -= mapSceneFun);
        events.MapExited += OnMapExited;
        _disposables.Add(() => events.MapExited -= OnMapExited);

        BattleButton.Pressed += () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.BattleScene).Value);
        CampfireButton.Pressed += () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.CampfireScene).Value);
        MapButton.Pressed += mapSceneFun;
        RewardsButton.Pressed += battleRewardSceneFun;
        TreasureButton.Pressed += () =>
        {
            ChangeView(resourceLoaderManager
                .GetSceneLoader(GameConstants.ResourcePaths.TreasureScene).Value);
            GD.Print("TreasureButton pressed");
        };
        ShopButton.Pressed += () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.ShopScene).Value);
    }

    private void OnMapExited()
    {
    }

    /// <summary>
    /// 断开事件连接的方法
    /// </summary>
    /// <remarks>
    /// 该方法遍历所有可释放的资源，并依次调用它们的Dispose方法来清理资源
    /// </remarks>
    private void DisconnectEvent()
    {
        // 遍历所有可释放资源并执行释放操作
        foreach (var disposable in _disposables)
        {
            disposable?.Invoke();
        }
    }

    private void ChangeView(PackedScene newScene)
    {
        if (CurrentView.GetChildCount() > 0)
        {
            CurrentView.GetChild(0).QueueFree();
        }

        GetTree().Paused = false;
        var newView = newScene.Instantiate();
        CurrentView.AddChild(newView);
    }

    public override void _ExitTree()
    {
        DisconnectEvent();
    }
}
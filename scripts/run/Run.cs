using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.enemies.battle;
using SlayTheSpireLike.scripts.map;
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
    [Export] public GoldUi GoldUi { get; set; }
    [Export] public Map Map { get; set; }
    public CharacterStats PlayerStats { get; set; }
    public RunStats RunStats { get; set; }
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
        // todo 暂时测试用
        RunStats = new RunStats();
        SetupEventConnections();
        SetupTopBar();
        Map.GenerateNewMap();
        Map.UnlockFloor(0);
    }

    private void SetupTopBar()
    {
        GoldUi.RunStats = RunStats;
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
       
        events.BattleWon += OnBattleWon;
        _disposables.Add(() => events.BattleWon -= OnBattleWon);
        events.BattleRewardExited += ShowMap;
        _disposables.Add(() => events.BattleRewardExited -= ShowMap);
        events.CampfireExited += ShowMap;
        _disposables.Add(() => events.CampfireExited -= ShowMap);
        events.ShopExited += ShowMap;
        _disposables.Add(() => events.ShopExited -= ShowMap);
        events.TreasureRoomExited += ShowMap;
        _disposables.Add(() => events.TreasureRoomExited -= ShowMap);
        events.MapExited += OnMapExited;
        _disposables.Add(() => events.MapExited -= OnMapExited);

        BattleButton.Pressed += () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.BattleScene).Value);
        CampfireButton.Pressed += () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.CampfireScene).Value);
        MapButton.Pressed += ShowMap;
        RewardsButton.Pressed += OnBattleWon;
        TreasureButton.Pressed += () =>
        {
            ChangeView(resourceLoaderManager
                .GetSceneLoader(GameConstants.ResourcePaths.TreasureScene).Value);
            GD.Print("TreasureButton pressed");
        };
        ShopButton.Pressed += () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.ShopScene).Value);
    }

    private void OnBattleWon()
    {
        var rewardScene =  ChangeView(ResourceLoaderManager.Instance
            .GetSceneLoader(GameConstants.ResourcePaths.BattleRewardScene).Value) as BattleReward;
        rewardScene!.RunStats = RunStats;
        rewardScene!.PlayerStats = PlayerStats;
        
        // todo 暂时添加
        rewardScene.AddGoldReward(77);
        rewardScene.AddCardReward();
    }
    private void OnMapExited(Room room)
    {
        switch (room.RoomType)
        {
            case Room.Type.Unknown:
                ChangeView(ResourceLoaderManager.Instance
                    .GetSceneLoader(GameConstants.ResourcePaths.BattleRewardScene).Value);
                break;
            case Room.Type.Monster:
                ChangeView(ResourceLoaderManager.Instance
                    .GetSceneLoader(GameConstants.ResourcePaths.BattleScene).Value);
                break;
            case Room.Type.Treasure:
                ChangeView(ResourceLoaderManager.Instance
                    .GetSceneLoader(GameConstants.ResourcePaths.TreasureScene).Value);
                break;
            case Room.Type.Campfire:
                ChangeView(ResourceLoaderManager.Instance
                    .GetSceneLoader(GameConstants.ResourcePaths.CampfireScene).Value);
                break;
            case Room.Type.Shop:
                ChangeView(ResourceLoaderManager.Instance
                    .GetSceneLoader(GameConstants.ResourcePaths.ShopScene).Value);
                break;
            case Room.Type.Boss:
                // ChangeView(ResourceLoaderManager.Instance
                //     .GetSceneLoader(GameConstants.ResourcePaths.BossScene).Value);
                ChangeView(ResourceLoaderManager.Instance
                    .GetSceneLoader(GameConstants.ResourcePaths.BattleScene).Value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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

    private void ShowMap()
    {
        if (CurrentView.GetChildCount() > 0)
        {
            CurrentView.GetChild(0).QueueFree();
        }
        Map.ShowMap();
        Map.UnlockNextRooms();
    }
    private Node ChangeView(PackedScene newScene)
    {
        if (CurrentView.GetChildCount() > 0)
        {
            CurrentView.GetChild(0).QueueFree();
        }

        GetTree().Paused = false;
        var newView = newScene.Instantiate();
        CurrentView.AddChild(newView);
        Map.HideMap();
        return newView;
    }
    private void ChangeViewNotReturn(PackedScene newScene)
    {
        ChangeView(newScene);
    }

    public override void _ExitTree()
    {
        DisconnectEvent();
    }
}
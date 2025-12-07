using System;
using System.Collections.Generic;
using global::SlayTheSpireLike.global;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.battle;
using SlayTheSpireLike.scripts.campfire;
using SlayTheSpireLike.scripts.core.save;
using SlayTheSpireLike.scripts.map;
using SlayTheSpireLike.scripts.random;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.room.treasure;
using SlayTheSpireLike.scripts.save;
using SlayTheSpireLike.scripts.shop;
using SlayTheSpireLike.scripts.ui;
using SlayTheSpireLike.scripts.ui.components;
using SlayTheSpireLike.scripts.win;
using BattleReward = SlayTheSpireLike.scripts.battle.BattleReward;

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
    [Export] public StatItem HealthUi { get; set; }
    [Export] public Map Map { get; set; }
    [Export] public RelicHandler RelicHandler { get; set; }
    [Export] public RelicTooltip RelicTooltip { get; set; }
    [Export] public PauseMeun PauseMeun { get; set; }
    public CharacterStats PlayerStats { get; set; }
    public RunStats RunStats { get; set; }
    private SaveData _saveData;
    private readonly List<Action> _disposables = new();

    private static async void OnSaveAndQuit()
    {
        try
        {
            await SceneTransitionManager.Instance.TransitionToScene(ResourceLoaderManager
                .Instance.GetSceneLoader(GameConstants.ResourcePaths.MainMenuScene).Value);
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
    }

    private void SaveRun(bool wasOnMap)
    {
        _saveData ??= new SaveData();

        // 使用 Duplicate() 创建独立副本（快照），避免保存的是可变引用
        _saveData.RunStats = RunStats.Duplicate() as RunStats;
        _saveData.PlayerStats = PlayerStats.Duplicate() as CharacterStats;

        // 如果 Relic、MapData 也是 Resource，继续用 Duplicate 或深拷贝
        _saveData.Relics = RelicHandler.GetAllRelics();
        _saveData.LastRoom = Map.LastRoom;
        _saveData.MapData = Map.MapData.Duplicate();
        _saveData.FloorsClimbed = Map.FloorsClimbed;
        _saveData.RngSeed = RandomNumberProvider.Instance.RandomNumberGenerator.Seed;
        _saveData.RngState = RandomNumberProvider.Instance.RandomNumberGenerator.State;
        _saveData.WasOnMap = wasOnMap;

        GameManager.SaveManager.Save(_saveData);
    }


    public override void _Ready()

    {
        PauseMeun.Connect(PauseMeun.SignalName.SaveAndQuite,
            Callable.From(OnSaveAndQuit));
        if (RunStartup.RunType == RunStartup.Type.NewRun)
        {
            PlayerStats = RunStartup.PlayerStats.CreateInstance();
            StartRun();
        }
        else
        {
            LoadRun();
        }
        HealthUi.FormatString = $"{{0}}/{PlayerStats.MaxHeath}";
        PlayerStats.Connect(Stats.SignalName.StatsChanged,
            Callable.From(() => HealthUi.UpdateValue(PlayerStats.Health)));
        HealthUi.UpdateValue(PlayerStats.Health);
    }
    private void LoadRun()
    {
        _saveData = GameManager.SaveManager.Load<SaveData>();
        RunStats = _saveData.RunStats;
        PlayerStats = _saveData.PlayerStats;
        PlayerStats.Deck = _saveData.PlayerStats.Deck;
        PlayerStats.Health = _saveData.PlayerStats.Health;
        RandomNumberProvider.Instance.SetRandomNumberGeneratorBySeedAndState(_saveData.RngSeed, _saveData.RngState);
        RelicHandler.AddRelics(_saveData.Relics);
        SetupTopBar();
        SetupEventConnections();
        Map.LoadMap(_saveData.MapData, _saveData.FloorsClimbed, _saveData.LastRoom);
        if (_saveData.LastRoom is not null && !_saveData.WasOnMap)
        {
            OnMapExited(_saveData.LastRoom);
        }
    }
    private void StartRun()
    {
        RunStats = new RunStats();
        SetupEventConnections();
        SetupTopBar();
        Map.GenerateNewMap();
        Map.UnlockFloor(0);
        SaveRun(true);
    }

    private void SetupTopBar()
    {
        GoldUi.RunStats = RunStats;

        RelicHandler.AddRelic(PlayerStats.StartingRelic);
        Events.Instance.RelicTooltipRequested += RelicTooltip.ShowTooltip;

        _disposables.Add(() => Events.Instance.RelicTooltipRequested -= RelicTooltip.ShowTooltip);
        DeckButton.CardPile = PlayerStats.Deck;
        DeckPileView.CardPile = PlayerStats.Deck;
        DeckButton.Pressed += () => DeckPileView.ShowCurrentView("抽牌堆");
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

        events.BattleWon += OnBattleWon;
        _disposables.Add(() => events.BattleWon -= OnBattleWon);

        events.BattleRewardExited += ShowMap;
        _disposables.Add(() => events.BattleRewardExited -= ShowMap);
        events.CampfireExited += ShowMap;
        _disposables.Add(() => events.CampfireExited -= ShowMap);
        events.ShopExited += ShowMap;
        _disposables.Add(() => events.ShopExited -= ShowMap);
        events.TreasureRoomExited += OnTreasureRoomExited;
        _disposables.Add(() => events.TreasureRoomExited -= OnTreasureRoomExited);
        events.MapExited += OnMapExited;
        _disposables.Add(() => events.MapExited -= OnMapExited);

        BattleButton.Pressed += () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.BattleScene).Value);
        CampfireButton.Pressed += () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.CampfireScene).Value);
        MapButton.Pressed += ShowMap;
        RewardsButton.Pressed += OnBattleWon;
        TreasureButton.Pressed += OnTreasureRoomEntered;
        ShopButton.Pressed += () => ChangeView(resourceLoaderManager
            .GetSceneLoader(GameConstants.ResourcePaths.ShopScene).Value);
    }

    private void OnTreasureRoomExited(Relic relic)
    {
        var rewardScene = ChangeView(ResourceLoaderManager.Instance
            .GetSceneLoader(GameConstants.ResourcePaths.BattleRewardScene).Value) as BattleReward;
        rewardScene!.RunStats = RunStats;
        rewardScene!.PlayerStats = PlayerStats;
        rewardScene.RelicHandler = RelicHandler;
        rewardScene.AddRelicReward(relic);
    }

    private void ShowRegularBattleRewards()
    {
        var rewardScene = ChangeView(ResourceLoaderManager.Instance
            .GetSceneLoader(GameConstants.ResourcePaths.BattleRewardScene).Value) as BattleReward;
        rewardScene!.RunStats = RunStats;
        rewardScene!.PlayerStats = PlayerStats;

        rewardScene.AddGoldReward(Map.LastRoom.BattleStats.RollGoldReward());
        rewardScene.AddCardReward();
    }

    private void OnBattleWon()
    {
        GD.Print($"{Map.FloorsClimbed}:{Map.MapGenerator.Floors}");
        if (Map.FloorsClimbed == Map.MapGenerator.Floors)
        {
            if (ChangeView(ResourceLoaderManager.Instance.GetSceneLoader(GameConstants.ResourcePaths.WinScreenScene)
                    .Value) is WinScreen winScene)
            {
                winScene.PlayerStats = PlayerStats;
            }
            GameManager.SaveManager.Delete();
        }
        else
        {
            ShowRegularBattleRewards();
        }
    }

    private void OnMapExited(Room room)
    {
        SaveRun(false);
        switch (room.RoomType)
        {
            case Room.Type.Unknown:
                ChangeView(ResourceLoaderManager.Instance
                    .GetSceneLoader(GameConstants.ResourcePaths.BattleRewardScene).Value);
                break;
            case Room.Type.Monster:
                OnBattleRoomEntered(room);
                break;
            case Room.Type.Treasure:
                OnTreasureRoomEntered();
                break;
            case Room.Type.Campfire:
                OnCampfireRoomEntered();
                break;
            case Room.Type.Shop:
                OnShopRoomEntered();
                break;
            case Room.Type.Boss:
                // ChangeView(ResourceLoaderManager.Instance
                //     .GetSceneLoader(GameConstants.ResourcePaths.BossScene).Value);
                OnBattleRoomEntered(room);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTreasureRoomEntered()
    {
        if (ChangeView(ResourceLoaderManager.Instance
                .GetSceneLoader(GameConstants.ResourcePaths.TreasureScene).Value) is not Treasure treasure)
        {
            return;
        }

        treasure.RelicHandler = RelicHandler;
        treasure.PlayerStats = PlayerStats;
        treasure.GenerateRelic();
    }

    /// <summary>
    /// 当玩家进入商店房间时触发的回调函数
    /// </summary>
    /// <remarks>
    /// 该函数负责加载商店场景并初始化商店的相关数据
    /// </remarks>
    private void OnShopRoomEntered()
    {
        // 尝试切换到商店场景，如果场景不存在则打印错误信息并返回
        if (ChangeView(ResourceLoaderManager.Instance
                .GetSceneLoader(GameConstants.ResourcePaths.ShopScene).Value) is not Shop shop)
        {
            GD.Print("ShopScene not found");
            return;
        }

        // 初始化商店的数据绑定
        shop.PlayerStats = PlayerStats;
        shop.RunStats = RunStats;
        shop.RelicHandler = RelicHandler;
        Events.Instance.RaiseShopEntered(shop);
        // 填充商店商品数据
        shop.PopulateShop();
    }


    private void OnCampfireRoomEntered()
    {
        if (ChangeView(ResourceLoaderManager.Instance
                .GetSceneLoader(GameConstants.ResourcePaths.CampfireScene).Value) is not Campfire campfire)
        {
            return;
        }

        campfire.PlayerStats = PlayerStats;
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
        SaveRun(true);
    }

    private void OnBattleRoomEntered(Room room)
    {
        if (ChangeView(ResourceLoaderManager.Instance
                .GetSceneLoader(GameConstants.ResourcePaths.BattleScene).Value) is not Battle battleScene)
        {
            GD.PrintErr("BattleScene is null");
            return;
        }

        battleScene.PlayerStats = PlayerStats;
        battleScene.BattleStats = room.BattleStats;
        battleScene.RelicHandler = RelicHandler;
        battleScene.StartBattle();
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
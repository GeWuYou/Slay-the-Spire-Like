using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.enemies;
using SlayTheSpireLike.scripts.extensions;
using SlayTheSpireLike.scripts.player;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts;

/// <summary>
///     战斗场景的主要控制类，负责管理玩家和敌人的回合流程、事件处理以及战斗初始化
/// </summary>
public partial class Battle : Node2D
{
    private BattleUi _battleUi;

    private Events _events;
    [Export] public CharacterStats PlayerStats { get; set; }

    [Export] public PlayerHandler PlayerHandler { get; set; }

    [Export] public EnemyHandler EnemyHandler { get; set; }

    [Export] public Player Player { get; set; }
    
    [Export] public AudioStream BattleMusic { get; set; }

    /// <summary>
    ///     当节点进入场景树时调用，初始化战斗UI并设置玩家属性
    /// </summary>
    public override void _EnterTree()
    {
        _battleUi = GetNode<BattleUi>("BattleUI");
        var newStats = PlayerStats.CreateInstance();
        Player.Stats = newStats;
        _battleUi.PlayerStats = newStats;
    }

    /// <summary>
    ///     节点准备就绪时调用，注册事件监听器并开始战斗
    /// </summary>
    public override void _Ready()
    {
        _events = Events.Instance;
        // 注册玩家回合结束事件
        _events.PlayerTurnEnded += PlayerHandler.EndTurn;
        // 注册敌人回合结束事件
        _events.EnemyTurnEnded += OnEnemyTurnEnded;
        // 注册玩家手牌丢弃事件
        _events.PlayerHandDiscarded += EnemyHandler.StartTurn;
        // 注册玩家死亡事件
        _events.PlayerDied += OnPlayerDied;
        // 注册敌人子节点顺序改变事件
        EnemyHandler.ChildOrderChanged += OnEnmiesChildOrderChanged;
        StartBattle(_battleUi.PlayerStats);
    }

    private static void OnPlayerDied()
    {
        Events.Instance.EmitSignal(Events.SignalName.BattleOverScreenRequested, "游戏结束！", BattleOverPanel.Type.Lose.GetBattleOverPanelTypeValue());
    }

    private void OnEnmiesChildOrderChanged()
    {
        if (EnemyHandler.GetChildCount() == 0) 
            Events.Instance.EmitSignal(Events.SignalName.BattleOverScreenRequested, "胜利！", BattleOverPanel.Type.Win.GetBattleOverPanelTypeValue());
    }

    /// <summary>
    ///     处理敌人回合结束后的逻辑，开始玩家的新回合并重置敌人行动状态
    /// </summary>
    private void OnEnemyTurnEnded()
    {
        GD.Print("敌人回合结束");
        PlayerHandler.StartTurn();
        EnemyHandler.ResetEnemyAcitons();
    }

    /// <summary>
    ///     初始化战斗，重置敌人行动状态并启动玩家战斗处理器
    /// </summary>
    /// <param name="newStats">玩家角色的属性统计信息</param>
    private void StartBattle(CharacterStats newStats)
    {
        GD.Print("开始战斗");
        AudioPlayerManager.Instance.PlayMusic(BattleMusic,true);
        EnemyHandler.ResetEnemyAcitons();
        PlayerHandler.StartBattle(newStats);
    }
}
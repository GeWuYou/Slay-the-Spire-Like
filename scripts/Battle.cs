using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.enemies;
using SlayTheSpireLike.scripts.player;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts;

/// <summary>
/// 战斗场景的主要控制类，负责管理玩家和敌人的回合流程、事件处理以及战斗初始化
/// </summary>
public partial class Battle : Node2D
{
    [Export] public CharacterStats PlayerStats { get; set; }
    private BattleUi _battleUi;
    [Export]
    public PlayerHandler PlayerHandler { get; set; }
    [Export]
    public EnemyHandler EnemyHandler { get; set; }
    
    [Export]
    public Player Player { get; set; }

    private Events _events;
    
    /// <summary>
    /// 当节点进入场景树时调用，初始化战斗UI并设置玩家属性
    /// </summary>
    public override void _EnterTree()
    {
        _battleUi = GetNode<BattleUi>("BattleUI");
        var newStats = PlayerStats.CreateInstance();
        Player.Stats = newStats;
        _battleUi.PlayerStats = newStats;
    }

    /// <summary>
    /// 节点准备就绪时调用，注册事件监听器并开始战斗
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
        StartBattle(_battleUi.PlayerStats);
    }

    /// <summary>
    /// 处理敌人回合结束后的逻辑，开始玩家的新回合并重置敌人行动状态
    /// </summary>
    private void OnEnemyTurnEnded()
    {
        GD.Print("敌人回合结束");
        PlayerHandler.StartTurn();
        EnemyHandler.ResetEnemyAcitons();
    }

    /// <summary>
    /// 初始化战斗，重置敌人行动状态并启动玩家战斗处理器
    /// </summary>
    /// <param name="newStats">玩家角色的属性统计信息</param>
    private void StartBattle(CharacterStats newStats)
    {
        EnemyHandler.ResetEnemyAcitons();
        PlayerHandler.StartBattle(newStats);
    }
}

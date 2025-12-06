using System;
using global::SlayTheSpireLike.global;
using Godot;
using SlayTheSpireLike.scripts.enemies;
using SlayTheSpireLike.scripts.enums;
using SlayTheSpireLike.scripts.global;
using SlayTheSpireLike.scripts.player;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.battle;

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
    [Export] public BattleStats BattleStats { get; set; }
    public RelicHandler RelicHandler { get; set; }

    /// <summary>
    ///     当节点进入场景树时调用，初始化战斗UI并设置玩家属性
    /// </summary>
    public override void _EnterTree()
    {
        _battleUi = GetNode<BattleUi>("BattleUI");
    }

    /// <summary>
    ///     节点准备就绪时调用，注册事件监听器并开始战斗
    /// </summary>
    public override void _Ready()
    {
        GetParent();
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
        EnemyHandler.ChildOrderChanged += OnEnemiesChildOrderChanged;
    }

    public override void _ExitTree()
    {
        if (_events != null)
        {
            _events.PlayerTurnEnded -= PlayerHandler.EndTurn;
            _events.EnemyTurnEnded -= OnEnemyTurnEnded;
            _events.PlayerHandDiscarded -= EnemyHandler.StartTurn;
            _events.PlayerDied -= OnPlayerDied;
        }

        // 移除 ChildOrderChanged 事件监听器
        EnemyHandler.ChildOrderChanged -= OnEnemiesChildOrderChanged;
    }

    private static void OnPlayerDied()
    {
        Events.Instance.RaiseBattleOverScreenRequested("游戏结束！", BattleOverPanel.Type.Lose);
        GameManager.SaveManager.Delete();
    }

    private void OnEnemiesChildOrderChanged()
    {
        if (EnemyHandler.GetChildCount() == 0 && IsInstanceValid(RelicHandler))
            RelicHandler.ActivateRelicsByType(RelicType.EndOfCombat);
    }

    /// <summary>
    ///     处理敌人回合结束后的逻辑，开始玩家的新回合并重置敌人行动状态
    /// </summary>
    private void OnEnemyTurnEnded()
    {
        PlayerHandler.StartTurn();
        EnemyHandler.ResetEnemyAcitons();
    }

    /// <summary>
    ///     初始化战斗，重置敌人行动状态并启动玩家战斗处理器
    /// </summary>
    public void StartBattle()
    {
        GetTree().Paused = false;
        _battleUi.PlayerStats = PlayerStats;
        Player.Stats = PlayerStats;
        EnemyHandler.SetupEnemies(BattleStats);
        EnemyHandler.ResetEnemyAcitons();
        AudioPlayerManager.Instance.PlayMusic(BattleMusic, true);
        PlayerHandler.RelicHandler = RelicHandler;
        RelicHandler.Connect(RelicHandler.SignalName.RelicsActivated, new Callable(this,nameof(OnRelicActivated)));
        RelicHandler.ActivateRelicsByType(RelicType.StartOfCombat);
    }

    private void OnRelicActivated(RelicType type)
    {
        switch (type)
        {
            case RelicType.StartOfTurn:
                break;
            case RelicType.StartOfCombat:
                PlayerHandler.StartBattle(PlayerStats);
                _battleUi.InitCardPileUi();
                break;
            case RelicType.EndOfTurn:
                break;
            case RelicType.EndOfCombat:
                Events.Instance.RaiseBattleOverScreenRequested("胜利！", BattleOverPanel.Type.Win);
                break;
            case RelicType.EventBased:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
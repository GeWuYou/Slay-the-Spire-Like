using System;
using System.Threading.Tasks;
using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.enemies;

/// <summary>
///     敌人类，继承自Area2D，用于表示游戏中的敌人单位。
///     负责管理敌人的属性、UI显示以及受到伤害后的逻辑处理。
/// </summary>
public partial class Enemy : Area2D
{
    private EnemyAction _currentAction;

    private EnemyStats _stats;
    [Export] public int ArrowOffset { get; set; } = 5;

    [Export] public IntentUi IntentUi { get; set; }

    public EnemyActionPicker EnemyActionPicker { get; set; }

    public EnemyAction CurrentAction
    {
        get => _currentAction;
        set
        {
            _currentAction = value;
            if (_currentAction != null) IntentUi.UpdateIntent(_currentAction.Intent);
        }
    }

    /// <summary>
    ///     获取或设置敌人的统计数据。当设置新值时会自动克隆一份实例以防止引用共享，
    ///     并确保只连接一次StatsChanged事件来更新UI。
    /// </summary>
    [Export]
    public EnemyStats Stats
    {
        get => _stats;
        set
        {
            _stats = value.CreateInstance();
            // 检查是否已连接属性变化事件，避免重复连接
            if (!_stats.IsConnected(resources.Stats.SignalName.StatsChanged,
                    Callable.From(UpdateStats)))
            {
                _stats.StatsChanged += UpdateStats;
                _stats.StatsChanged += UpdateAction;
            }

            _ = UpdateEnemy();
        }
    }


    [Export] public Sprite2D Sprite2D { get; set; }
    [Export] public StatsUi StatsUi { get; set; }
    [Export] public Sprite2D Arrow { get; set; }

    public void UpdateAction()
    {
        if (EnemyActionPicker is null) return;

        if (CurrentAction is null)
        {
            CurrentAction = EnemyActionPicker.GetAction();
            return;
        }

        var action = EnemyActionPicker.GetFirstConditionalAction();
        if (action is not null && CurrentAction != action) CurrentAction = action;
    }

    public void SetupAi()
    {
        EnemyActionPicker?.QueueFree();
        var newPicker = _stats.Ai.Instantiate();
        AddChild(newPicker);
        EnemyActionPicker = newPicker as EnemyActionPicker;
        if (EnemyActionPicker != null) EnemyActionPicker.Enemy = this;
    }

    /// <summary>
    ///     初始化节点，在_ready阶段注册区域进入和退出的事件监听器。
    /// </summary>
    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }


    /// <summary>
    ///     当有其他区域离开本敌人区域范围时调用，隐藏指示箭头。
    /// </summary>
    /// <param name="area">离开的区域对象。</param>
    private void OnAreaExited(Area2D area)
    {
        Arrow.Hide();
    }

    /// <summary>
    ///     当有其他区域进入本敌人区域范围时调用，显示指示箭头。
    /// </summary>
    /// <param name="area">进入的区域对象。</param>
    private void OnAreaEntered(Area2D area)
    {
        Arrow.Show();
    }

    /// <summary>
    ///     当角色属性发生变化时调用此方法更新UI界面中展示的数值。
    /// </summary>
    private void UpdateStats()
    {
        StatsUi.UpdateStats(Stats);
    }

    public void DoTurn()
    {
        Stats.Block = 0;
        CurrentAction?.PerformAction();
    }

    /// <summary>
    ///     异步更新敌人相关的视觉元素与数据状态。
    ///     包括加载角色贴图、调整箭头位置，并触发一次统计数据刷新。
    /// </summary>
    private async Task UpdateEnemy()
    {
        try
        {
            // 检查角色属性是否存在
            if (_stats is not { } characterStats) return;

            // 等待节点准备就绪
            if (!IsInsideTree()) await ToSignal(this, GameConstants.Signals.Ready);

            // 更新角色图像
            Sprite2D.Texture = characterStats.Art as Texture2D;
            Arrow.Position = Vector2.Right * (Sprite2D.GetRect().Size.X / 2 + ArrowOffset);
            SetupAi();
            UpdateStats();
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
    }

    /// <summary>
    ///     对当前敌人造成指定数量的伤害。
    ///     若敌人生命值降至0或以下，则从场景中移除该敌人对象。
    /// </summary>
    /// <param name="damage">要造成的伤害点数。</param>
    public void TakeDamage(int damage)
    {
        // 如果敌人已经死亡，则直接返回
        if (Stats.Health <= 0) return;
        Sprite2D.Material = ResourceFactories.WhiteSpriteMatFactory();
        // 创建动画序列来处理伤害效果
        var tween = CreateTween();
        tween.TweenCallback(Callable.From(() => Shaker.Instance.Shake(this, 16, 0.15f)));
        tween.TweenCallback(Callable.From(() => Stats.TakeDamage(damage)));
        tween.TweenInterval(0.17f);

        // 动画完成后检查敌人是否死亡，如果死亡则从场景中移除
        tween.Finished += () =>
        {
            Sprite2D.Material = null;
            if (Stats.Health <= 0) QueueFree();
        };
    }
}
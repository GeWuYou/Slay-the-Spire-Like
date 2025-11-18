using System;
using System.Threading.Tasks;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.player;

/// <summary>
///     玩家角色类，继承自Node2D节点。负责管理玩家的角色属性、显示和伤害处理。
/// </summary>
public partial class Player : Node2D
{
    private CharacterStats _stats;

    /// <summary>
    ///     获取或设置玩家的角色属性。当设置时会创建一个新的属性实例并连接属性变化事件。
    /// </summary>
    public CharacterStats Stats
    {
        get => _stats;
        set
        {
            _stats = value;
            // 检查是否已连接属性变化事件，避免重复连接
            if (!_stats.IsConnected(resources.Stats.SignalName.StatsChanged,
                    Callable.From(UpdateStats)))
                _stats.StatsChanged += UpdateStats;

            _ = UpdatePlayer();
        }
    }

    [Export] public Sprite2D Sprite2D { get; set; }
    [Export] public StatsUi StatsUi { get; set; }

    /// <summary>
    ///     异步更新玩家显示信息，包括角色图像和属性UI。
    /// </summary>
    /// <returns>表示异步操作的任务</returns>
    private async Task UpdatePlayer()
    {
        try
        {
            // 检查角色属性是否存在
            if (_stats is not { } characterStats) return;

            // 等待节点准备就绪
            if (!IsInsideTree()) await ToSignal(this, GameConstants.Signals.Ready);

            // 更新角色图像
            Sprite2D.Texture = characterStats.Art as Texture2D;
            UpdateStats();
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
    }

    /// <summary>
    ///     更新玩家属性UI显示。
    /// </summary>
    private void UpdateStats()
    {
        StatsUi.UpdateStats(Stats);
    }

    /// <summary>
    ///     对玩家造成伤害。如果生命值降至0或以下，则移除玩家节点。
    /// </summary>
    /// <param name="damage">造成的伤害值</param>
    public void TakeDamage(int damage)
    {
        // 如果玩家已经死亡，直接返回
        if (Stats.Health <= 0) return;

        Sprite2D.Material = ResourceFactories.WhiteSpriteMatFactory();
        var tween = CreateTween();
        // 添加震动效果和伤害处理的回调函数
        tween.TweenCallback(Callable.From(() => Shaker.Instance.Shake(this, 16, 0.15f)));
        tween.TweenCallback(Callable.From(() => Stats.TakeDamage(damage)));
        tween.TweenInterval(0.17f);

        // 伤害处理完成后的回调函数
        tween.Finished += () =>
        {
            Sprite2D.Material = null;
            // 检查玩家是否死亡
            if (Stats.Health > 0) return;

            Events.Instance.EmitSignal(Events.SignalName.PlayerDied);
            QueueFree();
        };
    }
}
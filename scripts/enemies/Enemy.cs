using System;
using System.Threading.Tasks;
using DeckBuilderTutorial.scripts.resources;
using DeckBuilderTutorial.scripts.ui;
using Godot;

namespace DeckBuilderTutorial.scripts.enemies;

/// <summary>
/// 敌人类，继承自Area2D，用于表示游戏中的敌人单位。
/// 负责管理敌人的属性、UI显示以及受到伤害后的逻辑处理。
/// </summary>
public partial class Enemy : Area2D
{
    [Export] public int ArrowOffset { get; set; } = 5;


    private Stats _stats;


    [Export]
    public Stats Stats
    {
        get => _stats;
        set
        {
            _stats = value.CreateInstance();
            // 检查是否已连接属性变化事件，避免重复连接
            if (!_stats.IsConnected(Stats.SignalName.StatsChanged,
                    Callable.From(UpdateStats)))
            {
                _stats.StatsChanged += UpdateStats;
            }

            _ = UpdateEnemy();
        }
    }


    [Export] public Sprite2D Sprite2D { get; set; }
    [Export] public StatsUi StatsUi { get; set; }
    [Export] public Sprite2D Arrow { get; set; }
    

    /// <summary>
    /// 当角色属性发生变化时调用此方法更新UI界面中展示的数值。
    /// </summary>
    private void UpdateStats()
    {
        StatsUi.UpdateStats(Stats);
    }

    /// <summary>
    /// 异步更新敌人相关的视觉元素与数据状态。
    /// 包括加载角色贴图、调整箭头位置，并触发一次统计数据刷新。
    /// </summary>
    private async Task UpdateEnemy()
    {
        try
        {
            // 检查角色属性是否存在
            if (_stats is not { } characterStats)
            {
                return;
            }

            // 等待节点准备就绪
            if (!IsInsideTree())
            {
                await ToSignal(this, "ready");
            }

            // 更新角色图像
            Sprite2D.Texture = characterStats.Art as Texture2D;
            Arrow.Position = Vector2.Right * (Sprite2D.GetRect().Size.X / 2 + ArrowOffset);
            UpdateStats();
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
    }
  
    /// <summary>
    /// 对当前敌人造成指定数量的伤害。
    /// 若敌人生命值降至0或以下，则从场景中移除该敌人对象。
    /// </summary>
    /// <param name="damage">要造成的伤害点数。</param>
    public void TakeDamage(int damage)
    {
        if (Stats.Health <= 0)
        {
            return;
        }
        Stats.TakeDamage(damage);
        if (Stats.Health <= 0)
        {
            QueueFree();
        }
    }
}

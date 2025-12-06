using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.effects;

namespace SlayTheSpireLike.scripts.enemies.toxic_ghost;

/// <summary>
/// 敌人动作模板类，用于定义敌人的具体行为动作
/// 继承自EnemyAction基类，需要实现具体的动作执行逻辑
/// </summary>
public partial class ToxicGhostMuscleBuffAction : EnemyAction
{
    [Export] public int 力量堆叠数 { get; set; } = 2;
    [Export] public int 最大使用次数 { get; set; } = 2;
    [Export] public int CurrentUsages { get; set; }
    [Export] public int 血量触发阈值 { get; set; } = 25;

    public override bool IsPerformable()
    {
        if (CurrentUsages >= 最大使用次数 || Enemy.Stats.Health > 血量触发阈值)
        {
            return false;
        }

        CurrentUsages++;
        return true;
    }
    
    public override void PerformAction()
    {
        // 检查敌人和目标是否存在
        if (Enemy == null || Target == null) return;
        var statusEffect = new StatusEffect();
        var muscle = ResourceFactories.MuscleStatusFactory();
        muscle.Stacks = 力量堆叠数;
        statusEffect.Status = muscle;
        statusEffect.Execute([Enemy]);
        AudioPlayerManager.Instance.Play(Sound,AudioPlayerManager.PlayerType.Sfx);
        Events.Instance.RaiseEnemyActionCompleted(Enemy);
    }
}
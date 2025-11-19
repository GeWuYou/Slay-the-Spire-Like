using SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.effects;

namespace SlayTheSpireLike.scripts.enemies.carb;

/// <summary>
///     敌人攻击动作类，继承自EnemyAction
///     负责执行敌人的攻击行为，包括移动、造成伤害和返回原位的动画过程
/// </summary>
public partial class CarbAttackAction : EnemyAction
{
    [Export] public int Damage { get; set; } = 5;

    /// <summary>
    ///     执行敌人攻击动作
    ///     包含敌人向目标移动、造成伤害、然后返回原位的完整动画流程
    /// </summary>
    public override void PerformAction()
    {
        // 检查敌人和目标是否存在
        if (Enemy == null || Target == null) return;

        // 创建补间动画并设置缓动类型
        var tween = CreateTween().SetTrans(Tween.TransitionType.Quint);

        // 计算起始位置和目标位置
        var start = Enemy.GlobalPosition;
        var end = Target.GlobalPosition + Vector2.Right * 32;

        // 创建伤害效果并设置伤害数值
        var damageEffect = new DamageEffect();
        Array<Node> targetArray = [Target];
        damageEffect.Amount = Damage;
        damageEffect.Sound = Sound;

        // 执行攻击动画序列：移动到目标位置 -> 造成伤害 -> 等待 -> 返回起始位置
        tween.TweenProperty(Enemy, "global_position", end, 0.4f);
        tween.TweenCallback(Callable.From(() => damageEffect.Execute(targetArray)));
        tween.TweenInterval(0.25f);
        tween.TweenProperty(Enemy, "global_position", start, 0.4f);
        // 动画完成后发出信号表示敌人行动结束
        tween.Finished += () => Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, Enemy);
    }
}
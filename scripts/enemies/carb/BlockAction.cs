using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.effects;

namespace SlayTheSpireLike.scripts.enemies.carb;

/// <summary>
///     敌人执行格挡动作的类，继承自EnemyAction
///     该类负责处理敌人给自己或目标添加格挡效果的逻辑
/// </summary>
public partial class BlockAction : EnemyAction
{
    [Export] public int Block { get; set; } = 6;

    /// <summary>
    ///     执行格挡动作
    ///     该方法会创建一个格挡效果并应用到目标身上，然后在延迟后发出动作完成信号
    /// </summary>
    public override void PerformAction()
    {
        // 检查敌方单位和目标是否存在，如果任一为空则直接返回
        if (Enemy == null || Target == null) return;

        // 创建格挡效果实例并设置格挡数值
        var blockEffect = new BlockEffect();
        blockEffect.Amount = Block;
        // 执行格挡效果，应用到目标身上
        blockEffect.Execute([Enemy]);

        // 创建定时器，在0.6秒后触发动作完成事件
        GetTree().CreateTimer(0.6f, false).Timeout += () =>
        {
            Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted, Enemy);
        };
    }
}
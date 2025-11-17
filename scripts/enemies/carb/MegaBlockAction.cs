using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.effects;

namespace SlayTheSpireLike.scripts.enemies.carb;

/// <summary>
/// 敌人动作类，用于处理巨型方块敌人的特殊行动
/// 当敌人的生命值低于指定阈值时，执行格挡效果
/// </summary>
public partial class MegaBlockAction : EnemyAction
{
    [Export] public int Block { get; set; } = 10;
    [Export] public int HpThreshold { get; set; } = 13;

    private bool _alreadyUsed;

    /// <summary>
    /// 检查当前动作是否可以执行
    /// </summary>
    /// <returns>当敌人存在且未使用过此动作，且生命值低于阈值时返回true，否则返回false</returns>
    public override bool IsPerformable()
    {
        // 检查敌人是否存在或动作是否已使用
        if (Enemy is null || _alreadyUsed)
        {
            return false;
        }

        // 判断敌人生命值是否低于阈值
        var isLow = Enemy.Stats.Health <= HpThreshold;
        _alreadyUsed = isLow;
        return isLow;
    }

    /// <summary>
    /// 执行动作效果，为目标添加格挡值
    /// </summary>
    public override void PerformAction()
    {
        // 检查敌人和目标是否存在
        if (Enemy is null || Target is null)
        {
            return;
        }

        // 创建并执行格挡效果
        var blockEffect = new BlockEffect();
        blockEffect.Amount = Block;
        blockEffect.Execute([Target]);
        
        // 延迟触发动作完成事件
        GetTree().CreateTimer(0.6f, false).Timeout += () =>
        {
            Events.Instance.EmitSignal(Events.SignalName.EnemyActionCompleted);
        };
    }
}

using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.effects;

namespace SlayTheSpireLike.scripts.statuses;

/// <summary>
/// 状态效果模板类，继承自Status基类。
/// 用于创建自定义的状态效果，可以定义状态的初始化逻辑和应用逻辑。
/// </summary>
public partial class TrueStrengthFormStatus : Status
{
    [Export] public int StacksPerTurn { get; set; }= 3;
    public override string GetTooltip()
    {
        return string.Format(base.GetTooltip(),StacksPerTurn);
    }

    /// <summary>
    /// 将当前状态应用于指定的目标节点，并发出状态变更信号
    /// </summary>
    /// <param name="target">要应用状态的目标节点</param>
    public override void ApplyStatus(Node target)
    {
        var statusEffect = new StatusEffect();
        var muscleStatus = ResourceFactories.MuscleStatusFactory();
        muscleStatus.Stacks = StacksPerTurn;
        statusEffect.Status = muscleStatus;
        statusEffect.Execute([target]);
        EmitSignal(Status.SignalName.StatusApplied, this);
    }
}
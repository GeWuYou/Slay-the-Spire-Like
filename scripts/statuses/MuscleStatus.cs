using Godot;
using SlayTheSpireLike.scripts.component;
using SlayTheSpireLike.scripts.modifier_handler;

namespace SlayTheSpireLike.scripts.statuses;

/// <summary>
/// 肌肉状态类，用于增加角色造成的伤害
/// 该状态通过修改伤害修饰器来提升目标的攻击力
/// </summary>
public partial class MuscleStatus : Status
{
    public const string Muscle = "Muscle";
    
    /// <summary>
    /// 获取状态的工具提示文本
    /// </summary>
    /// <returns>格式化后的工具提示字符串，显示状态层数</returns>
    public override string GetTooltip()
    {
        return string.Format(base.GetTooltip(),Stacks);
    }

    /// <summary>
    /// 初始化状态效果，在状态被添加到目标时调用。
    /// 可以在此方法中设置初始值或注册事件监听器。
    /// </summary>
    /// <param name="target">状态效果被应用的目标节点</param>
    public override void InitializeStatus(Node target)
    {
        StatusChanged += () => OnStatusChanged(target);
        OnStatusChanged(target);
    }

    /// <summary>
    /// 当状态发生变化时的回调处理方法。
    /// 更新目标节点的伤害修饰器数值，将状态层数作为平坦伤害加成应用。
    /// </summary>
    /// <param name="target">状态效果作用的目标节点</param>
    private void OnStatusChanged(Node target)
    {
        // 检查目标是否实现了IModifierComponent接口
        if (target is not IModifierComponent modifierComponent)
        {
            return;
        }
        
        // 获取目标的伤害修饰器并更新肌肉状态对应的数值
        var modifier = modifierComponent.ModifierHandler.GetModifier(Modifier.ModifierType.DmgDealt);
        // 获取或创建肌肉属性的修饰器值
        // 如果不存在则创建一个基于百分比的平面修饰器值
        var value =  modifier.GetValue(Muscle) ?? 
                     ModifierValue.CreatePercentBased(Muscle,ModifierValue.ModifierValueType.Flat);
        
        // 设置修饰器的平面值为当前堆叠数
        value.FlatValue = Stacks;
        
        // 将修改后的值添加回修饰器中
        modifier.AddValue(value);

    }

    /// <summary>
    /// 将当前状态应用于指定的目标节点，并发出状态变更信号
    /// </summary>
    /// <param name="target">要应用状态的目标节点</param>
    public override void ApplyStatus(Node target)
    {
        EmitSignal(Status.SignalName.StatusApplied, this);
    }
}

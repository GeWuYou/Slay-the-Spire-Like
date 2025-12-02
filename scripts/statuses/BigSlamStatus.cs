using Godot;
using SlayTheSpireLike.scripts.component;
using SlayTheSpireLike.scripts.modifier_handler;

namespace SlayTheSpireLike.scripts.statuses;


/// <summary>
/// 状态效果模板类，继承自Status基类。
/// 用于创建自定义的状态效果，可以定义状态的初始化逻辑和应用逻辑。
/// </summary>
public partial class BigSlamStatus : Status
{
	public const string BigSlam = "BigSlam";
	
	/// <summary>
	/// 伤害倍率
	/// </summary>
	[Export]
	public float Ratio { get; set; } = 2f;
	
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
        
		// 获取目标的伤害修饰器并更新BigSlam状态对应的数值
		var modifier = modifierComponent.ModifierHandler.GetModifier(Modifier.ModifierType.DmgDealt);
		// 如果不存在则创建一个基于百分比的平面修饰器值
		var value =  modifier.GetValue(BigSlam) ?? 
		             ModifierValue.CreatePercentBased(BigSlam,ModifierValue.ModifierValueType.Flat);
        
		// 获取肌肉状态的层数，并应用2倍比率
		int muscleStacks = 0;
		var muscleValue = modifier.GetValue(MuscleStatus.Muscle);
		if (muscleValue != null)
		{
			muscleStacks = muscleValue.FlatValue;
		}
		
		// 设置修饰器的平面值为当前堆叠数乘以比率
		value.FlatValue = (int)(muscleStacks * Ratio);
        
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
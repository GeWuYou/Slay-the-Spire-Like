// meta-name: 状态效果模板
// meta-description: 状态效果脚本模板
using Godot;
namespace SlayTheSpireLike.scripts.statuses;


/// <summary>
/// 状态效果模板类，继承自Status基类。
/// 用于创建自定义的状态效果，可以定义状态的初始化逻辑和应用逻辑。
/// </summary>
public partial class _CLASS_ : Status
{
	/// <summary>
	/// 示例成员变量，可以根据实际需要修改或删除。
	/// </summary>
	[Export] public int Member { get; set; }
	
	/// <summary>
	/// 初始化状态效果，在状态被添加到目标时调用。
	/// 可以在此方法中设置初始值或注册事件监听器。
	/// </summary>
	/// <param name="target">状态效果被应用的目标节点</param>
	public override void InitializeStatus(Node target)
	{
        
	}
	
	/// <summary>
	/// 将当前状态应用于指定的目标节点，并发出状态变更信号
	/// </summary>
	/// <param name="target">要应用状态的目标节点</param>
	public override void ApplyStatus(Node target)
	{
		EmitSignal(SignalName.StatusChanged,this);
	}
}


using Godot;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// 状态资源类，用于表示游戏中的各种状态效果
/// 该类继承自Godot的Resource基类，定义了状态的基本属性和事件
/// </summary>
[GlobalClass]
public partial class Status : Resource
{
    private int _duration;
    private int _stacks;

    /// <summary>
    /// 状态改变事件处理器委托
    /// 当状态发生改变时触发此事件
    /// </summary>
    [Signal]
    public delegate void StatusChangedEventHandler();

    /// <summary>
    /// 状态应用事件处理器委托
    /// 当状态被应用到目标时触发此事件
    /// </summary>
    /// <param name="status">被应用的状态实例</param>
    [Signal]
    public delegate void StatusAppliedEventHandler(Status status);

    /// <summary>
    /// 状态类型枚举，定义了状态的触发时机
    /// </summary>
   public  enum StatusType
    {
        /// <summary>
        /// 回合开始时触发的状态
        /// </summary>
        StartOfTurn,
        /// <summary>
        /// 回合结束时触发的状态
        /// </summary>
        EndOfTurn,
        /// <summary>
        /// 基于事件触发的状态
        /// </summary>
        EventBased
    }

    /// <summary>
    /// 状态堆叠类型枚举，定义了状态如何堆叠的规则
    /// </summary>
    public enum StackType
    {
        /// <summary>
        /// 不可堆叠
        /// </summary>
        None,
        /// <summary>
        /// 按强度堆叠
        /// </summary>
        Intensity,
        /// <summary>
        /// 按持续时间堆叠
        /// </summary>
        Duration
    }

    [ExportGroup("状态数据")]
    [Export]
    public string Id { get; set; }
    [Export]
    public StatusType Type { get; set; }
    [Export]
    public StackType StatusStackType { get; set; }
    [Export]
    public bool CanExpire { get; set; }
    
    /// <summary>
    /// 获取或设置状态的持续时间
    /// 设置值后会延迟调用SetDuration方法并发出状态变更信号
    /// </summary>
    [Export]
    public int Duration
    {
        get => _duration;
        set
        {
            _duration = value;
            CallDeferred(nameof(SetDuration),value);
        }
    }

    /// <summary>
    /// 获取或设置状态的堆叠层数
    /// 设置值后会延迟调用SetStacks方法并发出状态变更信号
    /// </summary>
    [Export]
    public int Stacks
    {
        get => _stacks;
        set
        {
            _stacks = value;
            CallDeferred(nameof(SetStacks),value);
        }
    }
    [ExportGroup("状态视觉")]
    [Export]
    public Texture Icon { get; set; }
    [Export(PropertyHint.MultilineText)] public string Tooltip { get; set; }

    /// <summary>
    /// 将当前状态应用于指定的目标节点，并发出状态变更信号
    /// </summary>
    /// <param name="target">要应用状态的目标节点</param>
    public void ApplyStatus(Node target)
    {
        EmitSignal(SignalName.StatusChanged,this);
    }

    /// <summary>
    /// 获取状态的工具提示文本
    /// </summary>
    /// <returns>返回状态的工具提示字符串</returns>
    public string GetTooltip()
    {
        return Tooltip;
    }
    
    /// <summary>
    /// 设置状态的持续时间并发出状态变更信号
    /// </summary>
    /// <param name="duration">新的持续时间值</param>
    public void SetDuration(int duration)
    {
        EmitSignal(SignalName.StatusChanged);
    }
    
    /// <summary>
    /// 设置状态的堆叠层数并发出状态变更信号
    /// </summary>
    /// <param name="stacks">新的堆叠层数值</param>
    public void SetStacks(int stacks)
    {
        EmitSignal(SignalName.StatusChanged);
    }
}

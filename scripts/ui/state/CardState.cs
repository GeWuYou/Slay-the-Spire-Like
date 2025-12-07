using global::SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.ui.state;

/// <summary>
///     卡牌状态管理类，负责处理卡牌状态转换相关的逻辑
///     继承自Node节点，用于在游戏场景中进行状态管理
/// </summary>
public partial class CardState : Node
{
    /// <summary>
    ///     状态转换请求信号，当需要进行状态转换时触发此信号
    ///     参数from: 转换前的状态
    ///     参数to: 转换后的状态目标
    /// </summary>
    [Signal]
    public delegate void TransitionRequestedEventHandler(CardState from, State to);

    /// <summary>
    ///     卡牌状态枚举，定义了卡牌可能处于的各种状态
    /// </summary>
    public enum State
    {
        /// <summary>
        ///     基础状态，卡牌处于默认状态
        /// </summary>
        Base,

        /// <summary>
        ///     已点击状态，卡牌被用户点击
        /// </summary>
        Clicked,

        /// <summary>
        ///     拖拽状态，卡牌正在被拖拽
        /// </summary>
        Dragging,

        /// <summary>
        ///     瞄准状态，卡牌正在瞄准目标
        /// </summary>
        Aiming,

        /// <summary>
        ///     已释放状态，卡牌被释放
        /// </summary>
        Released
    }

    protected Events Events;

    /// <summary>
    ///     获取当前卡牌所处的状态值
    /// </summary>
    [Export] public State StateValue { get; private set; }

    /// <summary>
    ///     关联的卡牌UI组件引用
    /// </summary>
    public CardUi CardUi { get; set; }

    /// <summary>
    ///     初始化方法，在节点准备就绪时调用
    ///     从全局单例获取事件系统实例并赋值给本地字段
    /// </summary>
    public override void _Ready()
    {
        Events = Events.Instance;
    }

    /// <summary>
    ///     进入操作
    ///     子类可重写该方法以实现进入特定状态时的初始化逻辑
    /// </summary>
    public virtual void Enter()
    {
    }

    /// <summary>
    ///     退出操作
    ///     子类可重写该方法以实现在退出当前状态时的清理逻辑
    /// </summary>
    public virtual void Exit()
    {
    }

    /// <summary>
    ///     处理输入事件的方法
    ///     子类可重写该方法来响应不同状态下的输入行为
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public virtual void OnInput(InputEvent @event)
    {
    }

    /// <summary>
    ///     处理GUI输入事件的方法
    ///     子类可重写该方法来响应与UI交互相关的输入事件
    /// </summary>
    /// <param name="event">GUI输入事件对象</param>
    public virtual void OnGuiInput(InputEvent @event)
    {
    }

    /// <summary>
    ///     鼠标进入事件处理方法
    ///     当鼠标光标进入关联的UI元素时调用
    ///     可由子类重写以执行高亮或其他视觉反馈逻辑
    /// </summary>
    public virtual void OnMouseEntered()
    {
    }

    /// <summary>
    ///     鼠标退出事件处理方法
    ///     当鼠标光标离开关联的UI元素时调用
    ///     可由子类重写以取消高亮或恢复原始样式
    /// </summary>
    public virtual void OnMouseExited()
    {
    }
    
    /// <summary>
    ///     在进入状态之后执行的操作
    ///     提供一个时机用于执行必须在状态完全设置好后才能运行的逻辑
    /// </summary>
    public virtual void PostEnter()
    {
        
    }
}

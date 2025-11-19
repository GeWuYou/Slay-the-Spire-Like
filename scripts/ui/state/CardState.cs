using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.extensions;

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

    [Export] public State StateValue { get; private set; }

    public CardUi CardUi { get; set; }

    public override void _Ready()
    {
        TransitionRequested += (from, to) => { GD.Print($"{from.StateValue} 转换为 {to}"); };
        Events = Events.Instance;
    }

    /// <summary>
    ///     进入操作
    /// </summary>
    public virtual void Enter()
    {
    }

    /// <summary>
    ///     退出操作
    /// </summary>
    public virtual void Exit()
    {
    }

    /// <summary>
    ///     处理输入事件的方法
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public virtual void OnInput(InputEvent @event)
    {
    }

    /// <summary>
    ///     处理GUI输入事件的方法
    /// </summary>
    /// <param name="event">GUI输入事件对象</param>
    public virtual void OnGuiInput(InputEvent @event)
    {
    }

    /// <summary>
    ///     鼠标进入事件处理方法
    ///     当鼠标光标进入关联的UI元素时调用
    /// </summary>
    public virtual void OnMouseEntered()
    {
    }

    /// <summary>
    ///     鼠标退出事件处理方法
    ///     当鼠标光标离开关联的UI元素时调用
    /// </summary>
    public virtual void OnMouseExited()
    {
    }
}
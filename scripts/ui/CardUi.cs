using Godot;

namespace DeckBuilderTutorial.scripts.ui;

/// <summary>
/// CardUi 类表示一个卡片用户界面控件
/// 该类继承自 Control，用于显示卡片的视觉元素和状态信息
/// </summary>
public partial class CardUi : Control
{
    /// <summary>
    /// 颜色矩形组件，用于显示卡片的背景颜色
    /// 通过 Export 特性暴露给编辑器，允许在编辑器中进行配置
    /// </summary>
    [Export]
    public ColorRect ColorRect { private set; get; }

    /// <summary>
    /// 标签组件，用于显示卡片的状态信息
    /// 通过 Export 特性暴露给编辑器，允许在编辑器中进行配置
    /// </summary>
    [Export]
    public Label StateText { private set; get; }


    /// <summary>
    /// 获取或设置掉落点检测器区域
    /// </summary>
    /// <remarks>
    /// 该属性用于检测物品掉落的目标区域，通过Area2D节点实现碰撞检测功能
    /// </remarks>
    [Export]
    public Area2D DropPointDetector { private set; get; }

    /// <summary>
    /// 获取或设置卡片状态机
    /// </summary>
    [Export]
    public CardStateMachine StateMachine { private set; get; }

    /// <summary>
    /// ReparentRequested 信号委托
    /// 当需要重新设置父级容器时触发此信号
    /// </summary>
    /// <param name="cardUi">触发信号的 CardUi 实例</param>
    [Signal]
    public delegate void ReparentRequestedEventHandler(CardUi cardUi);

    /// <summary>
    /// 当节点准备就绪时调用此方法
    /// 初始化状态机并将其与当前卡片UI实例关联
    /// </summary>
    public override void _Ready()
    {
        StateMachine.Init(this);
    }

    public override void _Input(InputEvent @event)
    {
        StateMachine.OnInput(@event);
    }

    public void OnGuiInput(InputEvent @event)
    {
        StateMachine.OnGuiInput(@event);
    }

    public void OnMouseEntered()
    {
        StateMachine.OnMouseEntered();
    }

    public void OnMouseExited()
    {
        StateMachine.OnMouseExited();
    }
}
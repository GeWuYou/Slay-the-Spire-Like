using DeckBuilderTutorial.scripts.resources;
using Godot;
using Godot.Collections;

namespace DeckBuilderTutorial.scripts.ui;

/// <summary>
/// CardUi 类表示一个卡片用户界面控件。
/// 该类继承自 Control，用于显示卡片的视觉元素和状态信息，并管理其交互逻辑。
/// </summary>
public partial class CardUi : Control
{
    private Card _card;
    [ExportGroup("卡片UI样式属性")] 
    [Export] public StyleBoxFlat BaseStyleBox { private set; get; }
    [Export] public StyleBoxFlat HoverStyleBox { private set; get; }
    [Export] public StyleBoxFlat SelectedStyleBox { private set; get; }
    [ExportGroup("卡片UI属性")] [Export] public Panel Panel { get; private set; }

    [Export] public Label Cost { get; private set; }
    [Export] public TextureRect Icon { get; private set; }

    /// <summary>
    /// 获取或设置掉落点检测器区域。
    /// 该属性用于检测物品掉落的目标区域，通过Area2D节点实现碰撞检测功能。
    /// </summary>
    [Export]
    public Area2D DropPointDetector { private set; get; }

    /// <summary>
    /// 获取或设置卡片状态机。
    /// </summary>
    [Export]
    public CardStateMachine StateMachine { private set; get; }

    /// <summary>
    /// 获取目标节点数组。
    /// 该属性提供对目标节点集合的访问，使用Godot的Export特性标记为可导出。
    /// </summary>
    [Export]
    public Array<Node> Targets { private set; get; } = [];

    /// <summary>
    /// 获取或设置当前绑定的卡片数据模型。
    /// </summary>
    [Export]
    public Card Card
    {
        get => _card;
        set
        {
            if (_card == value)
            {
                return;
            }

            _card = value;
            if (IsNodeReady())
            {
                OnCardChanged();
            }
            else
            {
                Callable.From(OnCardChanged).CallDeferred();
            }
        }
    }

    private void OnCardChanged()
    {
        if (_card == null)
        {
            return;
        }

        Cost.Text = _card.Cost.ToString();
        Icon.Texture = _card.Icon as Texture2D;
    }

    /// <summary>
    /// 动画补间对象，用于执行UI动画效果。
    /// </summary>
    public Tween Tween { private set; get; }

    /// <summary>
    /// 获取或设置当前控件的父级容器。
    /// </summary>
    public Control Parent { get; set; }

    /// <summary>
    /// ReparentRequested 信号委托。
    /// 当需要重新设置父级容器时触发此信号。
    /// </summary>
    /// <param name="cardUi">触发信号的 CardUi 实例。</param>
    [Signal]
    public delegate void ReparentRequestedEventHandler(CardUi cardUi);


    /// <summary>
    /// 执行从当前位置到指定坐标的缓动动画。
    /// 使用圆形缓出方式平滑过渡位置变化。
    /// </summary>
    /// <param name="newPosition">目标全局坐标位置。</param>
    /// <param name="duration">动画持续时间（单位：秒）。</param>
    public void AnimateToPosition(Vector2 newPosition, float duration)
    {
        Tween = CreateTween().SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.Out);
        Tween.TweenProperty(this, "global_position", newPosition, duration);
    }


    /// <summary>
    /// 当节点准备就绪时调用此方法。
    /// 初始化状态机并将其与当前卡片UI实例关联。
    /// </summary>
    public override void _Ready()
    {
        StateMachine.Init(this);
    }

    /// <summary>
    /// 处理输入事件的方法。
    /// 将输入事件传递给状态机进行处理。
    /// </summary>
    /// <param name="event">输入事件对象。</param>
    public override void _Input(InputEvent @event)
    {
        StateMachine.OnInput(@event);
    }

    /// <summary>
    /// 处理GUI输入事件的方法。
    /// 将GUI输入事件传递给状态机进行处理。
    /// </summary>
    /// <param name="event">GUI输入事件对象。</param>
    public void OnGuiInput(InputEvent @event)
    {
        StateMachine.OnGuiInput(@event);
    }

    /// <summary>
    /// 当鼠标进入控件区域时调用此方法。
    /// 通知状态机鼠标已进入控件区域。
    /// </summary>
    public void OnMouseEntered()
    {
        StateMachine.OnMouseEntered();
    }

    /// <summary>
    /// 当鼠标离开控件区域时调用此方法。
    /// 通知状态机鼠标已离开控件区域。
    /// </summary>
    public void OnMouseExited()
    {
        StateMachine.OnMouseExited();
    }

    /// <summary>
    /// 当有区域进入掉落点检测器时调用此方法。
    /// 若该区域尚未存在于目标列表中，则添加进目标列表。
    /// </summary>
    /// <param name="area2D">进入的区域对象。</param>
    public void OnDropPointDetectorAreaEntered(Area2D area2D)
    {
        if (!Targets.Contains(area2D))
        {
            Targets.Add(area2D);
        }
    }

    /// <summary>
    /// 当有区域离开掉落点检测器时调用此方法。
    /// 从目标列表中移除对应的区域对象。
    /// </summary>
    /// <param name="area2D">离开的区域对象。</param>
    public void OnDropPointDetectorAreaExited(Area2D area2D)
    {
        Targets.Remove(area2D);
    }
}
using Godot;
using SlayTheSpireLike.scripts.extensions;

namespace SlayTheSpireLike.scripts.relic_handler;

/// <summary>
/// 控制遗物界面分页显示的控件。
/// 负责处理左右翻页按钮、页面数量计算以及平滑滚动动画。
/// </summary>
public partial class RelicsControl : Control
{
    /// <summary>
    /// 每页显示的遗物数量，默认为5个。
    /// </summary>
    [Export] public int RelicsPerPage { get; set; } = 5;

    /// <summary>
    /// 页面切换时使用的缓动动画持续时间（秒），默认为0.2秒。
    /// </summary>
    [Export] public float TweenScrollDuration { get; set; } = 0.2f;

    /// <summary>
    /// 当前拥有的遗物总数。
    /// </summary>
    [Export] public int NumOfRelics { get; set; }

    /// <summary>
    /// 当前所在的页面编号，从1开始计数，默认为第一页。
    /// </summary>
    [Export] public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// 根据遗物总数和每页显示数量计算出的最大页面数。
    /// </summary>
    [Export] public int MaxPages { get; set; }

    /// <summary>
    /// 左侧翻页按钮引用。
    /// </summary>
    [Export] private TextureButton _leftButton;

    /// <summary>
    /// 右侧翻页按钮引用。
    /// </summary>
    [Export] private TextureButton _rightButton;

    /// <summary>
    /// 包含所有遗物节点的容器。
    /// </summary>
    [Export] private HBoxContainer _relics;

    /// <summary>
    /// 单个页面宽度，用于控制每次滚动的距离。
    /// </summary>
    private float _pageWidth;

    /// <summary>
    /// 当前正在执行的滚动动画Tween对象。
    /// </summary>
    private Tween _scrollTween;

    /// <summary>
    /// 初始化组件并绑定事件监听器。
    /// 绑定左右按钮点击事件及子节点顺序变化事件。
    /// </summary>
    public override void _Ready()
    {
        _pageWidth = CustomMinimumSize.X;
        _leftButton.Pressed += OnLeftButtonPressed;
        _rightButton.Pressed += OnRightButtonPressed;
        _relics.ChildOrderChanged += OnRelicsChildOrderChanged;
        foreach (var child in _relics.GetChildren())
        {
            child.FreeX();
        }
        OnRelicsChildOrderChanged();
    }

    /// <summary>
    /// 遗物列表子节点顺序发生变化时触发更新逻辑。
    /// </summary>
    private void OnRelicsChildOrderChanged()
    {
        Update();
    }

    /// <summary>
    /// 处理右侧翻页按钮按下事件。
    /// 若当前不是最后一页，则向右翻页并播放滚动动画。
    /// </summary>
    private void OnRightButtonPressed()
    {
        if (CurrentPage >= MaxPages) return;
        CurrentPage += 1;
        Update();
        TweenTo(_relics.Position.X - _pageWidth);
    }

    /// <summary>
    /// 处理左侧翻页按钮按下事件。
    /// 若当前不是第一页，则向左翻页并播放滚动动画。
    /// </summary>
    private void OnLeftButtonPressed()
    {
        if (CurrentPage <= 1) return;
        CurrentPage -= 1;
        Update();
        TweenTo(_relics.Position.X + _pageWidth);
    }

    /// <summary>
    /// 更新遗物数量与最大页码，并根据当前页设置按钮是否可交互。
    /// </summary>
    public void Update()
    {
        if (!IsInstanceValid(_leftButton) || !IsInstanceValid(_rightButton))
        {
            return;
        }
        NumOfRelics = _relics.GetChildCount();
        MaxPages = Mathf.CeilToInt(NumOfRelics / (float)RelicsPerPage);
        _leftButton.Disabled = CurrentPage <= 1;
        _rightButton.Disabled = CurrentPage >= MaxPages;
    }

    /// <summary>
    /// 执行横向位置缓动动画到指定X坐标。
    /// </summary>
    /// <param name="xPosition">目标X轴坐标</param>
    private void TweenTo(float xPosition)
    {
        // 停止之前的动画以避免冲突
        _scrollTween?.Kill();

        // 创建新的缓动动画实例并配置其过渡类型和缓动方式
        _scrollTween = CreateTween()
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);

        // 添加属性补间：将_relics的位置x值在指定时间内过渡到新值
        _scrollTween.TweenProperty(_relics, "position:x", xPosition, TweenScrollDuration);
    }
}

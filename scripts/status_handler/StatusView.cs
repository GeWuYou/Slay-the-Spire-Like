using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.extensions;
using SlayTheSpireLike.scripts.statuses;

namespace SlayTheSpireLike.scripts.status_handler;

/// <summary>
/// 状态视图控制器，用于显示和管理游戏中的各种状态效果
/// 负责状态信息的展示、用户交互处理以及视图的显示/隐藏控制
/// </summary>
public partial class StatusView : Control
{
    [Export]
    public VBoxContainer StatusTooltips { get; set; }
    [Export]
    public Button BackButton { get; set; }

    /// <summary>
    /// 初始化视图组件，设置事件监听器
    /// 在节点准备就绪时调用，清理子节点并绑定按钮和输入事件
    /// </summary>
    public override void _Ready()
    {
        foreach (var child in StatusTooltips.GetChildren())
            child?.QueueFreeX();
        // 绑定GUI输入事件处理器
        GuiInput+=OnGuiInput;
        // 绑定返回按钮点击事件
        BackButton.Pressed+=OnBackButtonPressed;
        Events.Instance.StatusTooltipRequested += ShowView;
    }

    public override void _ExitTree()
    {
       Events.Instance.StatusTooltipRequested -= ShowView;
    }

    /// <summary>
    /// 处理返回按钮点击事件
    /// 当用户点击返回按钮时隐藏当前视图
    /// </summary>
    private void OnBackButtonPressed()
    {
        HideView();
    }

    /// <summary>
    /// 处理GUI输入事件
    /// 监听用户按键操作，当按下取消键且视图可见时隐藏视图
    /// </summary>
    /// <param name="event">输入事件对象</param>
    private void OnGuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("left_mouse") && Visible)
        {
            HideView();
        }
    }

    /// <summary>
    /// 处理全局输入事件
    /// 监听用户按键操作，当按下取消键且视图可见时隐藏视图
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && Visible)
        {
            HideView();
        }
    }

    /// <summary>
    /// 显示状态视图
    /// 根据传入的状态列表创建对应的状态提示控件并显示整个视图
    /// </summary>
    /// <param name="statuses">要显示的状态效果数组</param>
    public void ShowView(Array<Status> statuses)
    {
        foreach (var child in StatusTooltips.GetChildren())
            child?.QueueFreeX();
        // 为每个状态创建对应的提示控件
        foreach (var status in statuses)
        {
            var newStatusTooltip = ResourceFactories.StatusTooltipFactory();
            StatusTooltips.AddChild(newStatusTooltip);
            newStatusTooltip.Status = status;
        }
        Show();
    }
    
    /// <summary>
    /// 隐藏状态视图
    /// 清理所有子节点并隐藏整个视图
    /// </summary>
    public void HideView()
    {
        // 清理所有子节点
        foreach (var child in StatusTooltips.GetChildren())
            child?.QueueFreeX();
        Hide();
    }
}

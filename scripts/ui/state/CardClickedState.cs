using DeckBuilderTutorial.scripts.extensions;
using Godot;

namespace DeckBuilderTutorial.scripts.ui.state;

/// <summary>
/// 卡牌点击状态类，处理卡牌被点击后的状态逻辑
/// 继承自CardState基类，负责管理卡牌在点击状态下的视觉效果和输入处理
/// </summary>
public partial class CardClickedState: CardState
{
    /// <summary>
    /// 进入点击状态时的初始化操作
    /// 设置卡牌UI的颜色为橙色，状态文本显示为"Clicked"
    /// 启用拖放检测器的监控功能
    /// </summary>
    public override void Enter()
    {
        // 设置卡牌背景颜色为橙色以表示选中状态
        CardUi.ColorRect.Color = Colors.Orange;
        // 更新状态文本显示
        CardUi.StateText.Text = "Clicked";
        // 启用拖放检测器
        CardUi.DropPointDetector.Monitoring = true;
    }

    /// <summary>
    /// 处理输入事件的方法
    /// 当检测到鼠标移动事件时，触发状态转换到拖拽状态
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public override void OnInput(InputEvent @event)
    {
        // 检测鼠标移动事件，用于触发拖拽状态转换
        if (@event is InputEventMouseMotion)
        {
            EmitSignal(CardState.SignalName.TransitionRequested, this, State.Dragging.GetCardStateValue());
        }
    }
}

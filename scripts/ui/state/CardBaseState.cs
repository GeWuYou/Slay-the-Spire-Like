using System;
using DeckBuilderTutorial.scripts.extensions;
using Godot;

namespace DeckBuilderTutorial.scripts.ui.state;

/// <summary>
/// 卡牌基础状态类，继承自CardState
/// 负责处理卡牌在基础状态下的行为和视觉表现
/// </summary>
public partial class CardBaseState : CardState
{
    /// <summary>
    /// 进入基础状态时执行的方法
    /// 初始化卡牌UI的显示状态，包括颜色、文本和位置
    /// </summary>
    public override async void Enter()
    {
        try
        {
            // 等待卡牌UI节点准备就绪
            if (!CardUi.IsNodeReady())
            {
                await ToSignal(CardUi, "ready");
            }

            if (CardUi.Tween!=null && CardUi.Tween.IsRunning())
            {
                CardUi.Tween.Kill();
            }
            
            // 请求重新设置父节点并更新UI状态显示
            CardUi.Panel.AddThemeStyleboxOverride("panel", CardUi.BaseStyleBox);
            CardUi.EmitSignal(CardUi.SignalName.ReparentRequested, CardUi);
            CardUi.PivotOffset = Vector2.Zero;
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
    }

    /// <summary>
    /// 处理GUI输入事件的方法
    /// 当检测到鼠标左键点击时触发状态转换
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public override void OnGuiInput(InputEvent @event)
    {
        // 只处理鼠标左键按下事件
        if (!@event.IsActionPressed("left_mouse"))
        {
            return;
        }
        
        // 设置卡牌的旋转中心点为鼠标点击位置
        CardUi.PivotOffset = CardUi.GetGlobalMousePosition() - CardUi.GlobalPosition;
        
        // 触发状态转换请求信号
        EmitSignal(CardState.SignalName.TransitionRequested, this, State.Clicked.GetCardStateValue());
    }

    public override void OnMouseEntered()
    {
        // 请求重新设置父节点并更新UI状态显示
        CardUi.Panel.AddThemeStyleboxOverride("panel", CardUi.HoverStyleBox);
    }
    public override void OnMouseExited()
    {
        // 恢复原始父节点并更新UI状态显示
        CardUi.Panel.AddThemeStyleboxOverride("panel", CardUi.BaseStyleBox);
    }
}
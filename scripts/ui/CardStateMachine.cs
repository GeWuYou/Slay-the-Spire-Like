using System.Collections.Generic;
using Godot;
using SlayTheSpireLike.scripts.extensions;
using SlayTheSpireLike.scripts.ui.state;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
///     卡牌状态机类，用于管理卡牌UI的不同状态转换
/// </summary>
public partial class CardStateMachine : Node
{
    private readonly Dictionary<CardState.State, CardState> _states = new();
    private CardState _currentState;
    [Export] private CardState _initialState;

    /// <summary>
    ///     初始化状态机，设置各个状态节点并建立状态映射关系
    /// </summary>
    /// <param name="cardUi">关联的卡牌UI组件</param>
    internal void Init(CardUi cardUi)
    {
        // 遍历所有子节点，初始化状态映射表
        foreach (var child in GetChildren())
        {
            if (child is not CardState cardState) continue;

            _states[cardState.StateValue] = cardState;
            cardState.TransitionRequested += OnTransitionRequested;
            cardState.CardUi = cardUi;
        }

        // 如果没有设置初始状态则直接返回
        if (_initialState == null) return;

        // 调用初始状态的进入方法
        _initialState.Enter();
        // 设置当前状态为初始状态
        _currentState = _initialState;
    }

    /// <summary>
    ///     处理输入事件，将事件传递给当前状态处理
    /// </summary>
    /// <param name="event">输入事件对象</param>
    public void OnInput(InputEvent @event)
    {
        _currentState?.OnInput(@event);
    }

    /// <summary>
    ///     处理GUI输入事件，将事件传递给当前状态处理
    /// </summary>
    /// <param name="event">GUI输入事件对象</param>
    public void OnGuiInput(InputEvent @event)
    {
        _currentState?.OnGuiInput(@event);
    }

    /// <summary>
    ///     处理鼠标进入事件，通知当前状态鼠标已进入
    /// </summary>
    public void OnMouseEntered()
    {
        _currentState?.OnMouseEntered();
    }

    /// <summary>
    ///     处理鼠标退出事件，通知当前状态鼠标已退出
    /// </summary>
    public void OnMouseExited()
    {
        _currentState?.OnMouseExited();
    }

    /// <summary>
    ///     处理状态转换请求，执行状态间的切换逻辑
    /// </summary>
    /// <param name="from">源状态</param>
    /// <param name="to">目标状态枚举值</param>
    private void OnTransitionRequested(CardState from, int to)
    {
        // 验证请求来源是否为当前状态
        if (from != _currentState) return;

        var newState = _states[to.GetCardState()];
        // 检查目标状态是否存在
        if (newState is null) return;

        // 执行状态退出和进入操作，并更新当前状态
        from.Exit();
        newState.Enter();
        _currentState = newState;
    }
}
using System;
using System.Collections.Generic;
using DeckBuilderTutorial.scripts.extensions;
using DeckBuilderTutorial.scripts.global;
using Godot;

namespace DeckBuilderTutorial.scripts.ui;

/// <summary>
/// CardTargetSelector 类用于处理卡牌目标选择的逻辑。
/// 它通过鼠标位置绘制弧线轨迹，并检测目标区域是否被选中。
/// </summary>
public partial class CardTargetSelector : Node2D
{
    /// <summary>
    /// 弧线上点的数量，用于绘制轨迹。
    /// </summary>
    [Export] public int ArcPoints { private set; get; } = 8;

    /// <summary>
    /// 用于检测目标区域的 Area2D 节点。
    /// </summary>
    [Export] public Area2D Area2D { private set; get; }

    /// <summary>
    /// 显示卡牌轨迹的 Line2D 节点。
    /// </summary>
    [Export] public Line2D CardArc { private set; get; }

    private CardUi _currentCard;
    private bool _targeting;
    private Events _events;

    /// <summary>
    /// 初始化节点，绑定事件监听器。
    /// </summary>
    public override void _Ready()
    {
        _events = this.Events();
        _events.CardAimingStarted += OnCardAimingStarted;
        _events.CardAimingEnded += OnCardAimingEnded;
        Area2D.AreaEntered += OnArea2DAreaEntered;
        Area2D.AreaExited += OnArea2DAreaExited;
    }

    /// <summary>
    /// 每帧更新逻辑，处理目标选择和轨迹绘制。
    /// </summary>
    /// <param name="delta">帧时间间隔</param>
    public override void _Process(double delta)
    {
        if (!_targeting)
        {
            return;
        }

        // 更新目标区域的位置为鼠标位置
        Area2D.Position = GetLocalMousePosition();

        // 更新轨迹线的点
        CardArc.Points = GetPoints().ToArray();
    }

    /// <summary>
    /// 计算并返回轨迹线上的点集合。
    /// </summary>
    /// <returns>表示轨迹的 Vector2 点列表</returns>
    private List<Vector2> GetPoints()
    {
         // 计算卡片中心点到鼠标位置的轨迹点集合
        // start: 卡片的中心点坐标
        // target: 鼠标在本地坐标系中的位置
        // distance: 从卡片中心点到鼠标位置的向量距离

        var points = new List<Vector2>();
        var start = _currentCard.GlobalPosition;
        // 从卡片顶部中央开始绘制箭头
        start.X += _currentCard.Size.X / 2;
        var target = GetLocalMousePosition();
        var distance = target - start;


        // 根据 EaseOutCubic 插值计算轨迹点
        // 生成缓动曲线上的点集合
        // 该循环通过EaseOutCubic缓动函数计算垂直坐标，水平坐标均匀分布
        for (var i = 0; i < ArcPoints; i++)
        {
            // 计算当前点在总点数中的比例
            var t = (1.0f / ArcPoints) * i;
            
            // 水平坐标：从起始点开始，按等间距递增
            var x = start.X + (distance.X / ArcPoints) * i;
            
            // 垂直坐标：从起始点开始，按EaseOutCubic缓动函数计算偏移量
            var y = start.Y + EaseOutCubic(t) * distance.Y;
            
            points.Add(new Vector2(x, y));
        }


        points.Add(target);
        return points;
    }

    /// <summary>
    /// EaseOutCubic 缓动函数，用于轨迹点的 Y 轴插值。
    /// </summary>
    /// <param name="number">插值参数</param>
    /// <returns>缓动后的浮点值</returns>
    private static float EaseOutCubic(float number)
    {
        return (float)(1.0f - Math.Pow(1.0 - number, 3));
    }

    /// <summary>
    /// 当卡牌瞄准开始时触发，重置目标选择状态。
    /// </summary>
    /// <param name="cardUi">当前卡牌 UI 实例</param>
    private void OnCardAimingStarted(CardUi cardUi)
    {
        if (!cardUi.Card.CardTarget.IsSingleTargeted())
        {
            return;
        }
        _targeting = true;
        Area2D.Monitoring = true;
        Area2D.Monitorable = true;
        _currentCard = cardUi;
    }
    
    /// <summary>
    /// 当卡牌瞄准结束时触发，开始目标选择逻辑。
    /// </summary>
    /// <param name="cardUi">当前卡牌 UI 实例</param>
    private void OnCardAimingEnded(CardUi cardUi)
    {
        if (!cardUi.Card.CardTarget.IsSingleTargeted())
        {
            return;
        }
        _targeting = false;
        CardArc.ClearPoints();
        Area2D.Position = Vector2.Zero;
        Area2D.Monitoring = false;
        Area2D.Monitorable = false;
        _currentCard = null;
    }



    /// <summary>
    /// 当目标区域进入检测范围时触发，添加目标到卡牌目标列表。
    /// </summary>
    /// <param name="area2D">进入检测区域的 Area2D 实例</param>
    private void OnArea2DAreaEntered(Area2D area2D)
    {
        if (_currentCard == null || !_targeting)
        {
            return;
        }

        if (!_currentCard.Targets.Contains(area2D))
        {
            _currentCard.Targets.Add(area2D);
        }
    }

    /// <summary>
    /// 当目标区域离开检测范围时触发，从卡牌目标列表中移除。
    /// </summary>
    /// <param name="area2D">离开检测区域的 Area2D 实例</param>
    private void OnArea2DAreaExited(Area2D area2D)
    {
        if (_currentCard == null || !_targeting)
        {
            return;
        }

        _currentCard.Targets.Remove(area2D);
    }
}

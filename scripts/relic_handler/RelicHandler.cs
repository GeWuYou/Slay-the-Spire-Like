using System.Linq;
using SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.enums;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.relic_handler;

/// <summary>
/// 处理遗物（Relic）逻辑与UI显示的核心类。负责管理遗物的添加、激活以及相关事件处理。
/// </summary>
public partial class RelicHandler : HBoxContainer
{
    /// <summary>
    /// 当某种类型的遗物被激活完成后触发的信号。
    /// </summary>
    /// <param name="type">已激活完成的遗物类型。</param>
    [Signal]
    public delegate void RelicsActivatedEventHandler(RelicType type);

    /// <summary>
    /// 遗物激活之间的间隔时间（秒），用于动画或延迟效果控制。
    /// </summary>
    private float RelicApplyInterval { get; set; } = 0.5f;

    /// <summary>
    /// 控制遗物行为的对象引用。
    /// </summary>
    [Export] private RelicsControl _relicsControl;

    /// <summary>
    /// 包含所有遗物UI节点的容器。
    /// </summary>
    [Export] private HBoxContainer _relics;

    /// <summary>
    /// 初始化方法，在节点加载时调用。
    /// 注册子节点退出树时的回调函数以进行清理操作。
    /// </summary>
    public override void _Ready()
    {
        _relics.ChildExitingTree += OnRelicExitingTree;
    }

    /// <summary>
    /// 在遗物UI节点从场景树中移除前调用此方法，用于反初始化该遗物。
    /// </summary>
    /// <param name="node">即将被移除的节点。</param>
    private void OnRelicExitingTree(Node node)
    {
        // 检查是否是有效的遗物UI节点，并且其关联了具体的遗物对象
        if (node is not RelicUi relicUi || relicUi.Relic is null)
        {
            return;
        }
        relicUi.Relic.DeactivateRelic(relicUi);
    }

    /// <summary>
    /// 根据指定类型批量激活当前拥有的对应类型的遗物。
    /// 使用Tween实现依次执行并带有间隔的效果。
    /// </summary>
    /// <param name="type">要激活的遗物类型。</param>
    public void ActivateRelicsByType(RelicType type)
    {
        // EventBased 类型不在此统一处理
        if (type == RelicType.EventBased)
        {
            return;
        }

        // 获取所有匹配类型的遗物UI节点
        var relicQueue = GetAllRelicUiNodes().Where(ui => ui.Relic.Type == type).ToArray();
        
        // 如果没有符合条件的遗物，则直接发出信号表示已完成
        if (relicQueue.Length == 0)
        {
            EmitSignal(SignalName.RelicsActivated, (int)type);
            return;
        }

        // 创建一个Tween来顺序执行每个遗物的激活动作
        var tween = CreateTween();
        foreach (var relicUi in relicQueue)
        {
            tween.TweenCallback(Callable.From(() => relicUi.Relic.ActivateRelic(relicUi)));
            tween.TweenInterval(RelicApplyInterval);
        }

        // 所有遗物激活完毕后发送信号通知其他系统
        tween.Finished += () => EmitSignal(SignalName.RelicsActivated, (int)type);
    }

    /// <summary>
    /// 获取所有属于 RelicUi 类型的子节点集合。
    /// </summary>
    /// <returns>包含所有 RelicUi 节点的数组。</returns>
    private Array<RelicUi> GetAllRelicUiNodes()
    {
        return new Array<RelicUi>(_relics
            .GetChildren()
            .OfType<RelicUi>().ToArray());
    }

    /// <summary>
    /// 将一组遗物添加到界面中。
    /// </summary>
    /// <param name="relicsArray">需要添加的一组遗物实例。</param>
    public void AddRelics(Array<Relic> relicsArray)
    {
        foreach (var relic in relicsArray)
        {
            AddRelic(relic);
        }
    }

    /// <summary>
    /// 添加单个遗物到界面上。
    /// 若已有相同ID的遗物则跳过添加。
    /// </summary>
    /// <param name="relic">待添加的遗物实例。</param>
    public void AddRelic(Relic relic)
    {
        // 判断是否已经存在相同的遗物
        if (HasRelic(relic.Id))
        {
            return;
        }

        // 实例化一个新的遗物UI控件并设置数据
        var newRelicUi = ResourceFactories.RelicUiFactory();
        _relics.AddChild(newRelicUi);
        newRelicUi.Relic = relic;
        newRelicUi.Relic.InitializeRelic(newRelicUi);
    }

    /// <summary>
    /// 检查是否存在具有特定ID的遗物。
    /// </summary>
    /// <param name="relicId">要检查的遗物唯一标识符。</param>
    /// <returns>如果存在返回true，否则false。</returns>
    private bool HasRelic(string relicId)
    {
        foreach (var child in _relics.GetChildren())
        {
            if (child is not RelicUi relicUi)
            {
                continue;
            }
            if (relicUi.Relic.Id == relicId && IsInstanceValid(relicUi))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取当前所有的遗物对象列表。
    /// </summary>
    /// <returns>包含所有遗物对象的数组。</returns>
    public Array<Relic> GetAllRelics()
    {
        return new Array<Relic>(GetAllRelicUiNodes().Select(relicUi => relicUi.Relic).ToArray());
    }
}

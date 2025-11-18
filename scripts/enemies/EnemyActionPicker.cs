using Godot;
using SlayTheSpireLike.scripts.extensions;

namespace SlayTheSpireLike.scripts.enemies;

/// <summary>
///     敌人行为选择器节点。用于根据条件或概率从子节点中选取一个敌人动作执行。
/// </summary>
public partial class EnemyActionPicker : Node
{
    private Enemy _enemy;
    private Node2D _target;

    private float _totalWeight;

    /// <summary>
    ///     关联的敌人对象。设置时会同步更新所有子动作的 Enemy 属性。
    /// </summary>
    [Export]
    public Enemy Enemy
    {
        get => _enemy;
        set
        {
            _enemy = value;
            foreach (var child in GetChildren())
                if (child is EnemyAction action)
                    action.Enemy = Enemy;
        }
    }

    /// <summary>
    ///     动作的目标节点（通常是玩家）。设置时会同步更新所有子动作的 Target 属性。
    /// </summary>
    [Export]
    public Node2D Target
    {
        get => _target;
        set
        {
            _target = value;
            foreach (var child in GetChildren())
                if (child is EnemyAction action)
                    action.Target = Target;
        }
    }

    /// <summary>
    ///     初始化方法，在节点准备就绪时调用。
    ///     默认将目标设为场景中的第一个 player 组节点，并初始化各动作的概率权重。
    /// </summary>
    public override void _Ready()
    {
        Target = GetTree().GetFirstNodeInGroup("player") as Node2D;
        SetupChances();
    }

    /// <summary>
    ///     遍历所有子节点并计算基于概率的动作总权重及累积权重。
    ///     只处理标记为 ChanceBased 类型的动作。
    /// </summary>
    private void SetupChances()
    {
        foreach (var child in GetChildren())
        {
            if (child is not EnemyAction action || !action.ActionType.IsChanceBasedType()) continue;
            _totalWeight += action.ChanceWeight;
            action.AccumulatedWeight = _totalWeight;
        }
    }

    /// <summary>
    ///     获取要执行的动作：优先尝试获取满足条件的第一个条件性动作，
    ///     若无符合条件的动作则按概率随机选择一个基于概率的动作。
    /// </summary>
    /// <returns>选中的敌人动作实例；若未找到有效动作则返回 null。</returns>
    public EnemyAction GetAction()
    {
        var action = GetFirstConditionalAction();
        return action ?? GetChanceBasedAction();
    }

    /// <summary>
    ///     按照顺序查找第一个可执行的条件性动作。
    ///     条件性动作必须通过 IsPerformable 方法判断是否可以执行。
    /// </summary>
    /// <returns>第一个满足条件且可执行的动作；如果没有则返回 null。</returns>
    public EnemyAction GetFirstConditionalAction()
    {
        foreach (var child in GetChildren())
            if (child is EnemyAction action && action.ActionType.IsConditionalType() && action.IsPerformable())
                return action;

        return null;
    }

    /// <summary>
    ///     根据预设的概率权重随机选择一个基于概率的动作。
    ///     使用累计权重与随机数比较的方式实现加权随机抽取。
    /// </summary>
    /// <returns>被选中的概率型动作；如果未能匹配到任何动作则输出错误信息并返回 null。</returns>
    private EnemyAction GetChanceBasedAction()
    {
        // 在 [0, totalWeight) 范围内生成一个随机浮点数作为判定值
        var roll = GD.RandRange(0.0f, _totalWeight);

        // 遍历所有子节点寻找符合权重区间的动作
        foreach (var child in GetChildren())
            if (child is EnemyAction action && action.ActionType.IsChanceBasedType() && action.AccumulatedWeight > roll)
                return action;

        // 找不到合适动作时记录错误日志
        GD.PrintErr($"没有找到概率动作，权重和为{_totalWeight}");
        return null;
    }
}
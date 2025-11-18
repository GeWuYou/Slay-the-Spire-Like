using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.enemies;

/// <summary>
///     敌人动作基类，定义了敌人可执行动作的基本结构和行为
///     提供两种动作类型：条件驱动型(Conditional)和概率驱动型(ChanceBased)
///     支持动作的可执行性判断和具体执行逻辑的实现
/// </summary>
public partial class EnemyAction : Node
{
    #region 类型定义

    /// <summary>
    ///     敌人动作的选择机制类型枚举
    ///     控制动作是如何被选中执行的
    /// </summary>
    public enum Type
    {
        /// <summary>
        ///     条件驱动型动作
        ///     只有当特定条件满足时才会被执行
        /// </summary>
        Conditional,

        /// <summary>
        ///     概率驱动型动作
        ///     根据设定的权重值按概率随机选择
        /// </summary>
        ChanceBased
    }

    #endregion

    /// <summary>
    ///     意图，描述动作的意图和效果
    /// </summary>
    [Export]
    public Intent Intent { get; set; }
    
    /// <summary>
    ///     动作对应的音频流，用于播放动作的音效
    /// </summary>
    [Export]
    public AudioStream Sound { get; set; }

    /// <summary>
    ///     动作类型枚举，决定动作的选择策略
    ///     Conditional: 基于特定条件是否满足来决定是否执行
    ///     ChanceBased: 基于权重概率系统来决定是否被选中
    /// </summary>
    [Export]
    public Type ActionType { get; set; }

    /// <summary>
    ///     概率权重值，仅在Type为ChanceBased时生效
    ///     数值越高被选中的概率越大，范围限制在0.0到10.0之间
    /// </summary>
    [Export(PropertyHint.Range, "0.0,10.0")]
    public float ChanceWeight { get; set; }

    /// <summary>
    ///     在概率计算过程中使用的累积权重值
    ///     存储此动作之前所有动作的权重总和，用于构建权重区间
    /// </summary>
    public float AccumulatedWeight { get; set; }

    /// <summary>
    ///     执行此动作的敌方单位引用
    ///     用于访问敌人的属性和方法，如攻击力、生命值等
    /// </summary>
    public Enemy Enemy { get; set; }

    /// <summary>
    ///     动作的目标对象引用，通常指向玩家角色
    ///     用于获取目标位置、状态等信息，支持面向目标的动作逻辑
    /// </summary>
    public Node2D Target { get; set; }

    /// <summary>
    ///     判断当前动作是否可以执行
    ///     对于Conditional类型，检查执行条件是否满足
    ///     对于ChanceBased类型，此方法通常返回true，实际选择由概率决定
    /// </summary>
    /// <returns>动作可执行返回true，否则返回false</returns>
    public virtual bool IsPerformable()
    {
        return false;
    }

    /// <summary>
    ///     执行具体的动作逻辑
    ///     根据不同类型的动作实现相应的游戏行为，如攻击、防御、施法等
    /// </summary>
    public virtual void PerformAction()
    {
    }
}
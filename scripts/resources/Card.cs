using System;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// 表示一张游戏中的卡牌资源，继承自Godot的Resource类。
/// 包含卡牌的基本属性如ID、名称、描述、类型、目标、费用以及图标等。
/// 提供获取目标节点和播放卡牌的方法。
/// </summary>
[GlobalClass]
public partial class Card : Resource
{

    [ExportGroup("卡牌属性")]
    [Export(PropertyHint.None, "卡牌的唯一标识符")]
    public string Id { private set; get; }

    [ExportGroup("卡牌属性")]
    [Export(PropertyHint.None, "卡牌的名称")]
    public string Name { private set; get; }

    [ExportGroup("卡牌属性")]
    [Export(PropertyHint.MultilineText, "卡牌的描述信息")]
    public string Description { private set; get; }

    [ExportGroup("卡牌属性")]
    [Export(PropertyHint.None, "卡牌的类型（攻击、能力、技能等）")]
    public Type CardType { private set; get; }

    [ExportGroup("卡牌属性")]
    [Export(PropertyHint.None, "卡牌的目标类型（自身、敌人、全体等）")]
    public Target CardTarget { private set; get; }

    [ExportGroup("卡牌属性")]
    [Export(PropertyHint.None, "卡牌的初始费用")]
    public int InitCost { private set; get; }

    [ExportGroup("卡牌属性")]
    [Export(PropertyHint.None, "卡牌的当前费用")]
    public int Cost { private set; get; }

    [ExportGroup("卡牌视觉效果")] [Export] public Texture Icon { get; private set; }

    /// <summary>
    /// 卡片类型枚举，定义了游戏中不同类型的卡片
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// 攻击类型卡片，通常用于对敌人造成伤害
        /// </summary>
        Attack,

        /// <summary>
        /// 能力类型卡片，通常提供持续性的效果或增强
        /// </summary>
        Power,

        /// <summary>
        /// 技能类型卡片，通常是具有特殊效果的一次性卡片
        /// </summary>
        Skill,

        /// <summary>
        /// 状态类型卡片，通常影响玩家或敌人的状态
        /// </summary>
        State,

        /// <summary>
        /// 诅咒类型卡片，通常对玩家产生负面效果
        /// </summary>
        Curse
    }

    /// <summary>
    /// 目标类型枚举，定义了卡片效果可应用的不同目标
    /// </summary>
    public enum Target
    {
        /// <summary>
        /// 自身目标，卡片效果作用于使用者自己
        /// </summary>
        Self,

        /// <summary>
        /// 敌人目标，卡片效果作用于单个敌人
        /// </summary>
        Enemy,

        /// <summary>
        /// 全体目标，卡片效果作用于所有角色
        /// </summary>
        All,

        /// <summary>
        /// 随机目标，卡片效果作用于随机选择的目标
        /// </summary>
        Random,

        /// <summary>
        /// 随机敌人目标，卡片效果作用于随机选择的敌人
        /// </summary>
        RandomEnemy,

        /// <summary>
        /// 随机盟友目标，卡片效果作用于随机选择的盟友
        /// </summary>
        RandomAlly,

        /// <summary>
        /// 所有敌人目标，卡片效果作用于所有敌人
        /// </summary>
        AllEnemies,

        /// <summary>
        /// 盟友目标，卡片效果作用于指定盟友
        /// </summary>
        Ally,
        
        /// <summary>
        /// 所有盟友目标，卡片效果作用于所有盟友
        /// </summary>
        AllAllies,
        
        /// <summary>
        /// 所有角色目标，卡片效果作用于所有盟友和玩家
        /// </summary>
        AllSelf,
        
        /// <summary>
        /// 随机己方目标，卡片效果作用于随机选择的己方角色
        /// </summary>
        RandomSelf,
    }

    /// <summary>
    /// 重写ToString方法，返回卡牌的完整信息字符串
    /// </summary>
    /// <returns>包含卡牌所有属性的详细字符串</returns>
    public override string ToString()
    {
        return $"Card(Id: {Id}, Name: {Name}, Description: {Description}, Type: {CardType}, Target: {CardTarget})";
    }

    /// <summary>
    /// 根据卡片目标类型获取实际的目标节点集合
    /// </summary>
    /// <param name="targets">原始目标节点数组，用于Enemy和Ally类型的目标选择</param>
    /// <returns>根据CardTarget类型筛选后的目标节点数组</returns>
    public Array<Node> GetTargets(Array<Node> targets)
    {
        // 检查输入目标数组是否为空或无元素
        if (targets is null || targets.Count == 0)
        {
            return [];
        }

        var tree = targets[0].GetTree();
        
        // 根据不同的目标类型返回相应的节点集合
        switch (CardTarget)
        {
            case Target.Self:
                // 返回玩家组的所有节点
                return tree.GetNodesInGroup(GameConstants.Groups.Player);
            case Target.AllEnemies:
                // 返回敌人组的所有节点
                return tree.GetNodesInGroup(GameConstants.Groups.Enemies);
            case Target.All:
                // 返回玩家组和敌人组的所有节点
                return tree.GetNodesInGroup(GameConstants.Groups.Player) 
                       + tree.GetNodesInGroup(GameConstants.Groups.Enemies)
                       // todo 后续加入盟友
                       // + tree.GetNodesInGroup(GameConstants.Groups.Allies)
                       ;
            case Target.Enemy:
                // 返回传入的目标节点
                return targets;
            case Target.Random:
                // 从所有角色中随机选择一个目标
                var allTargets = tree.GetNodesInGroup(GameConstants.Groups.Player) + tree.GetNodesInGroup(GameConstants.Groups.Enemies);
                return [allTargets[new Random().Next(0, allTargets.Count)]];
            case Target.RandomEnemy:
                // 从敌人中随机选择一个目标
                var enemyTargets = tree.GetNodesInGroup(GameConstants.Groups.Enemies);
                return [enemyTargets[new Random().Next(0, enemyTargets.Count)]];
            case Target.RandomAlly:
                // 从盟友中随机选择一个目标
                var allyTargets = tree.GetNodesInGroup(GameConstants.Groups.Player);
                return [allyTargets[new Random().Next(0, allyTargets.Count)]];
            case Target.Ally:
                // 返回传入的目标节点
                return targets;
            case Target.AllAllies:
                // 返回所有盟友节点
                return tree.GetNodesInGroup(GameConstants.Groups.Allies);
            case Target.AllSelf:
                // 返回所有盟友和己方
                return tree.GetNodesInGroup(GameConstants.Groups.Player) + tree.GetNodesInGroup(GameConstants.Groups.Allies);
            case Target.RandomSelf:
                // 从所有盟友和己方中随机选择一个目标
                var selfTargets = tree.GetNodesInGroup(GameConstants.Groups.Player) + tree.GetNodesInGroup(GameConstants.Groups.Allies);
                return [selfTargets[new Random().Next(0, selfTargets.Count)]];
            default:
                // 默认情况下返回空数组
                return  [];
        }
    }

    /// <summary>
    /// 播放该卡牌并触发相关事件与效果
    /// </summary>
    /// <param name="targets">原始目标节点列表</param>
    /// <param name="stats">角色统计数据对象，用于扣除法力值</param>
    public void Play(Array<Node> targets, CharacterStats stats)
    {
        Events.Instance.EmitSignal(SlayTheSpireLike.scripts.global.Events.SignalName.CardPlayed, this);
        stats.Mana -= Cost;
        ApplyEffects(GetTargets(targets));
    }

    /// <summary>
    /// 应用卡牌的效果到指定的目标上。子类应重写此方法以实现具体逻辑。
    /// </summary>
    /// <param name="targets">经过处理后的真实目标节点列表</param>
    protected virtual void ApplyEffects(Array<Node> targets)
    {
        
    }
}

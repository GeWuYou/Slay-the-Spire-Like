using Godot;

namespace DeckBuilderTutorial.scripts.resources;

public partial class Card : Resource
{
    [ExportGroup("卡牌属性")] [Export] public string Id { private set; get; }
    [ExportGroup("卡牌属性")] [Export] public string Name { private set; get; }
    [ExportGroup("卡牌属性")] [Export] public string Description { private set; get; }
    [ExportGroup("卡牌属性")] [Export] public Type CardType { private set; get; }
    [ExportGroup("卡牌属性")] [Export] public Target CardTarget { private set; get; }
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
        RandomAlly
    }
}
using DeckBuilderTutorial.scripts.effects;
using DeckBuilderTutorial.scripts.resources;
using Godot;
using Godot.Collections;

namespace DeckBuilderTutorial.scripts.player.warrior.cards;

/// <summary>
/// 战士攻击卡牌类，继承自Card基类
/// 代表战士职业的一种攻击卡牌，用于对目标造成伤害
/// </summary>
public partial class WarriorAttack : Card
{
    /// <summary>
    /// 应用卡牌效果到指定目标
    /// 重写父类方法，创建并执行伤害效果
    /// </summary>
    /// <param name="targets">目标节点数组，包含所有需要应用效果的目标</param>
    protected override void ApplyEffects(Array<Node> targets)
    {
        // 创建伤害效果实例
        var effect = new DamageEffect();
        effect.Amount = 6;
        // 执行伤害效果
        effect.Execute(targets);
    }
}

using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.modifier_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.player.warrior.cards;

/// <summary>
///     战士攻击卡牌类，继承自Card基类
///     代表战士职业的一种攻击卡牌，用于对目标造成伤害
/// </summary>
public partial class WarriorAttack : Card
{
    [Export] public int BaseDamage { get; set; } = 6;
    /// <summary>
    ///     应用卡牌的效果到指定的目标上。子类应重写此方法以实现具体逻辑。
    /// </summary>
    /// <param name="targets">经过处理后的真实目标节点列表</param>
    /// <param name="modifierHandler">修饰符处理器，用于处理效果应用过程中的修饰符</param>
    protected override void ApplyEffects(Array<Node> targets, ModifierHandler modifierHandler)
    {
        // 创建伤害效果实例
        var effect = new DamageEffect();
        effect.Amount = modifierHandler.GetModifiedValue(Modifier.ModifierType.DmgDealt,BaseDamage);
        effect.Sound = Sound;
        // 执行伤害效果
        effect.Execute(targets);
    }

    public override string GetDefaultDescription()
    {
        return string.Format(base.GetDefaultDescription(), BaseDamage);
    }

    public override string GetDescription(ModifierHandler playerModifierHandler, ModifierHandler enemyModifierHandler)
    {
        var modifiedDamage = playerModifierHandler.GetModifiedValue(Modifier.ModifierType.DmgDealt, BaseDamage);
        if (enemyModifierHandler is not null)
        {
            modifiedDamage = enemyModifierHandler.GetModifiedValue(Modifier.ModifierType.DmgTaken, modifiedDamage);
        }
        return string.Format(GetDefaultDescription(),modifiedDamage);
    }
}
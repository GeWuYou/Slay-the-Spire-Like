using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.modifier_handler;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.statuses;
using Array = Godot.Collections.Array;

namespace SlayTheSpireLike.scripts.player.warrior.cards;

/// <summary>
/// 卡牌逻辑模板类，用于定义卡牌的具体效果和行为
/// </summary>
public partial class WarriorBigSlam : Card
{
    /// <summary>
    /// 可选的声音列表，用于存储卡牌播放时可能用到的音效资源
    /// </summary>
    [Export]
    public Array OptionalSoundList { get; set; }

    [Export] public int BaseDamage { get; set; } = 14;
    [Export] public int Ratio { get; set; } = 2;

    /// <summary>
    ///     应用卡牌的效果到指定的目标上。子类应重写此方法以实现具体逻辑。
    /// </summary>
    /// <param name="targets">经过处理后的真实目标节点列表</param>
    /// <param name="modifierHandler">修饰符处理器，用于处理效果应用过程中的修饰符</param>
    protected override void ApplyEffects(Array<Node> targets, ModifierHandler modifierHandler)
    {
        // 创建伤害效果实例
        var effect = new DamageEffect();
        // 计算伤害值，包括基础伤害和双倍肌肉状态加成
        effect.Amount = CalculateDamage(modifierHandler);
        effect.Sound = Sound;
        // 执行伤害效果
        effect.Execute(targets);
    }
    public override string GetDefaultDescription()
    {
        return string.Format(base.GetDefaultDescription(), BaseDamage);
    }

    /// <summary>
    /// 计算卡牌的伤害值，包括基础伤害和双倍肌肉状态加成
    /// </summary>
    /// <param name="modifierHandler">修饰符处理器</param>
    /// <returns>计算后的伤害值</returns>
    private int CalculateDamage(ModifierHandler modifierHandler)
    {
        // 获取基础伤害值
        var damage = BaseDamage;
        
        // 检查是否有肌肉状态，并应用双倍加成
        var muscleModifier = modifierHandler.GetModifier(Modifier.ModifierType.DmgDealt);
        var muscleValue = muscleModifier?.GetValue(MuscleStatus.Muscle);
        if (muscleValue != null)
        {
            // 应用双倍肌肉状态加成
            damage += muscleValue.FlatValue * Ratio;
        }

        return damage;
    }
    
    public override string GetDescription(ModifierHandler playerModifierHandler, ModifierHandler enemyModifierHandler)
    {
        var damage = CalculateDamage(playerModifierHandler);
        
        // 应用敌人承受伤害的修饰符（如果有）
        if (enemyModifierHandler is not null)
        {
            damage = enemyModifierHandler.GetModifiedValue(Modifier.ModifierType.DmgTaken, damage);
        }
        
        return string.Format(base.GetDefaultDescription(), damage);
    }
}
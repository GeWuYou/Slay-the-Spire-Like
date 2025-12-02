using System;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.modifier_handler;
using SlayTheSpireLike.scripts.resources;
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
    [Export] public int ExposedDuration { get; set; } = 2;

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
}
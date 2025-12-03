using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.modifier_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.player.warrior.cards;

/// <summary>
///     战士防御卡牌类，继承自Card基类
///     该卡牌用于执行防御效果，为玩家提供格挡值
/// </summary>
public partial class WarriorDefend : Card
{
    [Export] public int BaseBlock { get; set; } = 6;
    /// <summary>
    ///     应用卡牌的效果到指定的目标上。子类应重写此方法以实现具体逻辑。
    /// </summary>
    /// <param name="targets">经过处理后的真实目标节点列表</param>
    /// <param name="modifierHandler">修饰符处理器，用于处理效果应用过程中的修饰符</param>
    protected override void ApplyEffects(Array<Node> targets, ModifierHandler modifierHandler)
    {
        // 创建格挡效果实例
        var effect = new BlockEffect();
        effect.Amount = BaseBlock;
        effect.Sound = Sound;
        // 执行格挡效果
        effect.Execute(targets);
    }
    public override string GetDefaultDescription()
    {
        return string.Format(base.GetDefaultDescription(),BaseBlock);
    }

    public override string GetDescription(ModifierHandler playerModifierHandler, ModifierHandler enemyModifierHandler)
    {
        return string.Format(GetDefaultDescription(),playerModifierHandler.GetModifiedValue(Modifier.ModifierType.Block,BaseBlock));
    }
}
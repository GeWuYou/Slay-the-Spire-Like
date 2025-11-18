using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.player.warrior.cards;

/// <summary>
///     战士防御卡牌类，继承自Card基类
///     该卡牌用于执行防御效果，为玩家提供格挡值
/// </summary>
public partial class WarriorDefend : Card
{
    /// <summary>
    ///     应用卡牌效果到指定目标
    ///     创建并执行一个格挡效果，为目标增加6点格挡值
    /// </summary>
    /// <param name="targets">效果作用的目标节点数组</param>
    protected override void ApplyEffects(Array<Node> targets)
    {
        // 创建格挡效果实例
        var effect = new BlockEffect();
        effect.Amount = 6;
        effect.Sound = Sound;
        // 执行格挡效果
        effect.Execute(targets);
    }
}
using DeckBuilderTutorial.scripts.enemies;
using DeckBuilderTutorial.scripts.player;
using Godot;
using Godot.Collections;

namespace DeckBuilderTutorial.scripts.effects;

/// <summary>
/// 伤害效果类，用于对目标造成指定数值的伤害
/// 继承自Effect基类，实现具体的伤害逻辑
/// </summary>
public partial class DamageEffect : Effect
{
    /// <summary>
    /// 伤害数值属性，通过导出可在编辑器中设置
    /// </summary>
    [Export] public int Amount { get; set; }

    /// <summary>
    /// 执行伤害效果的核心方法
    /// 遍历所有目标节点，根据目标类型调用相应的受伤方法
    /// </summary>
    /// <param name="targets">目标节点数组，包含需要受到伤害的所有目标</param>
    public override void Execute(Array<Node> targets)
    {
        // 遍历所有目标并造成伤害
        foreach (var target in targets)
        {
            switch (target)
            {
                // 跳过空目标
                case null:
                    continue;
                // 对敌人目标造成伤害
                case Enemy enemy:
                    enemy.TakeDamage(Amount);
                    break;
                // 对玩家目标造成伤害
                case Player player:
                    player.TakeDamage(Amount);
                    break;
            }
        }
    }
}

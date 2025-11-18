using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.component;

namespace SlayTheSpireLike.scripts.effects;

/// <summary>
///     BlockEffect类用于实现格挡效果，可以为目标增加格挡值
/// </summary>
public partial class BlockEffect : Effect
{
    [Export] public int Amount { get; set; }

    /// <summary>
    ///     执行格挡效果，为目标增加指定数量的格挡值
    /// </summary>
    /// <param name="targets">效果作用的目标节点数组，可以包含敌人或玩家</param>
    public override void Execute(Array<Node> targets)
    {
        // 遍历所有目标并增加格挡值
        foreach (var target in targets)
        {
            if (target is not IBlockableComponent blockableComponent)
            {
                continue;
            }
            blockableComponent.TakeBlock(Amount);
            AudioPlayerManager.Instance.PlaySfx(Sound);
        }
           
    }
}
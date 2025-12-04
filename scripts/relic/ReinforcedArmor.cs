using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.modifier_handler;
using SlayTheSpireLike.scripts.player;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.relic;

public partial class ReinforcedArmor : Relic
{
    [Export] public int 格挡值 { get; set; } = 6;

    [Export] private AudioStream _sound;


    /// <summary>
    /// 激活遗物UI效果
    /// </summary>
    /// <param name="relicUi">需要激活的遗物UI组件</param>
    public override void ActivateRelic(RelicUi relicUi)
    {
        var node = relicUi.GetTree().GetFirstNodeInGroup(GameConstants.Groups.Player);
        // 获取场景中的玩家节点
        if (node is not Player player)
        {
            return;
        }

        // 创建格挡效果实例
        var effect = new BlockEffect();
        player.ModifierHandler.GetModifier(Modifier.ModifierType.Block);
        if (player.Stats.Block != 0)
        {
            return;
        }
        relicUi.Flash();
        effect.Amount = player.ModifierHandler.GetModifiedValue(Modifier.ModifierType.Block, 格挡值);
        effect.Sound = _sound;
        // 执行格挡效果
        effect.Execute([node]);
    }


    /// <summary>
    /// 获取遗物的提示文本信息
    /// </summary>
    /// <returns>返回当前遗物的提示文本字符串</returns>
    public override string GetTooltip()
    {
        return string.Format(Tooltip, 格挡值);
    }
}
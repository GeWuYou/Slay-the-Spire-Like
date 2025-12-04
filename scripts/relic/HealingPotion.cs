using Godot;
using SlayTheSpireLike.scripts.global;
using SlayTheSpireLike.scripts.player;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.relic;

/// <summary>
/// 治疗药水遗物类，继承自Relic基类
/// </summary>
public partial class HealingPotion : Relic
{
	/// <summary>
	/// 恢复的血量数量
	/// </summary>
	[Export] public int HealAmount { get; set; } = 6;
	/// <summary>
	/// 激活遗物UI效果，为玩家恢复6点生命值并显示闪光效果
	/// </summary>
	/// <param name="relicUi">需要激活的遗物UI组件</param>
	public override void ActivateRelic(RelicUi relicUi)
	{
		// 获取场景中的玩家节点
		if (relicUi.GetTree().GetFirstNodeInGroup(GameConstants.Groups.Player) is not Player player)
		{
			return;
		}
		// 为玩家恢复6点生命值并显示遗物闪光效果
		player.Stats.Heal(HealAmount);
		relicUi.Flash();
	}
}

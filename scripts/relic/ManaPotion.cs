using SlayTheSpireLike.scripts.global;
using SlayTheSpireLike.scripts.player;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.relic;

/// <summary>
/// 法力药水遗物类，继承自Relic基类
/// </summary>
public partial class ManaPotion : Relic
{

	private bool _hasBeenActivated;
	/// <summary>
	/// 激活遗物效果的方法
	/// </summary>
	/// <param name="relicUi">遗物UI组件实例</param>
	public override void ActivateRelic(RelicUi relicUi)
	{
		// 获取场景中的玩家节点
		if (relicUi.GetTree().GetFirstNodeInGroup(GameConstants.Groups.Player) is not Player player)
		{
			return;
		}

		if (_hasBeenActivated)
		{
			return;
		}
		relicUi.Flash();
		player.Stats.MaxMana += 1;
		_hasBeenActivated = true;

	}


	/// <summary>
	/// 获取遗物的提示文本信息
	/// </summary>
	/// <returns>返回当前遗物的提示文本字符串</returns>
	public override string GetTooltip()
	{
		return Tooltip;
	}
}


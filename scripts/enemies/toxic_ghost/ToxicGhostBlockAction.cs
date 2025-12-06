using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.modifier_handler;

namespace SlayTheSpireLike.scripts.enemies.toxic_ghost;

public partial class ToxicGhostBlockAction : EnemyAction
{

	[Export] public int Block { get; set; } = 10;

	
	/// <summary>
	///     执行格挡动作
	///     该方法会创建一个格挡效果并应用到目标身上，然后在延迟后发出动作完成信号
	/// </summary>
	public override void PerformAction()
	{
		// 检查敌方单位和目标是否存在，如果任一为空则直接返回
		if (Enemy == null || Target == null) return;

		// 创建格挡效果实例并设置格挡数值
		var blockEffect = new BlockEffect();
		blockEffect.Amount = Block;
		blockEffect.Sound = Sound;
		// 执行格挡效果，应用到目标身上
		blockEffect.Execute([Enemy]);

		// 创建定时器，在0.6秒后触发动作完成事件
		GetTree().CreateTimer(0.6f, false).Timeout += () =>
		{
			Events.Instance.RaiseEnemyActionCompleted(Enemy);
		};
	}
	/// <summary>
	/// 更新意图文本内容
	/// </summary>
	/// <remarks>
	/// 将当前意图的文本内容重置为基础文本内容
	/// </remarks>
	public override void UpdateIntentText()
	{
		// 计算修改后的伤害值并更新意图文本
		var modifiedDmg =  Enemy.ModifierHandler.GetModifiedValue(Modifier.ModifierType.Block,Block);
		Intent.CurrentText = string.Format(Intent.BaseText, modifiedDmg);
	}
}
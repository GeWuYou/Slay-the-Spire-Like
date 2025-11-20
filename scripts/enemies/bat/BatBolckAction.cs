using Godot;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.global;

namespace SlayTheSpireLike.scripts.enemies.bat;

/// <summary>
///     敌方格挡动作类
///     继承自EnemyAction，用于执行敌方单位的格挡行为
/// </summary>
public partial class BatBolckAction : EnemyAction
{
	[Export] public int Block { get; set; } = 6;

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
}
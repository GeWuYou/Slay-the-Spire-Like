using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.global;

namespace SlayTheSpireLike.scripts.enemies.bat;

/// <summary>
/// 敌人动作模板类，用于定义敌人的具体行为动作
/// 继承自EnemyAction基类，需要实现具体的动作执行逻辑
/// </summary>
public partial class BatAttackAction : EnemyAction
{
	/// <summary>
	/// 获取或设置伤害值属性
	/// </summary>
	/// <value>表示伤害数值的整型属性，默认值为5</value>
	[Export] public int Damage { get; set; } = 5;


	/// <summary>
	/// 执行敌人的攻击动作，包括移动到目标位置、造成伤害、然后返回原位的完整动画序列
	/// </summary>
	/// <remarks>
	/// 该方法会创建一个补间动画来实现敌人的攻击行为，包含以下步骤：
	/// 1. 检查敌人和目标的有效性
	/// 2. 创建并配置补间动画参数
	/// 3. 计算攻击起始位置和目标位置
	/// 4. 设置伤害效果参数
	/// 5. 执行完整的攻击动画序列
	/// 6. 在动画结束后触发敌人行动完成事件
	/// </remarks>
	public override void PerformAction()
	{
		// 检查敌人和目标是否存在
		if (Enemy == null || Target == null) return;

		// 创建补间动画并设置缓动类型
		var tween = CreateTween().SetTrans(Tween.TransitionType.Quint);

		// 计算起始位置和目标位置
		var start = Enemy.GlobalPosition;
		var end = Target.GlobalPosition + Vector2.Right * 32;

		// 创建伤害效果并设置伤害数值
		var damageEffect = new DamageEffect();
		Array<Node> targetArray = [Target];
		damageEffect.Amount = Damage;
		damageEffect.Sound = Sound;

		// 执行攻击动画序列：移动到目标位置 -> 造成伤害 -> 等待 -> 返回起始位置
		tween.TweenProperty(Enemy, "global_position", end, 0.4f);
		tween.TweenCallback(Callable.From(() => damageEffect.Execute(targetArray)));
		tween.TweenInterval(0.35f);
		tween.TweenCallback(Callable.From(() => damageEffect.Execute(targetArray)));
		tween.TweenInterval(0.25f);
		tween.TweenProperty(Enemy, "global_position", start, 0.4f);
		
		// 动画完成后发出信号表示敌人行动结束
		tween.Finished += () => Events.Instance.RaiseEnemyActionCompleted(Enemy);
	}

}
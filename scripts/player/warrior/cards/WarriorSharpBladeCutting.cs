using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.resources;
using Array = Godot.Collections.Array;

namespace SlayTheSpireLike.scripts.player.warrior.cards;

/// <summary>
/// 卡牌逻辑模板类，用于定义卡牌的具体效果和行为
/// </summary>
public partial class WarriorSharpBladeCutting : Card
{
	/// <summary>
	/// 可选的声音列表，用于存储卡牌播放时可能用到的音效资源
	/// </summary>
	[Export] public Array OptionalSoundList { get; set; }

	/// <summary>
	/// 应用卡牌效果到指定目标
	/// </summary>
	/// <param name="targets">目标节点数组，包含卡牌效果作用的目标对象</param>
	protected override void ApplyEffects(Array<Godot.Node> targets)
	{
		// 创建伤害效果实例
		var effect = new DamageEffect();
		effect.Amount = 8;
		effect.Sound = Sound;
		// 执行伤害效果
		effect.Execute(targets);
	}
}
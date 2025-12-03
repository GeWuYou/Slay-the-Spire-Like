using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.modifier_handler;
using SlayTheSpireLike.scripts.resources;
using Array = Godot.Collections.Array;

namespace SlayTheSpireLike.scripts.player.warrior.cards;

/// <summary>
/// 卡牌逻辑模板类，用于定义卡牌的具体效果和行为
/// </summary>
public partial class WarriorTrueStrength : Card
{
	/// <summary>
	/// 可选的声音列表，用于存储卡牌播放时可能用到的音效资源
	/// </summary>
	[Export] public Array OptionalSoundList { get; set; }
	
	/// <summary>
	///	基础力量
	/// </summary>
	[Export] public int BaseStrength { get; set; } = 3;

	/// <summary>
	///     应用卡牌的效果到指定的目标上。子类应重写此方法以实现具体逻辑。
	/// </summary>
	/// <param name="targets">经过处理后的真实目标节点列表</param>
	/// <param name="modifierHandler">修饰符处理器，用于处理效果应用过程中的修饰符</param>
	protected override void ApplyEffects(Array<Node> targets, ModifierHandler modifierHandler)
	{
		// 创建伤害效果实例
		var statusEffect = new StatusEffect();
		var status = ResourceFactories.TrueStrengthFormStatusFactory();
		statusEffect.Status = status;
		statusEffect.Execute(targets);
	}
	public override string GetDescription(ModifierHandler playerModifierHandler, ModifierHandler enemyModifierHandler)
	{
		return string.Format(GetDefaultDescription(),BaseStrength);
	}
}
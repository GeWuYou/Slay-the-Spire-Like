// meta-name: 卡牌逻辑
// meta-description: 作为卡牌逻辑的模板

using System;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.resources;
using Array = Godot.Collections.Array;
using SlayTheSpireLike.scripts.modifier_handler;

/// <summary>
/// 卡牌逻辑模板类，用于定义卡牌的具体效果和行为
/// </summary>
public partial class _CLASS_ : Card
{
    /// <summary>
    /// 可选的声音列表，用于存储卡牌播放时可能用到的音效资源
    /// </summary>
    [Export] public Array OptionalSoundList { get; set; }

    /// <summary>
    ///     应用卡牌的效果到指定的目标上。子类应重写此方法以实现具体逻辑。
    /// </summary>
    /// <param name="targets">经过处理后的真实目标节点列表</param>
    /// <param name="modifierHandler">修饰符处理器，用于处理效果应用过程中的修饰符</param>
    protected override void ApplyEffects(Array<Node> targets,ModifierHandler modifierHandler)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// 获取默认描述文本
    /// </summary>
    /// <returns>返回卡牌的原始描述内容</returns>
    public override virtual string GetDefaultDescription()
    {
        return Description;
    }
    /// <summary>
    /// 获取动态生成的描述文本，考虑修饰符的影响
    /// </summary>
    /// <param name="playerModifierHandler">玩家修饰符处理器</param>
    /// <param name="enemyModifierHandler">敌人修饰符处理器</param>
    /// <returns>返回可能被修饰符修改过的描述文本</returns>
    public override virtual string GetDescription(ModifierHandler playerModifierHandler, ModifierHandler enemyModifierHandler)
    {
        return GetDefaultDescription();
    }
}

using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.modifier_handler;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.relic;

/// <summary>
/// 爆炸桶遗物类，当激活时对所有敌人造成固定伤害
/// </summary>
public partial class ExplosiveBarrel : Relic
{
    /// <summary>
    ///		伤害值
    /// </summary>
    [Export]
    public int 伤害值 { get; set; } = 2;

    /// <summary>
    /// 激活遗物UI效果，对所有敌人造成伤害
    /// </summary>
    /// <param name="relicUi">需要激活的遗物UI组件</param>
    public override void ActivateRelic(RelicUi relicUi)
    {
        relicUi.Flash();
        var tree = relicUi
            .GetTree();
        var enemies = tree
            .GetNodesInGroup(GameConstants.Groups.Enemies);
        var effect = new DamageEffect();
        effect.Type = Modifier.ModifierType.NoModifier;
        effect.Amount = 伤害值;
        effect.Execute(enemies);
    }

    /// <summary>
    /// 获取遗物的提示文本信息
    /// </summary>
    /// <returns>返回当前遗物的提示文本字符串</returns>
    public override string GetTooltip()
    {
        return string.Format(Tooltip, 伤害值);
    }
}
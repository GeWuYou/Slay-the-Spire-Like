using Godot;
using SlayTheSpireLike.scripts.enums;
using SlayTheSpireLike.scripts.relic_handler;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// 遗物资源类，用于定义游戏中的遗物属性和行为
/// </summary>
[GlobalClass]
public partial class Relic : Resource
{
    /// <summary>
    /// 遗物名称
    /// </summary>
    [Export]
    public string RelicName { get; set; }

    /// <summary>
    /// 遗物唯一标识符
    /// </summary>
    [Export]
    public string Id { get; set; }

    /// <summary>
    /// 遗物触发时机类型
    /// </summary>
    [Export]
    public RelicType Type { get; set; }

    /// <summary>
    /// 遗物支持的角色类型
    /// </summary>
    [Export]
    public CharacterType SupportCharacterType { get; set; }

    /// <summary>
    /// 是否为初始遗物
    /// </summary>
    [Export]
    public bool StarterRelic { get; set; }

    /// <summary>
    /// 遗物图标纹理
    /// </summary>
    [Export]
    public Texture Icon { get; set; }

    /// <summary>
    /// 遗物提示文本信息
    /// </summary>
    [Export(PropertyHint.MultilineText)]
    public string Tooltip { get; set; }

    public void InitializeRelic(RelicUi relicUi)
    {
        //todo 初始化遗物UI
    }

    public void ActivateRelic(RelicUi relicUi)
    {
        //todo 激活遗物UI
    }

    public void DeactivateRelic(RelicUi relicUi)
    {
        //todo 停用遗物UI
    }

    public string GetTooltip()
    {
        return Tooltip;
    }

    public bool CanAppearAsReward(CharacterStats characterStats)
    {
        if (StarterRelic)
        {
            return false;
        }

        if (SupportCharacterType == CharacterType.All)
        {
            return true;
        }
        return SupportCharacterType == characterStats.Type;
    }
}
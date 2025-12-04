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

    /// <summary>
    /// 初始化遗物UI显示
    /// </summary>
    /// <param name="relicUi">需要初始化的遗物UI组件</param>
    public virtual void InitializeRelic(RelicUi relicUi)
    {
        //todo 初始化遗物UI
    }

    /// <summary>
    /// 激活遗物UI效果
    /// </summary>
    /// <param name="relicUi">需要激活的遗物UI组件</param>
    public virtual void ActivateRelic(RelicUi relicUi)
    {
        //todo 激活遗物UI
    }

    /// <summary>
    /// 停用遗物UI效果
    /// </summary>
    /// <param name="relicUi">需要停用的遗物UI组件</param>
    public virtual void DeactivateRelic(RelicUi relicUi)
    {
        //todo 停用遗物UI
    }

    /// <summary>
    /// 获取遗物的提示文本信息
    /// </summary>
    /// <returns>返回当前遗物的提示文本字符串</returns>
    public virtual string GetTooltip()
    {
        return Tooltip;
    }

    /// <summary>
    /// 判断该遗物是否可以作为奖励出现
    /// </summary>
    /// <param name="characterStats">角色状态信息，用于判断角色类型匹配性</param>
    /// <returns>如果遗物可作为奖励则返回true，否则返回false</returns>
    public bool CanAppearAsReward(CharacterStats characterStats)
    {
        // 初始遗物不能出现在奖励中
        if (StarterRelic)
        {
            return false;
        }

        // 支持所有角色类型的遗物可以直接返回true
        if (SupportCharacterType == CharacterType.All)
        {
            return true;
        }

        // 否则检查与角色类型是否一致
        return SupportCharacterType == characterStats.Type;
    }
}

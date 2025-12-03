using Godot;
using SlayTheSpireLike.scripts.enums;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// 遗物资源类，用于定义游戏中的遗物属性和行为
/// </summary>
public partial class Relic : Resource
{
    /// <summary>
    /// 遗物触发时机的枚举类型
    /// </summary>
    public enum RelicType
    {
        /// <summary>
        /// 回合开始时触发
        /// </summary>
        StartOfTurn,
        /// <summary>
        /// 战斗开始时触发
        /// </summary>
        StartOfCombat,
        /// <summary>
        /// 回合结束时触发
        /// </summary>
        EndOfTurn,
        /// <summary>
        /// 战斗结束时触发
        /// </summary>
        EndOfCombat,
        /// <summary>
        /// 基于事件触发
        /// </summary>
        EventBased
    }
    
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
}

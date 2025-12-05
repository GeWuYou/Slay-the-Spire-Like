using Godot;

namespace SlayTheSpireLike.scripts.modifier_handler;

/// <summary>
/// 修改值类，用于表示游戏中各种数值的修改器。
/// 支持百分比和固定值两种修改类型，可以应用于伤害计算、属性加成等场景。
/// </summary>
public partial class ModifierValue : Node
{
    /// <summary>
    /// 修改类型枚举，定义了数值修改的方式
    /// </summary>
    public enum ModifierValueType
    {
        /// <summary>
        /// 百分比类型修改，基于原始值的百分比进行增减
        /// </summary>
        PercentBased,
        /// <summary>
        /// 固定值类型修改，直接增加或减少固定数值
        /// </summary>
        Flat
    }
    
    /// <summary>
    /// 获取或设置修改器的类型（百分比或固定值）
    /// </summary>
    [Export] public ModifierValueType Type { get; set; }
    
    /// <summary>
    /// 获取或设置百分比修改值
    /// 当Type为PercentBased时使用此属性
    /// </summary>
    [Export] public float PercentValue { get; set; }
    
    /// <summary>
    /// 获取或设置固定值修改值
    /// 当Type为Flat时使用此属性
    /// </summary>
    [Export] public int FlatValue { get; set; }
    
    /// <summary>
    /// 获取或设置修改器的来源标识
    /// 用于标识是哪个技能、装备或状态产生了此修改器
    /// </summary>
    [Export] public string Source { get; set; }
    
    /// <summary>
    /// 创建一个百分比类型的修改值对象
    /// </summary>
    /// <param name="modifierSource">修改器的来源标识</param>
    /// <param name="whatValueType">修改器的类型</param>
    /// <returns>配置好的ModifiedValue实例</returns>
    public static ModifierValue CreateNewModifier(string modifierSource,ModifierValueType whatValueType)
    {
        var modifiedValue = new ModifierValue();
        modifiedValue.Source = modifierSource;
        modifiedValue.Type = whatValueType;
        return modifiedValue;
    }
}

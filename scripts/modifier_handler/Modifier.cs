using Godot;

namespace SlayTheSpireLike.scripts.modifier_handler;

/// <summary>
/// 修改器类，用于处理游戏中各种数值的修改，如伤害、费用等
/// </summary>
public partial class Modifier : Node
{
    /// <summary>
    /// 修改器类型枚举，定义了可以被修改的游戏数值类型
    /// </summary>
    public enum ModifierType
    {
        /// <summary>
        /// 造成的伤害
        /// </summary>
        DmgDealt,
        /// <summary>
        /// 承受的伤害
        /// </summary>
        DmgTaken,
        /// <summary>
        /// 卡牌费用
        /// </summary>
        CardCost,
        /// <summary>
        /// 商店费用
        /// </summary>
        ShopCost,
        /// <summary>
        /// 无修改器
        /// </summary>
        NoModifier
    }

    [Export] public ModifierType Type { get; set; }

    /// <summary>
    /// 根据来源获取对应的修改值
    /// </summary>
    /// <param name="source">修改值的来源标识</param>
    /// <returns>找到的修改值对象，如果未找到则返回null</returns>
    public ModifierValue GetValue(string source)
    {
        foreach (var child in GetChildren())
        {
            if (child is not ModifierValue value)
            {
                continue;
            }

            if (value.Source != source)
            {
                continue;
            }

            return value;
        }

        return null;
    }

    /// <summary>
    /// 添加或更新修改值
    /// </summary>
    /// <param name="value">要添加的修改值对象</param>
    public void AddValue(ModifierValue value)
    {
        var modifierValue = GetValue(value.Source);
        if (modifierValue is null)
        {
            AddChild(value);
        }
        else
        {
            modifierValue.FlatValue = value.FlatValue;
            modifierValue.PercentValue = value.PercentValue;
        }
    }
    
    /// <summary>
    /// 移除指定来源的修改值
    /// </summary>
    /// <param name="source">要移除的修改值的来源标识</param>
    public void RemoveValue(string source)
    {
        foreach (var child in GetChildren())
        {
            if (child is not ModifierValue value)
            {
                continue;
            }

            if (value.Source != source)
            {
                continue;
            }

            child.QueueFree();
            break;
        }
    }
    
    /// <summary>
    /// 清除所有修改值
    /// </summary>
    public void ClearValues()
    {
        foreach (var child in GetChildren())
        {
            child.QueueFree();
        }
    }

    /// <summary>
    /// 计算修改后的最终数值
    /// 先应用所有固定值修改，再应用所有百分比修改
    /// </summary>
    /// <param name="baseValue">基础数值</param>
    /// <returns>经过所有修改器计算后的最终数值</returns>
    public int GetModifiedValue(int baseValue)
    {
        // 应用所有固定值修改
        var flatResult = baseValue;
        // 应用所有百分比修改
        var percentResult = 1.0f;
        foreach (var child in GetChildren())
        {
            if (child is ModifierValue { Type: ModifierValue.ModifierValueType.Flat } value)
            {
                flatResult+=value.FlatValue;
            }
        }
        foreach (var child in GetChildren())
        {
            if (child is ModifierValue { Type: ModifierValue.ModifierValueType.PercentBased } value)
            {
                percentResult+=value.PercentValue;
            }
        }

        return Mathf.FloorToInt(flatResult * percentResult);
    }
}
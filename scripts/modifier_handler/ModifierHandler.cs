using Godot;

namespace SlayTheSpireLike.scripts.modifier_handler;


public partial class ModifierHandler : Node
{

    /// <summary>
    /// 检查是否存在指定类型的修饰符
    /// </summary>
    /// <param name="type">要检查的修饰符类型</param>
    /// <returns>如果存在指定类型的修饰符则返回true，否则返回false</returns>
    public bool HasModifier(Modifier.ModifierType type)
    {
        // 遍历所有子节点，查找匹配类型的修饰符
        foreach (var child in GetChildren())
        {
            if (child is Modifier modifier && modifier.Type == type)
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 获取指定类型的修饰符实例
    /// </summary>
    /// <param name="type">要获取的修饰符类型</param>
    /// <returns>找到的修饰符实例，如果未找到则返回null</returns>
    public Modifier GetModifier(Modifier.ModifierType type)
    {
        // 遍历所有子节点，查找并返回匹配类型的修饰符
        foreach (var child in GetChildren())
        {
            if (child is Modifier modifier && modifier.Type == type)
            {
                return modifier;
            }
        }

        return null;
    }
    
    /// <summary>
    /// 获取经过指定类型修饰符修改后的值
    /// </summary>
    /// <param name="type">修饰符类型</param>
    /// <param name="baseValue">基础值</param>
    /// <returns>经过修饰符修改后的值，如果没有找到对应修饰符则返回基础值</returns>
    public int GetModifiedValue(Modifier.ModifierType type, int baseValue)
    {
        return GetModifier(type)?.GetModifiedValue(baseValue) ?? baseValue;
    }
}



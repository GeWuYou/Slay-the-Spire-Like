using Godot;
using SlayTheSpireLike.scripts.modifier_handler;

namespace SlayTheSpireLike.scripts.component;

/// <summary>
/// 修饰符组件接口，用于定义具有修饰符处理能力的组件
/// </summary>
public interface IModifierComponent
{
    /// <summary>
    /// 获取或设置修饰符处理器
    /// </summary>
    [Export] public ModifierHandler ModifierHandler { get; set; }
}

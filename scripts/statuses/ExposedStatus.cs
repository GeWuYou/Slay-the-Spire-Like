using System;
using Godot;
using SlayTheSpireLike.scripts.component;
using SlayTheSpireLike.scripts.effects;
using SlayTheSpireLike.scripts.modifier_handler;

namespace SlayTheSpireLike.scripts.statuses;

/// <summary>
/// 状态效果模板类，继承自Status基类。
/// 用于创建自定义的状态效果，可以定义状态的初始化逻辑和应用逻辑。
/// </summary>
public partial class ExposedStatus : Status
{
    public const string Exposed = "Exposed";

    /// <summary>
    /// 伤害倍率
    /// </summary>
    [Export]
    public float Ratio { get; set; } = 0.5f;
    private StatusChangedEventHandler _statusChangedHandler;

    public override void InitializeStatus(Node target)
    {
        if (target is not IModifierComponent modifierComponent)
        {
            return;
        }

        var modifier = modifierComponent.ModifierHandler.GetModifier(Modifier.ModifierType.DmgTaken);
        var value = modifier.GetValue(Exposed);
        if (value is null)
        {
            value = ModifierValue.CreatePercentBased(Exposed, ModifierValue.ModifierValueType.PercentBased);
            value.PercentValue = Ratio;
            modifier.AddValue(value);
        }

        // 保存委托引用以便后续断开连接
        _statusChangedHandler = () => OnStatusChanged(modifier);
        StatusChanged += _statusChangedHandler;
    }

    private void OnStatusChanged(Modifier modifier)
    {
        if (Duration > 0 || modifier is null)
        {
            return;
        }
        modifier.RemoveValue(Exposed);
        StatusChanged -= _statusChangedHandler;
    }
    
}
using Godot;
using SlayTheSpireLike.scripts.component;
using SlayTheSpireLike.scripts.modifier_handler;

namespace SlayTheSpireLike.scripts.statuses;

/// <summary>
/// ExposedStatus 类表示暴露状态效果，该状态会增加目标受到的伤害
/// </summary>
public partial class ExposedStatus : Status
{
    public const string Exposed = "Exposed";

    /// <summary>
    /// 伤害倍率
    /// </summary>
    [Export]
    public float Ratio { get; set; } = 0.5f;

    /// <summary>
    /// 获取状态的工具提示信息
    /// </summary>
    /// <returns>格式化后的工具提示字符串</returns>
    public override string GetTooltip()
    {
        return string.Format(base.GetTooltip(),Duration);
    }

    private StatusChangedEventHandler _statusChangedHandler;

    /// <summary>
    /// 初始化状态效果，为目标添加伤害修正值
    /// </summary>
    /// <param name="target">应用状态的目标节点</param>
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
            value = ModifierValue.CreateNewModifier(Exposed, ModifierValue.ModifierValueType.PercentBased);
            value.PercentValue = Ratio;
            modifier.AddValue(value);
        }

        // 保存委托引用以便后续断开连接
        _statusChangedHandler = () => OnStatusChanged(modifier);
        StatusChanged += _statusChangedHandler;
    }

    /// <summary>
    /// 当状态发生变化时的处理方法，当持续时间结束时移除修饰值
    /// </summary>
    /// <param name="modifier">要处理的修饰器对象</param>
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

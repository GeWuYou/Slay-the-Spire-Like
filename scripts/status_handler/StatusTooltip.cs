using Godot;
using SlayTheSpireLike.scripts.statuses;

namespace SlayTheSpireLike.scripts.status_handler;

/// <summary>
/// 状态提示信息显示控件类
/// 继承自HBoxContainer，用于显示状态的图标和描述文本
/// </summary>
public partial class StatusTooltip : HBoxContainer
{
    /// <summary>
    /// 状态属性，当设置新状态时会自动更新UI显示
    /// </summary>
    [Export]
    public Status Status
    {
        get => _status;
        set
        {
            _status = value;
            CallDeferred(nameof(SetStatus));
        }
    }

    private TextureRect _icon;
    private RichTextLabel _label;
    private Status _status;

    /// <summary>
    /// 节点初始化方法，在节点准备就绪时调用
    /// 获取子节点的引用，包括图标和标签控件
    /// </summary>
    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("Icon");
        _label = GetNode<RichTextLabel>("Label");
    }
    
    /// <summary>
    /// 设置状态显示内容的方法
    /// 更新图标纹理和标签文本为当前状态对应的内容
    /// </summary>
    private void SetStatus()
    {
        _icon.Texture = Status.Icon as Texture2D;
        _label.Text = Status.GetTooltip();
    }
    
}

using Godot;

namespace SlayTheSpireLike.scripts.ui.components;

/// <summary>
/// 统计项控件类，用于显示带有图标和数值的统计信息
/// 继承自HBoxContainer，水平排列图标和数值标签
/// </summary>
public partial class StatItem : HBoxContainer
{
    [Export]
    public Texture IconTexture { get; set; }
    public string FormatString { get; set; } = "{0}";
    private TextureRect _icon;
    private Label _statValue;

    /// <summary>
    /// 节点初始化方法，在节点准备就绪时调用
    /// 获取子节点引用并设置初始图标纹理
    /// </summary>
    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("Icon");
        _statValue = GetNode<Label>("StatValue");
        // 设置图标纹理
        _icon.Texture = IconTexture as Texture2D;
    }

    /// <summary>
    /// 更新统计数值显示
    /// 当数值大于0时显示控件，否则隐藏控件
    /// </summary>
    /// <param name="value">要显示的统计数值</param>
    public void UpdateValue(int value)
    {
        _statValue.Text = string.Format(FormatString, value);
        Visible = value > 0;
    }
}

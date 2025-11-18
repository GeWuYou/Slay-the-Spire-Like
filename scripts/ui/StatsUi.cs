using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
///     StatsUi类负责显示游戏中的统计数据UI
///     继承自HBoxContainer，用于横向排列统计信息
/// </summary>
public partial class StatsUi : HBoxContainer
{
    [Export] public HBoxContainer Block { get; set; }
    [Export] public Label BlockLabel { get; set; }
    [Export] public HBoxContainer Health { get; set; }
    [Export] public Label HealthLabel { get; set; }

    /// <summary>
    ///     更新统计数据显示
    ///     根据传入的Stats对象更新Block和Health的数值显示
    ///     并根据数值决定是否显示对应的UI元素
    /// </summary>
    /// <param name="stats">包含Block和Health数值的Stats对象</param>
    public void UpdateStats(Stats stats)
    {
        if (!(IsInstanceValid(BlockLabel) && IsInstanceValid(HealthLabel))) return;
        // 更新Block和Health的标签文本
        BlockLabel.Text = stats.Block.ToString();
        HealthLabel.Text = stats.Health.ToString();

        // 根据数值决定UI元素的可见性
        Block.Visible = stats.Block > 0;
        Health.Visible = stats.Health > 0;
    }
}
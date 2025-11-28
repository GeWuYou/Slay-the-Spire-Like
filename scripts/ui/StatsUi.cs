using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui.components;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
///     StatsUi类负责显示游戏中的统计数据UI
///     继承自HBoxContainer，用于横向排列统计信息
/// </summary>
public partial class StatsUi : HBoxContainer
{
    [Export] public Texture HealthIcon { get; set; }
    [Export] public Texture BlockIcon { get; set; }
    private StatItem _block;
    private StatItem _health;
    

    /// <summary>
    ///     初始化StatsUi控件
    ///     创建Block和Health的StatItem实例，并设置对应的图标纹理
    /// </summary>
    public override void _Ready()
    {
        _block = ResourceFactories.StatItemFactory();
        _health = ResourceFactories.StatItemFactory();
        _block.IconTexture = BlockIcon;
        _health.IconTexture = HealthIcon;
        AddChild(_block);
        AddChild(_health);
    }

    /// <summary>
    ///     更新统计数据显示
    ///     根据传入的Stats对象更新Block和Health的数值显示
    ///     并根据数值决定是否显示对应的UI元素
    /// </summary>
    /// <param name="stats">包含Block和Health数值的Stats对象</param>
    public void UpdateStats(Stats stats)
    {
        // 检查Block和Health的StatItem实例是否有效
        if (!(IsInstanceValid(_block) && IsInstanceValid(_health))) return;
        // 更新Block和Health的标签文本
        _block.UpdateValue(stats.Block);
        _health.UpdateValue(stats.Health);
    }
}

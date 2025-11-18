using SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.extensions;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
/// 战斗结束面板类，用于显示战斗胜利或失败的结果界面
/// 继承自Godot的Panel控件
/// </summary>
public partial class BattleOverPanel : Panel
{
    /// <summary>
    /// 战斗结果类型枚举
    /// </summary>
    public enum  Type
    {
        Win,   // 胜利
        Lose   // 失败
    }

    [Export] public Label Label { get; set; }
    [Export] public Button ContinueButton { get; set; }
    [Export] public Button RestartButton { get; set; }
    
    /// <summary>
    /// 当节点准备就绪时调用此方法
    /// 初始化按钮点击事件：继续按钮退出游戏，重启按钮重新加载当前场景
    /// </summary>
    public override void _Ready()
    {
        // 绑定继续按钮点击事件，点击后退出游戏
        var sceneTree = GetTree();
        ContinueButton.Pressed += () => sceneTree.Quit();
        // 绑定重启按钮点击事件，点击后重新加载当前场景
        RestartButton.Pressed += () =>
        {
            sceneTree.ReloadCurrentScene();
        };
        // 监听战斗结束界面请求事件
        Events.Instance.BattleOverScreenRequested += ShowScreen;
    }

    /// <summary>
    /// 显示战斗结束界面
    /// 根据战斗结果类型显示相应的文本和按钮
    /// </summary>
    /// <param name="text">要显示的文本内容</param>
    /// <param name="typeValue">战斗结果类型值（胜利或失败）</param>
    public void ShowScreen(string text, int typeValue)
    {
        var type = typeValue.GetBattleOverPanelType();
        Label.Text = text;
        ContinueButton.Visible = type == Type.Win;
        RestartButton.Visible = type == Type.Lose;
        Show();
        GetTree().Paused = true;
    }
}

using global::SlayTheSpireLike.scripts.global;
using Godot;

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
    [Export] public Button MainMenuButton { get; set; }
    
    /// <summary>
    /// 当节点准备就绪时调用此方法
    /// 初始化按钮点击事件：继续按钮退出游戏，重启按钮重新加载当前场景
    /// </summary>
    public override void _Ready()
    {
        ContinueButton.Pressed += () => Events.Instance.RaiseBattleWon();
        MainMenuButton.Pressed +=async () => await SceneTransitionManager.Instance.TransitionToScene(ResourceLoaderManager
            .Instance.GetSceneLoader(GameConstants.ResourcePaths.MainMenuScene).Value);
        // 监听战斗结束界面请求事件
        Events.Instance.BattleOverScreenRequested += ShowScreen;
    }

    public override void _ExitTree()
    {
        // 移除事件监听器
        Events.Instance.BattleOverScreenRequested -= ShowScreen;
    }

    /// <summary>
    /// 显示战斗结束界面
    /// 根据战斗结果类型显示相应的文本和按钮
    /// </summary>
    /// <param name="text">要显示的文本内容</param>
    /// <param name="type">战斗结果类型（胜利或失败）</param>
    public void ShowScreen(string text, Type type)
    {
        Label.Text = text;
        ContinueButton.Visible = type == Type.Win;
        MainMenuButton.Visible = type == Type.Lose;
        Show();
        GetTree().Paused = true;
    }
}

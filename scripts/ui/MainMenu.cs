using System;
using System.Threading.Tasks;
using SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
/// 主菜单界面控制类，负责处理主菜单的各种按钮交互和界面逻辑
/// </summary>
public partial class MainMenu : Control
{
    [Export] public Button ContinueButton { get; set; }
    [Export] public Button NewGameButton { get; set; }
    [Export] public Button QuitButton { get; set; }

    /// <summary>
    /// 当节点准备就绪时调用，初始化按钮事件绑定和游戏状态
    /// </summary>
    public override void _Ready()
    {
        // 取消游戏暂停状态
        GetTree().Paused = false;
        
        // 绑定继续游戏按钮事件
        ContinueButton.Pressed += OnContinueButtonPressed;
        
        // 绑定新游戏按钮事件，使用异步处理
        NewGameButton.Pressed += async () => await OnNewGameButtonPressed();
        
        // 绑定退出按钮事件
        QuitButton.Pressed += () => GetTree().Quit();
    }

    /// <summary>
    /// 处理新游戏按钮按下事件，加载角色选择场景并执行场景切换过渡动画
    /// </summary>
    /// <returns>异步任务</returns>
    private static async Task OnNewGameButtonPressed()
    {
        // 从资源管理器获取角色选择场景
        var packed = ResourceLoaderManager.Instance
            .GetSceneLoader(GameConstants.ResourcePaths.CharacterSelectorScene).Value;

        // 执行场景切换过渡动画
        await SceneTransitionManager.Instance.TransitionToScene(packed);
    }

    /// <summary>
    /// 处理继续游戏按钮按下事件
    /// </summary>
    private static void OnContinueButtonPressed()
    {
        throw new NotImplementedException();
    }
}

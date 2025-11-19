using System.Threading.Tasks;
using Godot;

namespace SlayTheSpireLike.scripts.global;
/// <summary>
/// 场景过渡管理器，用于实现带淡入淡出效果的场景切换。
/// 该类继承自 CanvasLayer，应挂载到场景中并正确设置导出属性 FadeRect 和 Anim。
/// </summary>
public partial class SceneTransitionManager : CanvasLayer
{
    /// <summary>
    /// 获取当前实例的静态访问属性。
    /// </summary>
    public static SceneTransitionManager Instance { get; private set; }
    [Export] public ColorRect FadeRect { get; set; }
    [Export] public AnimationPlayer Anim { get; set; }

    private const string FadeOutAnim = "fade_out";
    private const string FadeInAnim = "fade_in";

    /// <summary>
    /// 初始化节点引用，并将 FadeRect 设置为不可见且完全透明。
    /// 若未在编辑器中指定 FadeRect 或 Anim，则尝试通过名称自动查找子节点。
    /// </summary>
    public override void _Ready()
    {
        Instance = this;
        // 尝试自动找到节点（若你没有在 Inspector 里手动绑定）
        FadeRect ??= GetNodeOrNull<ColorRect>("FadeRect");
        Anim ??= GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

        if (FadeRect == null)
        {
            return;
        }

        FadeRect.Visible = false;
        // 确保完全透明开始
        var m = FadeRect.Modulate;
        m.A = 0f;
        FadeRect.Modulate = m;
    }

    /// <summary>
    /// 执行一个完整的场景过渡流程：淡出当前画面 -> 切换场景 -> 淡入新场景。
    /// </summary>
    /// <param name="targetScene">目标场景资源</param>
    /// <returns>异步任务</returns>
    public async Task TransitionToScene(PackedScene targetScene)
    {
        if (FadeRect == null || Anim == null)
        {
            // 回退：若没有配置 animation 或 fade rect，就直接切场景
            GetTree().ChangeSceneToPacked(targetScene);
            return;
        }

        // 确保可见并在最上层
        FadeRect.Visible = true;

        // 播放遮盖动画（会把 modulate.a 从 0 -> 1）
        Anim.Play(FadeOutAnim);
        await ToSignal(Anim, $"animation_finished");

        // 切换场景（此时屏幕被遮盖，不会看到突变）
        GetTree().ChangeSceneToPacked(targetScene);

        // 播放解除遮罩动画（会把 modulate.a 从 1 -> 0）
        Anim.Play(FadeInAnim);
        await ToSignal(Anim, "animation_finished");

        // 隐藏，恢复输入
        FadeRect.Visible = false;
    }

    /// <summary>
    /// 只播放一次淡出动画，使画面逐渐变为全黑（或指定颜色）。
    /// </summary>
    /// <returns>异步任务</returns>
    public async Task PlayFadeOut()
    {
        if (FadeRect == null || Anim == null) return;
        FadeRect.Visible = true;
        Anim.Play(FadeOutAnim);
        await ToSignal(Anim, "animation_finished");
    }

    /// <summary>
    /// 只播放一次淡入动画，使画面从全黑（或指定颜色）逐渐显现。
    /// </summary>
    /// <returns>异步任务</returns>
    public async Task PlayFadeIn()
    {
        if (FadeRect == null || Anim == null) return;
        Anim.Play(FadeInAnim);
        await ToSignal(Anim, "animation_finished");
        FadeRect.Visible = false;
    }
}

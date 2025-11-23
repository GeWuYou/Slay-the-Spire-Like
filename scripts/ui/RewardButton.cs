using Godot;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
/// 奖励按钮控件类，用于显示奖励图标和文本，并在点击时销毁自身
/// 继承自Godot的Button控件，提供奖励相关的UI展示功能
/// </summary>
public partial class RewardButton : Button
{
    private Texture _rewardIcon;
    private string _rewardText;

    /// <summary>
    /// 获取或设置奖励图标纹理
    /// 当值发生变化时会延迟调用SetRewardIcon方法更新界面显示
    /// </summary>
    [Export]
    public Texture RewardIcon
    {
        get => _rewardIcon;
        set
        {
            _rewardIcon = value;
            CallDeferred(nameof(SetRewardIcon));
        }
    }

    /// <summary>
    /// 获取或设置奖励文本内容
    /// 当值发生变化时会延迟调用SetRewardText方法更新界面显示
    /// </summary>
    [Export]
    public string RewardText
    {
        get => _rewardText;
        set
        {
            _rewardText = value;
            CallDeferred(nameof(SetRewardText));
        }
    }

    [Export]
    public TextureRect IconRect { get; set; }
    [Export]
    public Label TextLabel { get; set; }

    /// <summary>
    /// 设置奖励图标显示
    /// 将RewardIcon赋值给IconRect的Texture属性
    /// </summary>
    public void SetRewardIcon()
    {
        IconRect.Texture = RewardIcon as Texture2D;
    }
    
    /// <summary>
    /// 设置奖励文本显示
    /// 将RewardText赋值给TextLabel的Text属性
    /// </summary>
    public void SetRewardText()
    {
        TextLabel.Text = RewardText;
    }

    /// <summary>
    /// 控件初始化完成时调用
    /// 注册按钮按下事件处理函数
    /// </summary>
    public override void _Ready()
    {
        Pressed+=OnPressed;
    }

    /// <summary>
    /// 按钮按下事件处理函数
    /// 点击按钮后将该控件从场景树中移除
    /// </summary>
    private void OnPressed()
    {
        QueueFree();
    }
}

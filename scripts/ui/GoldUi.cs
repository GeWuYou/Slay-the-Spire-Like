using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
/// 金币UI控制器类，用于显示和更新游戏中的金币数量
/// 继承自HBoxContainer，作为UI容器使用
/// </summary>
public partial class GoldUi : HBoxContainer
{
    private RunStats _runStats;
    private Callable _callable;

    /// <summary>
    /// 获取或设置运行统计信息对象，并自动处理信号连接
    /// 当RunStats属性被设置时，会自动连接GoldChanged信号到OnGoldChanged方法
    /// </summary>
    public RunStats RunStats
    {
        get => _runStats;
        set
        {
            // 如果之前绑定了一个 CardPile，先断开旧连接，避免重复触发
            if (_runStats != null &&  _runStats.IsConnected(RunStats.SignalName.GoldChanged, _callable))
            {
                _runStats.Disconnect(CardPile.SignalName.CardPileSizeChanged, _callable);
            }

            _runStats = value;
            if (RunStats.IsConnected(RunStats.SignalName.GoldChanged, _callable))
            {
                return;
            }
            RunStats.Connect(RunStats.SignalName.GoldChanged, _callable);
            OnGoldChanged();
        }
    }
    
    /// <summary>
    /// 获取或设置用于显示金币数量的标签控件
    /// </summary>
    [Export]
    public Label Label { get; set; }

    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 初始化Callable对象并设置标签初始文本
    /// </summary>
    public override void _Ready()
    {
        _callable = new Callable(this, nameof(OnGoldChanged));
        Label.Text = "0";
    }

    /// <summary>
    /// 金币数量变化时的回调方法
    /// 更新标签文本以显示当前金币数量
    /// </summary>
    private void OnGoldChanged()
    {
        Label.Text = RunStats.Gold.ToString();
    }
}

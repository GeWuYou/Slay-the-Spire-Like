using Godot;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// 运行状态资源类，用于跟踪游戏运行期间的统计信息
/// </summary>
public partial class RunStats : Resource
{
    private int _gold = 70;

    /// <summary>
    /// 金币变化事件处理器委托
    /// 当金币数量发生变化时触发此信号
    /// </summary>
    [Signal]
    public delegate void GoldChangedEventHandler();

    /// <summary>
    /// 获取或设置当前金币数量
    /// 当金币数量被修改时，会自动触发GoldChanged信号通知监听者
    /// </summary>
    [Export]
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            // 发送金币变化信号通知UI等组件更新显示
            EmitSignal(SignalName.GoldChanged);
        }
    }
}

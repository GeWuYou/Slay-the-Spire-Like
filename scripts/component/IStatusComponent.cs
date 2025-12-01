using Godot;
using SlayTheSpireLike.scripts.status_handler;

namespace SlayTheSpireLike.scripts.component;

/// <summary>
/// 状态组件接口，定义了具有状态处理器的对象应该实现的契约。
/// 任何实现此接口的类都需要提供一个状态处理器属性。
/// </summary>
public interface IStatusComponent
{
     /// <summary>
     /// 获取或设置状态处理器组件。
     /// 状态处理器负责管理角色的各种状态效果，如增益、减益等。
     /// </summary>
     [Export] public StatusHandler StatusHandler { get; set; }
}

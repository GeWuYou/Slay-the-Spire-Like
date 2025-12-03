using System.Collections.Generic;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.map;

/// <summary>
/// 地图房间节点类，继承自Godot的Area2D。用于表示游戏地图中的一个房间。
/// 包含房间图标、连线动画等视觉元素，并处理玩家交互事件。
/// </summary>
public partial class MapRoom : Area2D
{
    /// <summary>
    /// 房间图标精灵组件（导出到编辑器）
    /// </summary>
    [Export]
    public Sprite2D Icon { get; set; }

    /// <summary>
    /// 连线渲染组件（导出到编辑器），用于显示与相邻房间之间的连接线
    /// </summary>
    [Export]
    public Line2D Line { get; set; }

    /// <summary>
    /// 动画播放器组件（导出到编辑器），控制高亮、选择等状态动画
    /// </summary>
    [Export]
    public AnimationPlayer AnimationPlayer { get; set; }

    /// <summary>
    /// 当前房间被选中时触发的信号委托定义
    /// 参数: Room - 被选中的房间数据对象
    /// </summary>
    [Signal]
    public delegate void SelectedEventHandler(Room room);

    /// <summary>
    /// 获取或设置当前房间是否可访问/可用。
    /// 设置该属性会调用延迟方法SetAvailable来更新UI表现。
    /// </summary>
    public bool Available
    {
        get => _available;
        set
        {
            _available = value;
            CallDeferred(nameof(SetAvailable));
        }
    }

    /// <summary>
    /// 获取或设置当前房间的数据模型。
    /// 设置该属性会调用延迟方法SetRoom来同步位置和图标信息。
    /// </summary>
    public Room Room
    {
        get => _room;
        set
        {
            _room = value;
            CallDeferred(nameof(SetRoom));
        }
    }

    /// <summary>
    /// 表示房间图标的配置结构体，包括纹理和缩放比例
    /// </summary>
    public struct RoomIconData
    {
        /// <summary>
        /// 图标使用的纹理资源
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// 图标的缩放向量
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// 构造一个新的房间图标数据实例
        /// </summary>
        /// <param name="texture">要使用的纹理</param>
        /// <param name="scale">图标的缩放大小</param>
        public RoomIconData(Texture2D texture, Vector2 scale)
        {
            Texture = texture;
            Scale = scale;
        }
    }

    /// <summary>
    /// 静态字典，存储不同房间类型对应的图标配置数据
    /// Key: 房间类型枚举值；Value: 对应的图标纹理及缩放信息
    /// </summary>
    public static Dictionary<Room.Type, RoomIconData> Icons { get; } = new()
    {
        { Room.Type.Unknown, new RoomIconData(null, Vector2.One) },
        { Room.Type.Monster, new RoomIconData(ResourceFactories.MonsterFactory(), Vector2.One) },
        { Room.Type.Treasure, new RoomIconData(ResourceFactories.TreasureFactory(), Vector2.One) },
        { Room.Type.Campfire, new RoomIconData(ResourceFactories.CampfireFactory(), new Vector2(0.6f, 0.6f)) },
        { Room.Type.Shop, new RoomIconData(ResourceFactories.ShopFactory(), new Vector2(0.6f, 0.6f)) },
        { Room.Type.Boss, new RoomIconData(ResourceFactories.BossFactory(), new Vector2(1.25f, 1.25f)) }
    };

    // 私有字段：标记房间是否可用
    private bool _available;

    // 私有字段：绑定的实际房间数据对象
    private Room _room;

    /// <summary>
    /// 根据Available属性的状态更新房间的可视化效果。
    /// 如果房间可用则播放高亮动画，否则若未被选中则重置动画状态。
    /// 此方法通过CallDeferred在下一帧执行以避免Godot内部错误。
    /// </summary>
    public void SetAvailable()
    {
        if (Available)
        {
            AnimationPlayer.Play("highlight");
        }
        else if (!Room.IsSelected)
        {
            AnimationPlayer.Play("RESET");
        }
    }

    /// <summary>
    /// 同步房间的位置和图标信息至UI组件。
    /// 更新节点坐标、旋转连线角度以及根据房间类型设定图标纹理和缩放。
    /// 此方法同样使用CallDeferred机制进行异步调用。
    /// </summary>
    public void SetRoom()
    {
        Position = Room.Position;
        Line.RotationDegrees = GlobalBean.RandomNumberGenerator.RandiRange(0, 360);
        Icon.Texture = Icons[Room.RoomType].Texture;
        Icon.Scale = Icons[Room.RoomType].Scale;
    }

    /// <summary>
    /// 初始化回调函数，在节点加载完成后注册输入事件监听器。
    /// </summary>
    public override void _Ready()
    {
        InputEvent += OnInputEvent;
    }

    /// <summary>
    /// 显示房间已被选中的视觉反馈，将连线颜色设为白色。
    /// </summary>
    public void ShowSelected()
    {
        Line.Modulate = Colors.White;
    }

    /// <summary>
    /// 处理用户点击或按键操作，当房间处于可用状态且按下鼠标左键
    /// 将房间标记为已选中并播放选择动画。
    /// </summary>
    /// <param name="viewport">事件来源视口节点</param>
    /// <param name="event">输入事件对象</param>
    /// <param name="shapeIdx">碰撞形状索引</param>   
    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (!Available || !@event.IsActionPressed("left_mouse"))
        {
            return;
        }
        Room.IsSelected = true;
        AnimationPlayer.Play("select");
    }

    /// <summary>
    /// 响应地图房间的选择动作，发出Selected信号通知外部系统当前房间已被选中。
    /// </summary>
    public void OnMapRoomSelected()
    {
        EmitSignal(SignalName.Selected, Room);
    }
}
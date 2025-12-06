using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.map;

/// <summary>
/// 表示游戏中的地图系统，负责生成、显示和交互式浏览地图。
/// 包含房间、连线以及与玩家输入相关的滚动逻辑。
/// </summary>
public partial class Map : Node2D
{
    /// <summary>
    /// 摄像机在Y轴方向的边界值，用于控制摄像机的移动范围
    /// </summary>
    private float _cameraEdgeY;

    /// <summary>
    /// 记录已爬升的楼层数量
    /// </summary>
    public int FloorsClimbed { get; set; }

    /// <summary>
    /// 存储地图数据的二维数组，用于保存各个楼层和房间的信息
    /// </summary>
    public Array<Array<Room>> MapData { get; set; }

    /// <summary>
    ///   地图滚动速度
    /// </summary>
    [Export]
    public float ScrollSpeed { get; set; } = 15;

    /// <summary>
    /// 地图生成器节点引用，用于生成地图结构
    /// </summary>
    [Export]
    public MapGenerator MapGenerator { get; set; }

    /// <summary>
    /// 连线容器节点引用，用于管理地图房间之间的连接线
    /// </summary>
    [Export]
    public Node2D Lines { get; set; }

    /// <summary>
    /// 房间容器节点引用，用于管理所有地图房间节点
    /// </summary>
    [Export]
    public Node2D Rooms { get; set; }

    /// <summary>
    /// 视觉元素容器节点引用，用于管理地图的视觉效果和UI
    /// </summary>
    [Export]
    public Node2D Visuals { get; set; }

    /// <summary>
    /// 2D摄像机节点引用，用于控制地图的视图和缩放
    /// </summary>
    [Export]
    public Camera2D Camera2D { get; set; } 

    /// <summary>
    /// 记录上一个访问的房间对象
    /// </summary>
    public Room LastRoom { get; set; }

    /// <summary>
    /// 初始化地图组件，在场景加载完成后调用。
    /// 设置摄像机Y轴边界的初始值。
    /// </summary>
    public override void _Ready()
    {
        _cameraEdgeY = MapGenerator.YDist * (MapGenerator.Floors - 1);
    }

    /// <summary>
    /// 解锁指定层级的所有房间，使其变为可选状态。
    /// </summary>
    /// <param name="level">需要解锁的楼层编号（从0开始）</param>
    public void UnlockFloor(int level)
    {
        foreach (var child in Rooms.GetChildren())
        {
            if (child is not MapRoom mapRoom)
            {
                continue;
            }

            if (mapRoom.Room.Row == level)
            {
                mapRoom.Available = true;
            }
        }
    }

    /// <summary>
    /// 根据当前最后进入的房间，解锁其后续可以前往的所有房间。
    /// </summary>
    public void UnlockNextRooms()
    {
        foreach (var child in Rooms.GetChildren())
        {
            if (child is not MapRoom mapRoom)
            {
                continue;
            }

            if (LastRoom.NextRooms.Contains(mapRoom.Room))
            {
                mapRoom.Available = true;
            }
        }
    }

    /// <summary>
    /// 显示地图界面并启用摄像机功能。
    /// </summary>
    public void ShowMap()
    {
        Show();
        Camera2D.Enabled = true;
    }

    /// <summary>
    /// 隐藏地图界面并禁用摄像机功能。
    /// </summary>
    public void HideMap()
    {
        Hide();
        Camera2D.Enabled = false;
    }

    /// <summary>
    /// 重新生成一张新的随机地图，并初始化相关数据。
    /// </summary>
    public void GenerateNewMap()
    {
        FloorsClimbed = 0;
        MapData = MapGenerator.GenerateMap();
        CreateMap();
    }
    public void LoadMap(Array<Array<Room>> mapData,int floorsCompleted,Room lastRoom)
    {
        FloorsClimbed = floorsCompleted;
        MapData = mapData;
        LastRoom = lastRoom;
        CreateMap();
        UnlockFloor(floorsCompleted > 0 ? floorsCompleted : 0);
    }
    /// <summary>
    /// 创建地图可视化内容，包括房间及其连接线。
    /// 同时居中整个地图布局。
    /// </summary>
    private void CreateMap()
    {
        foreach (var currentFloor in MapData)
        {
            foreach (var room in currentFloor)
            {
                // 跳过没有前后连接关系的孤立房间
                if (room.NextRooms.Count <= 0 && room.PreviousRoomKeys.Count <= 0)
                {
                    continue;
                }

                SpawnRoom(room);
            }
        }

        var mapWidthPixels = MapGenerator.XDist * (MapGenerator.MapWidth - 1);
        var visualPosition = Visuals.Position;
        var viewportRect = GetViewportRect();
        visualPosition.X = (viewportRect.Size.X - mapWidthPixels) / 2;
        visualPosition.Y = viewportRect.Size.Y / 2;
        Visuals.Position = visualPosition;
    }

    /// <summary>
    /// 实例化一个地图房间实例，并将其加入到房间容器中。
    /// 同时创建该房间与其他相邻房间之间的连线。
    /// </summary>
    /// <param name="room">要创建的地图房间信息</param>
    private void SpawnRoom(Room room)
    {
        var mapRoom = ResourceFactories.MapRoomFactory();
        mapRoom.Room = room;
        mapRoom.Selected += OnMapRoomSelected;
        Rooms.AddChild(mapRoom);
        ConnectLines(room);
        if (room.IsSelected && room.Row < FloorsClimbed)
        {
            mapRoom.ShowSelected();
        }
    }

    /// <summary>
    /// 当某个地图房间被点击选择后触发此方法。
    /// 将同层其他房间设为不可选，并更新最后访问的房间及已爬升楼层数。
    /// 最终发出地图退出事件通知外部系统。
    /// </summary>
    /// <param name="room">用户所选择的房间对象</param>
    private void OnMapRoomSelected(Room room)
    {
        foreach (var child in Rooms.GetChildren())
        {
            if (child is not MapRoom mapRoom)
            {
                continue;
            }

            if (mapRoom.Room.Row == room.Row)
            {
                mapRoom.Available = false;
            }
        }

        LastRoom = room;
        FloorsClimbed++;
        Events.Instance.RaiseMapExited(room);
    }

    /// <summary>
    /// 绘制给定房间与其下一层相邻房间之间的连接线段。
    /// </summary>
    /// <param name="room">起始房间对象</param>
    private void ConnectLines(Room room)
    {
        if (room.NextRooms.Count == 0)
        {
            return;
        }

        foreach (var nextRoom in room.NextRooms)
        {
            var line = ResourceFactories.MapLineFactory();
            line.AddPoint(room.Position);
            line.AddPoint(nextRoom.Position);
            Lines.AddChild(line);
        }
    }

    /// <summary>
    /// 处理用户的键盘或控制器输入，实现地图上下滚动查看的功能。
    /// 只有当地图可见时才响应操作。
    /// </summary>
    /// <param name="event">Godot 输入事件对象</param>
    public override void _Input(InputEvent @event)
    {
        if (!Visible)
        {
            return;
        }

        var cameraPosition = Camera2D.Position;
        var y = cameraPosition.Y;
        if (@event.IsActionPressed("scroll_up"))
        {
            y -= ScrollSpeed;
        }
        else if (@event.IsActionPressed("scroll_down"))
        {
            y += ScrollSpeed;
        }

        y = Mathf.Clamp(y, -_cameraEdgeY, 0f);
        cameraPosition.Y = y;
        Camera2D.Position = cameraPosition;
    }
}

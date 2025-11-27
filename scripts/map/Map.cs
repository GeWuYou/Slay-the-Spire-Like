using SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.map;

public partial class Map : Node2D
{
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
    /// 存储地图数据的二维数组，用于保存各个楼层和房间的信息
    /// </summary>
    private Array<Array<Room>> _mapData;

    /// <summary>
    /// 记录已爬升的楼层数量
    /// </summary>
    private int _floorsClimbed;

    /// <summary>
    /// 记录上一个访问的房间对象
    /// </summary>
    public Room LastRoom { get; set; }

    /// <summary>
    /// 摄像机在Y轴方向的边界值，用于控制摄像机的移动范围
    /// </summary>
    private float _cameraEdgeY;


    public override void _Ready()
    {
        _cameraEdgeY = MapGenerator.YDist * (MapGenerator.Floors - 1);
    }

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

    public void ShowMap()
    {
        Show();
        Camera2D.Enabled = true;
    }
    
    public void HideMap()
    {
        Hide();
        Camera2D.Enabled = false;
        
    }
    public void GenerateNewMap()
    {
        _floorsClimbed = 0;
        _mapData = MapGenerator.GenerateMap();
        CreateMap();
    }

    private void CreateMap()
    {
        foreach (var currentFloor in _mapData)
        {
            foreach (var room in currentFloor)
            {
                if (room.NextRooms.Count <= 0 && room.PreviousRooms.Count <= 0)
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

    private void SpawnRoom(Room room)
    {
        var mapRoom = ResourceFactories.MapRoomFactory();
        mapRoom.Room = room;
        mapRoom.Selected += OnMapRoomSelected;
        Rooms.AddChild(mapRoom);
        ConnectLines(room);
        if (room.IsSelected && room.Row < _floorsClimbed)
        {
            mapRoom.ShowSelected();
        }
    }

    private void OnMapRoomSelected(Room room)
    {
        foreach (var child in Rooms.GetChildren())
        {
            if (child is not MapRoom mapRoom)
            {
                continue;
            }

            if (mapRoom.Room.Row==room.Row)
            {
                mapRoom.Available = false;
            }
        }
        LastRoom = room;
        _floorsClimbed++;
        Events.Instance.RaiseMapExited(room);
    }

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

    public override void _Input(InputEvent @event)
    {
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
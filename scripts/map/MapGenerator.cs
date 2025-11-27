using System;
using System.Linq;
using SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.map;

/// <summary>
/// 地图生成器类，用于生成游戏地图结构。
/// 继承自Node节点，包含地图生成所需的各种配置参数。
/// </summary>
public partial class MapGenerator : Node
{
    #region 常量

    /// <summary>
    /// X轴方向上房间之间的距离。
    /// </summary>
    [Export]
    public int XDist { get; set; } = 30;

    /// <summary>
    /// Y轴方向上房间之间的距离。
    /// </summary>
    [Export]
    public int YDist { get; set; } = 25;

    /// <summary>
    /// 房间放置时的随机偏移量，用于增加地图的随机性。
    /// </summary>
    [Export]
    public int PlacementRandomness { get; set; } = 5;

    /// <summary>
    /// 地图的层数/楼层数量。
    /// </summary>
    [Export]
    public int Floors { get; set; } = 15;

    /// <summary>
    /// 地图宽度，表示每层的最大房间数量。
    /// </summary>
    [Export]
    public int MapWidth { get; set; } = 7;

    /// <summary>
    /// 路径数量，表示从起始层到结束层的路径分支数。
    /// </summary>
    [Export]
    public int Paths { get; set; } = 6;

    /// <summary>
    /// 怪物房间的权重值，影响怪物房间在地图中出现的概率。
    /// 权重越高，生成概率越大。
    /// </summary>
    [Export]
    public float MonsterRoomWeight { get; set; } = 10.0f;

    /// <summary>
    /// 商店房间的权重值，影响商店房间在地图中出现的概率。
    /// 权重越高，生成概率越大。
    /// </summary>
    [Export]
    public float ShopRoomWeight { get; set; } = 2.5f;

    /// <summary>
    /// 营火房间的权重值，影响营火房间在地图中出现的概率。
    /// 权重越高，生成概率越大。
    /// </summary>
    [Export]
    public float CampfireRoomWeight { get; set; } = 4.0f;

    #endregion

    [Export]
    public BattleStatsPool BattleStatsPool { get; set; }
    /// <summary>
    /// 随机房间类型权重字典，用于控制不同类型房间在随机生成时的概率权重。
    /// </summary>
    /// <remarks>
    /// 包含三种房间类型的权重配置：
    /// - Monster: 怪物房间权重
    /// - Shop: 商店房间权重  
    /// - Campfire: 营火房间权重
    /// 权重值为0表示该类型房间不会被随机选中。
    /// </remarks>
    public Dictionary<Room.Type, float> RandomRoomTypeWeights { get; set; } = new()
    {
        { Room.Type.Monster, 0.0f },
        { Room.Type.Shop, 0.0f },
        { Room.Type.Campfire, 0.0f },
    };

    /// <summary>
    /// 获取或设置随机房间类型的总权重值。
    /// </summary>
    public float RandomRoomTypeTotalWeight { get; set; }

    /// <summary>
    /// 获取或设置地图数据的二维数组。
    /// </summary>
    public Array<Array<Room>> MapData { get; set; } = [];
    
    /// <summary>
    /// 主要的地图生成逻辑入口函数。
    /// 完成整个地图结构的初始化、路径连接、房间类型分配等操作。
    /// </summary>
    /// <returns>返回完整的地图数据结构。</returns>
    public Array<Array<Room>> GenerateMap()
    {
        MapData = GenerateInitialGrid();
        var startingPoints = GetRandomStartingPoints();

        foreach (var col in startingPoints)
        {
            var currentCol = col;

            for (var row = 0; row < Floors - 1; row++)
            {
                currentCol = SetupConnection(row, currentCol);
            }
        }
        BattleStatsPool.Setup();
        SetupBossRoom();
        SetupRandomRoomWeights();
        SetupRoomTypes();
        DedupeAllNextRooms();
        return MapData;
    }


    /// <summary>
    /// 对地图数据中所有房间的下一个房间列表进行去重处理。
    /// </summary>
    /// <remarks>
    /// 遍历MapData中的每个房间，将其NextRooms列表中的重复项移除，
    /// 确保每个房间的下一个房间列表中不包含重复的房间引用。
    /// </remarks>
    private void DedupeAllNextRooms()
    {
        // 遍历所有地图数据中的房间
        foreach (var t in MapData)
        {
            foreach (var room in t)
            {
                // 跳过空房间或没有下一个房间的房间
                if (room?.NextRooms == null) continue;

                // 使用HashSet对下一个房间列表进行去重，然后转换回数组
                var uniq = new System.Collections.Generic.HashSet<Room>(room.NextRooms);
                room.NextRooms = new Array<Room>(uniq.ToArray());
            }
        }
    }


    /// <summary>
    /// 设置房间类型。
    /// </summary>
    /// <remarks>
    /// 该函数根据楼层位置为房间分配特定类型：
    /// - 第一层（索引0）的可通行房间设置为怪物房间
    /// - 中间层（Floors/2）的可通行房间设置为宝藏房间
    /// - 倒数第二层（Floors-2）的可通行房间设置为篝火房间
    /// </remarks>
    private void SetupRoomTypes()
    {
        // 设置第一层房间类型为怪物房间
        foreach (var room in MapData[0])
        {
            if (room.NextRooms.Count > 0)
            {
                room.RoomType = Room.Type.Monster;
                room.BattleStats = BattleStatsPool.GetRandomBattleStatsForTier(0);
            }
        }

        // 设置中间层房间类型为宝藏房间
        foreach (var room in MapData[Floors / 2])
        {
            if (room.NextRooms.Count > 0)
            {
                room.RoomType = Room.Type.Treasure;
            }
        }

        // 设置倒数第二层房间类型为篝火房间
        foreach (var room in MapData[Floors - 2])
        {
            if (room.NextRooms.Count > 0)
            {
                room.RoomType = Room.Type.Campfire;
            }
        }

        foreach (var rooms in MapData)
        {
            foreach (var room in rooms)
            {
                foreach (var nextRoom in room.NextRooms)
                {
                    if (nextRoom.RoomType == Room.Type.Unknown)
                    {
                        SetRoomRandomly(nextRoom);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 随机设置房间类型，根据特定规则避免不合法的房间生成配置。
    /// </summary>
    /// <param name="nextRoom">需要设置类型的房间对象。</param>
    private void SetRoomRandomly(Room nextRoom)
    {
        const int maxAttempts = 30;
        var attempts = 0;

        while (attempts++ < maxAttempts)
        {
            var roomType = GetRandomRoomTypeByWeight();
            var isCampfire = roomType == Room.Type.Campfire;
            var hasCampfireParent = RoomHasParentOfType(nextRoom, Room.Type.Campfire);
            var isShop = roomType == Room.Type.Shop;
            var hasShopParent = RoomHasParentOfType(nextRoom, Room.Type.Shop);

            // 营火房间不能出现在第3行之前
            if (isCampfire && nextRoom.Row < 3)
            {
                continue;
            }

            // 营火房间不能有营火父房间（避免连续营火）
            if (isCampfire && hasCampfireParent)
            {
                continue;
            }

            // 商店房间不能有商店父房间（避免连续商店）
            if (isShop && hasShopParent)
            {
                continue;
            }

            // 营火房间不能出现在最后一行
            if (isCampfire && nextRoom.Row >= Floors - 3)
            {
                continue;
            }


            // 通过所有约束，赋值并退出
            nextRoom.RoomType = roomType;
            if (roomType != Room.Type.Monster)
            {
                return;
            }
            var tierForMonsterRooms = 0;
            if (nextRoom.Row > 2)
            {
                tierForMonsterRooms = 1;
            }
            nextRoom.BattleStats = BattleStatsPool.GetRandomBattleStatsForTier(tierForMonsterRooms);
            return;
        }

        // 若超过尝试次数仍无合法类型，就退化为 Monster
        nextRoom.RoomType = Room.Type.Monster;
    }


    /// <summary>
    /// 检查指定房间是否具有指定类型的父房间。
    /// </summary>
    /// <param name="room">要检查的房间。</param>
    /// <param name="type">要查找的父房间类型。</param>
    /// <returns>如果存在指定类型的父房间则返回true，否则返回false。</returns>
    private bool RoomHasParentOfType(Room room, Room.Type type)
    {
        if (room == null) return false;
        var parents = new System.Collections.Generic.List<Room>();

        var prevRow = room.Row - 1;
        if (prevRow < 0) return false;

        // left
        if (room.Column - 1 >= 0)
        {
            var leftParent = MapData[prevRow][room.Column - 1];
            if (leftParent != null && leftParent.NextRooms.Contains(room))
                parents.Add(leftParent);
        }

        // middle
        var midParent = MapData[prevRow][room.Column];
        if (midParent != null && midParent.NextRooms.Contains(room))
            parents.Add(midParent);

        // right
        if (room.Column + 1 >= MapWidth)
        {
            return parents.Any(p => p.RoomType == type);
        }

        var rightParent = MapData[prevRow][room.Column + 1];
        if (rightParent != null && rightParent.NextRooms.Contains(room))
            parents.Add(rightParent);

        return parents.Any(p => p.RoomType == type);
    }


    /// <summary>
    /// 根据权重随机获取房间类型。
    /// </summary>
    /// <returns>返回根据权重随机选择的房间类型。</returns>
    private Room.Type GetRandomRoomTypeByWeight()
    {
        var roll = GlobalBean.RandomNumberGenerator.RandfRange(0.0f, RandomRoomTypeTotalWeight);

        // 明确顺序：Monster, Campfire, Shop
        if (roll < RandomRoomTypeWeights[Room.Type.Monster])
        {
            return Room.Type.Monster;
        }

        return roll < RandomRoomTypeWeights[Room.Type.Campfire] ? Room.Type.Campfire : Room.Type.Shop;
    }


    /// <summary>
    /// 设置随机房间类型的权重值，用于后续的随机房间选择。
    /// </summary>
    /// <remarks>
    /// 该方法通过累加的方式设置不同房间类型的权重范围：
    /// - 怪物房间权重范围：[0, MonsterRoomWeight)
    /// - 营火房间权重范围：[MonsterRoomWeight, MonsterRoomWeight + CampfireRoomWeight)
    /// - 商店房间权重范围：[MonsterRoomWeight + CampfireRoomWeight, MonsterRoomWeight + CampfireRoomWeight + ShopRoomWeight)
    /// </remarks>
    private void SetupRandomRoomWeights()
    {
        // 设置怪物房间的权重上限
        RandomRoomTypeWeights[Room.Type.Monster] = MonsterRoomWeight;
        // 设置营火房间的权重上限（累加前面的权重）
        RandomRoomTypeWeights[Room.Type.Campfire] = MonsterRoomWeight + CampfireRoomWeight;
        // 设置商店房间的权重上限（累加所有权重）
        RandomRoomTypeWeights[Room.Type.Shop] = MonsterRoomWeight + CampfireRoomWeight + ShopRoomWeight;

        // 保存总权重值，用于后续的随机选择计算
        RandomRoomTypeTotalWeight = RandomRoomTypeWeights[Room.Type.Shop];
    }


    /// <summary>
    /// 设置Boss房间并连接通往Boss房间的路径。
    /// </summary>
    /// <remarks>
    /// 该函数将地图最顶层中间位置的房间设置为Boss房间，
    /// 并将倒数第二层所有具有下级房间的房间都连接到Boss房间。
    /// </remarks>
    private void SetupBossRoom()
    {
        var middle = Mathf.FloorToInt(MapWidth * 0.5f);
        var bossRoom = MapData[Floors - 1][middle];
        bossRoom.RoomType = Room.Type.Boss;
        bossRoom.BattleStats = BattleStatsPool.GetRandomBattleStatsForTier(2);
        for (var j = 0; j < MapWidth; j++)
        {
            var currentRoom = MapData[Floors - 2][j];
            if (currentRoom is not { NextRooms.Count: > 0 })
            {
                continue;
            }

            var arr = new Array<Room> { bossRoom };
            currentRoom.NextRooms = arr;
        }
    }
    

    /// <summary>
    /// 设置房间之间的连接关系，将当前房间与下一层的相邻房间建立连接。
    /// </summary>
    /// <param name="row">当前房间所在的行索引。</param>
    /// <param name="col">当前房间所在的列索引。</param>
    /// <returns>连接的下一个房间的列索引。</returns>
    /// <exception cref="InvalidOperationException">当地图数据为空时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当行或列索引超出有效范围时抛出。</exception>
    private int SetupConnection(int row, int col)
    {
        if (MapData == null || MapData.Count == 0)
        {
            throw new InvalidOperationException("MapData 是空的——先调用 GenerateInitialGrid（）。");
        }

        if (row < 0 || row >= MapData.Count - 1) // 不能为最后一层
        {
            throw new ArgumentOutOfRangeException(nameof(row));
        }

        if (col < 0 || col >= MapWidth)
        {
            throw new ArgumentOutOfRangeException(nameof(col));
        }

        Room nextRoom = null;
        var currentRoom = MapData[row][col];

        // 防止极端情况下死循环，设定最大尝试次数
        var attempts = 0;
        const int maxAttempts = 50;

        // 尝试找到一个合适的下一房间进行连接，避免路径交叉
        while ((nextRoom is null || WouldCrossExistingPath(row, col, nextRoom)) && attempts++ < maxAttempts)
        {
            var randomJ = Mathf.Clamp(GlobalBean.RandomNumberGenerator.RandiRange(col - 1, col + 1), 0, MapWidth - 1);
            nextRoom = MapData[row + 1][randomJ];
        }

        // 如果未找到合适的房间，则默认连接到正下方的房间
        nextRoom ??= MapData[row + 1][Mathf.Clamp(col, 0, MapWidth - 1)];

        // 双向连接：设置下一个房间的同时也设置上一个房间
        if (!currentRoom.NextRooms.Contains(nextRoom))
        {
            currentRoom.NextRooms.Add(nextRoom);
        }
    
        // 设置上一个房间列表
        if (!nextRoom.PreviousRooms.Contains(currentRoom))
        {
            nextRoom.PreviousRooms.Add(currentRoom);
        }

        return nextRoom.Column;
    }


    /// <summary>
    /// 检查在指定位置放置房间是否会与现有路径交叉。
    /// </summary>
    /// <param name="i">地图网格的行索引。</param>
    /// <param name="j">地图网格的列索引。</param>
    /// <param name="room">要放置的房间对象。</param>
    /// <returns>如果会与现有路径交叉则返回true，否则返回false。</returns>
    private bool WouldCrossExistingPath(int i, int j, Room room)
    {
        // 参数合法性校验
        if (i < 0 || i >= MapData.Count || j < 0 || j >= MapWidth)
            throw new ArgumentException("参数不合法！");

        // 确保 MapData[i] 不为空
        if (MapData[i] == null)
        {
            return false;
        }

        Room leftNeighbour = null;
        Room rightNeighbour = null;

        // 获取左右相邻的房间引用
        if (j > 0 && MapData[i][j - 1] != null)
        {
            leftNeighbour = MapData[i][j - 1];
        }

        if (j < MapWidth - 1 && MapData[i][j + 1] != null)
        {
            rightNeighbour = MapData[i][j + 1];
        }

        // 检查是否会与右侧房间的后续路径相交（假定 room.Column == j）
        if (rightNeighbour != null && room.Column > j &&
            rightNeighbour.NextRooms.Any(nextRoom => nextRoom.Column < room.Column))
        {
            return true;
        }

        // 检查是否会与左侧房间的后续路径相交（假定 room.Column == j）
        return leftNeighbour != null && room.Column < j &&
               leftNeighbour.NextRooms.Any(nextRoom => nextRoom.Column > room.Column);
    }


    /// <summary>
    /// 获取随机的起始点坐标数组。
    /// </summary>
    /// <returns>包含随机起始点Y坐标的整数数组。</returns>
    private Array<int> GetRandomStartingPoints()
    {
        var yCoordinates = new Array<int>();
        var uniquePoints = 0;

        // 循环生成随机起始点，直到至少有两个不同的点
        while (uniquePoints < 2)
        {
            uniquePoints = 0;
            yCoordinates = [];

            // 为每条路径生成一个随机的起始点
            for (var i = 0; i < Paths; i++)
            {
                var startingPoint = GlobalBean.RandomNumberGenerator.RandiRange(0, MapWidth - 1);
                if (!yCoordinates.Contains(startingPoint))
                {
                    uniquePoints++;
                }

                yCoordinates.Add(startingPoint);
            }
        }

        return yCoordinates;
    }


    /// <summary>
    /// 生成初始的房间网格结构。
    /// </summary>
    /// <returns>二维数组形式的房间网格，外层数组表示楼层，内层数组表示每层的房间。</returns>
    private Array<Array<Room>> GenerateInitialGrid()
    {
        var result = new Array<Array<Room>>();

        // 遍历每一层楼
        for (var i = 0; i < Floors; i++)
        {
            var floorRooms = new Array<Room>();

            // 遍历当前楼层的每一个房间位置
            for (var j = 0; j < MapWidth; j++)
            {
                // 创建新的房间实例并设置其属性
                var currentRoom = new Room();
                // 生成随机偏移量用于房间位置的随机化 placement randomness
                var offset = new Vector2(GD.Randf(), GD.Randf()) * PlacementRandomness;
                // 设置房间位置，基于网格坐标并添加随机偏移
                currentRoom.Position = new Vector2(j * XDist, i * -YDist) + offset;
                // 记录房间所在的行和列索引
                currentRoom.Row = i;
                currentRoom.Column = j;
                // 初始化下一个可到达的房间列表
                currentRoom.NextRooms = [];
                // 初始化上一个可到达的房间列表
                currentRoom.PreviousRooms = [];
                // 特殊处理最底层的房间位置
                if (i == Floors - 1)
                {
                    var pos = currentRoom.Position;
                    pos.Y = (i + 1) * -YDist;
                    currentRoom.Position = pos;
                }

                floorRooms.Add(currentRoom);
            }

            result.Add(floorRooms);
        }

        return result;
    }
}

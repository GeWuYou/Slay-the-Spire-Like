using Godot;
using Godot.Collections;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// Room类表示游戏中的一个房间，继承自Resource类。
/// 房间可以有不同的类型，如怪物房、宝箱房、篝火房等。
/// </summary>
public partial class Room : Resource
{
    /// <summary>
    /// 房间类型的枚举定义
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// 未知类型房间
        /// </summary>
        Unknown,

        /// <summary>
        /// 怪物房间 - 包含敌人战斗
        /// </summary>
        Monster,

        /// <summary>
        /// 宝藏房间 - 获得奖励和道具
        /// </summary>
        Treasure,

        /// <summary>
        /// 篝火房间 - 可以休息和升级卡牌
        /// </summary>
        Campfire,

        /// <summary>
        /// 商店房间 - 可以购买道具和卡牌
        /// </summary>
        Shop,

        /// <summary>
        /// Boss房间 - 与Boss进行战斗
        /// </summary>
        Boss,
    }

    /// <summary>
    /// 房间类型属性，用于标识当前房间的类型
    /// </summary>
    [Export]
    public Type RoomType { get; set; }

    /// <summary>
    /// 行索引属性，表示房间在网格中的行位置
    /// </summary>
    [Export]
    public int Row { get; set; }

    /// <summary>
    /// 列索引属性，表示房间在网格中的列位置
    /// </summary>
    [Export]
    public int Column { get; set; }

    /// <summary>
    /// 位置向量属性，表示房间在二维空间中的精确坐标
    /// </summary>
    [Export]
    public Vector2 Position { get; set; }

    /// <summary>
    /// 下一个房间数组属性，存储与当前房间相连的后续房间列表
    /// </summary>
    [Export]
    public Array<Room> NextRooms { get; set; }
    
    /// <summary>
    /// 上一个房间数组属性，存储与当前房间相连的上一个房间列表
    /// </summary>
    [Export]
    public Array<string> PreviousRoomKeys { get; set; }
    
    /// <summary>
    /// 选中状态属性，标识房间是否被选中
    /// </summary>
    [Export]
    public bool IsSelected { get; set; }

    [Export]
    public BattleStats BattleStats { get; set; }
    public override string ToString()
    {
        return
            $"Room(Type: {RoomType}, Position: [{Row}, {Column}], WorldPos: {Position}, NextRooms: {NextRooms?.Count ?? 0}, Selected: {IsSelected})";
    }
}
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.core.save;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.save;

/// <summary>
/// 存档数据类，用于保存游戏运行时的状态信息
/// 实现了ISaveAble接口，支持游戏状态的保存和恢复功能
/// </summary>
[GlobalClass]
public partial class SaveData : Resource, IGodotResourceSaveAble
{
    /// <summary>
    /// 获取或设置随机数生成器的种子值
    /// </summary>
    [Export]
    public ulong RngSeed { get; set; }
    
    /// <summary>
    /// 获取或设置随机数生成器的状态值
    /// </summary>
    [Export] 
    public ulong RngState { get; set; }

    /// <summary>
    /// 游戏运行统计信息
    /// </summary>
    [Export]
    public RunStats RunStats { get; set; }
    
    /// <summary>
    /// 玩家角色属性统计信息
    /// </summary>
    [Export]
    public CharacterStats PlayerStats { get; set; }
    
    /// <summary>
    /// 已获得的遗物列表
    /// </summary>
    [Export]
    public Array<Relic> Relics { get; set; }
    
    /// <summary>
    /// 地图数据，包含各层地图信息
    /// </summary>
    [Export]
    public Array<Array<Room>> MapData { get; set; }
    
    /// <summary>
    /// 上一个房间信息
    /// </summary>
    [Export]
    public Room LastRoom { get; set; }
    
    /// <summary>
    /// 已攀登的楼层数
    /// </summary>
    [Export]
    public int FloorsClimbed { get; set; }
    
    /// <summary>
    /// 是否在地图界面
    /// </summary>
    [Export]
    public bool WasOnMap { get; set; }
    
    /// <summary>
    /// 获取存档键名
    /// </summary>
    /// <returns>返回存档数据的键名字符串"saveData"</returns>
    public string GetSaveKey() => "saveData";

    public Resource CaptureState()
    {
        return this;
    }
}

using System;

namespace SlayTheSpireLike.scripts.save;

/// <summary>
/// 存档元数据类，用于存储游戏存档的基本信息
/// </summary>
public class SaveMeta
{
    /// <summary>
    /// 存档槽位ID，用于唯一标识一个存档位置
    /// </summary>
    public string SlotId { get; set; } = string.Empty;
    
    /// <summary>
    /// 存档显示名称，用于在用户界面中展示给玩家
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
    
    /// <summary>
    /// 存档创建时间，记录存档首次创建的日期和时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// 存档最后修改时间，记录存档最后一次被修改的日期和时间
    /// </summary>
    public DateTime LastModified { get; set; }
    
    /// <summary>
    /// 存档版本号，用于标识存档数据结构的版本，便于后续兼容性处理
    /// </summary>
    public string Version { get; set; } = "1.0.0";
    
    /// <summary>
    /// 游戏总游玩时间，以秒为单位记录玩家在此存档上的游戏时长
    /// </summary>
    public long PlayTimeSeconds { get; set; } = 0;
    
    /// <summary>
    /// 存档校验和，用于验证存档文件完整性和有效性
    /// </summary>
    public string Checksum { get; set; } = string.Empty; // 可能用于后续校验
}

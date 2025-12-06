using System.IO;
using Godot;
using SlayTheSpireLike.scripts.core.save;

namespace SlayTheSpireLike.scripts.save;

/// <summary>
///   保存管理器类，用于管理游戏数据的保存和加载
/// </summary>
public class SaveManager
{
    private readonly string _rootFolder;
    private readonly DefaultGodotResourceSaveSerializer _godotResourceSaveSerializer = new();

    /// <summary>
    /// 初始化保存管理器，创建保存文件夹路径
    /// </summary>
    public SaveManager()
    {
        // 尝试使用 Godot.OS.GetUserDataDir()，否则降级到 Environment
        var baseDir = OS.GetUserDataDir(); // 在 Godot 环境下
        _rootFolder = Path.Combine(baseDir, "saves");
        Directory.CreateDirectory(_rootFolder);
    }

    /// <summary>
    /// 保存游戏数据到指定存档槽位
    /// </summary>
    /// <param name="data">需要保存的游戏数据对象，必须实现IGodotResourceSaveAble接口</param>
    /// <param name="slotId">存档槽位ID，默认为"0"</param>
    /// <param name="saveFileName">保存文件名称</param>
    /// <returns>包含保存结果的SaveResult对象，其中T类型为Error枚举</returns>
    public void Save(IGodotResourceSaveAble data, string slotId = "0", string saveFileName = "saveData.tres")
    {
        var slotPath = Path.Combine(_rootFolder, slotId);
        // 确保存档槽位目录存在
        if (!Directory.Exists(slotPath))
        {
            Directory.CreateDirectory(slotPath);
        }

        var result = _godotResourceSaveSerializer.Serialize(data, Path.Combine(slotPath, saveFileName));
        GD.Print($"存档完成，结果：{result.Payload}");
    }


    /// <summary>
    /// 从指定存档槽位加载游戏数据
    /// </summary>
    /// <typeparam name="T">要加载的数据类型，必须继承自Resource类</typeparam>
    /// <param name="slotId">存档槽位ID，默认为"0"</param>
    /// <param name="saveFileName">保存文件名</param>
    /// <returns>加载的游戏数据对象</returns>
    public T Load<T>(string slotId = "0", string saveFileName = "saveData.tres") where T : Resource
    {
        return _godotResourceSaveSerializer.Deserialize<T>(Path.Combine(_rootFolder, slotId, saveFileName));
    }
    
    /// <summary>
    /// 删除指定存档槽位下的保存文件
    /// </summary>
    /// <param name="slotId">存档槽位ID，默认为"0"</param>
    /// <param name="saveFileName">保存文件名</param>
    public void Delete(string slotId = "0", string saveFileName = "saveData.tres")
    {
        var slotPath = Path.Combine(_rootFolder, slotId, saveFileName);
        if (File.Exists(slotPath))
        {
            File.Delete(slotPath);
        }
    }

    /// <summary>
    /// 检查指定存档槽位是否存在保存文件
    /// </summary>
    /// <param name="slotId">存档槽位ID，默认为"0"</param>
    /// <param name="saveFileName">保存文件名</param>
    /// <returns>是否存在保存文件</returns>
    public bool Exists(string slotId = "0", string saveFileName = "saveData.tres")
    {
        var slotPath = Path.Combine(_rootFolder, slotId, saveFileName);
        return File.Exists(slotPath);
    }
}
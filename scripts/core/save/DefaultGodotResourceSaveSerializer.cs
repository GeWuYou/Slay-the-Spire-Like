using Godot;

namespace SlayTheSpireLike.scripts.save;

/// <summary>
/// 默认的Godot资源保存序列化器，用于序列化和反序列化ISaveAble对象到Godot Resource格式
/// </summary>
public class DefaultGodotResourceSaveSerializer: IGodotResourceSaveSerializer
{
    /// <summary>
    /// 将可保存的数据序列化并保存到指定路径
    /// </summary>
    /// <param name="data">要保存的可序列化数据对象，必须实现ISaveAble接口且数据类型为Resource</param>
    /// <param name="path">保存文件的目标路径</param>
    /// <returns>包含保存操作结果的SaveResult对象，其中泛型参数为Error类型表示可能的错误信息</returns>
    public SaveResult<Error> Serialize(ISaveAble<Resource> data, string path)
    {
        return SaveResult<Error>.Of(ResourceSaver.Save(data.CaptureState(), path));
    }

    /// <summary>
    /// 从指定路径反序列化加载Resource资源
    /// </summary>
    /// <param name="path">要加载的资源文件路径</param>
    /// <returns>如果文件存在则返回加载的Resource对象，否则返回null</returns>
    public Resource Deserialize(string path)
    {
        // 检查文件是否存在，存在则加载资源，不存在则返回null
        return FileAccess.FileExists(path) ? ResourceLoader.Load(path) : null;
    }
    
    /// <summary>
    /// 从指定路径反序列化加载指定类型的Resource资源
    /// </summary>
    /// <typeparam name="T">要加载的资源类型，必须继承自Resource</typeparam>
    /// <param name="path">要加载的资源文件路径</param>
    /// <returns>如果文件存在则返回指定类型的Resource对象，否则返回null</returns>
    public T Deserialize<T>(string path) where T : Resource
    {
        // 检查文件是否存在，存在则加载指定类型的资源，不存在则返回null
        return FileAccess.FileExists(path) ? ResourceLoader.Load<T>(path) : null;
    }
}

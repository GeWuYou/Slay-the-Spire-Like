using SlayTheSpireLike.scripts.core.save;

namespace SlayTheSpireLike.scripts.save;

/// <summary>
/// 定义数据序列化和反序列化的接口，用于保存和加载游戏数据
/// </summary>
/// <typeparam name="T">序列化操作的结果类型</typeparam>
/// <typeparam name="TD">需要保存的数据类型</typeparam>
public interface ISaveSerializer<T,TD>
{
    /// <summary>
    /// 将指定数据序列化并保存到指定路径
    /// </summary>
    /// <param name="data">需要序列化的数据对象，必须实现ISaveAble接口</param>
    /// <param name="path">数据保存的文件路径</param>
    /// <returns>包含序列化结果的SaveResult对象</returns>
    public SaveResult<T> Serialize(ISaveAble<TD> data, string path);

    /// <summary>
    /// 从指定路径反序列化数据
    /// </summary>
    /// <param name="path">数据文件的路径</param>
    /// <returns>反序列化后的数据对象</returns>
    public TD Deserialize(string path);
}

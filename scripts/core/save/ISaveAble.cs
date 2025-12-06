namespace SlayTheSpireLike.scripts.save;

/// <summary>
/// 定义了可保存对象的接口，用于捕获和恢复对象的状态
/// </summary>
/// <typeparam name="T">状态数据的类型</typeparam>
public interface ISaveAble<out T>
{

    /// <summary>
    /// 捕获当前对象的状态并返回状态数据
    /// </summary>
    /// <returns>表示当前对象状态的数据</returns>
    T CaptureState();
}

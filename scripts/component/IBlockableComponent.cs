namespace SlayTheSpireLike.scripts.component;

/// <summary>
/// 定义可阻挡组件的接口，用于处理阻挡效果
/// </summary>
public interface IBlockableComponent
{
    /// <summary>
    /// 接受并处理阻挡效果
    /// </summary>
    /// <param name="block">阻挡值，表示需要处理的阻挡量</param>
    void TakeBlock(int block);
}

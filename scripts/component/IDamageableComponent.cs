namespace SlayTheSpireLike.scripts.component;

/// <summary>
/// 定义可受到伤害的游戏对象组件接口
/// </summary>
public interface IDamageableComponent
{
    /// <summary>
    /// 处理对象受到的伤害
    /// </summary>
    /// <param name="damage">伤害值，必须为非负整数</param>
    void TakeDamage(int damage);
}

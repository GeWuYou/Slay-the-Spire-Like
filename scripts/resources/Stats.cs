using Godot;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// 表示一个角色或实体的基本属性（如生命值、护盾等）的数据资源。
/// 提供了健康值和护盾值的管理逻辑，并支持伤害与治疗操作。
/// 当属性发生变化时会发出信号通知监听者。
/// </summary>
[GlobalClass]
public partial class Stats : Resource
{
    /// <summary>
    /// 属性变化事件。当 Health 或 Block 发生变化时触发。
    /// </summary>
    [Signal]
    public delegate void StatsChangedEventHandler();

    /// <summary>
    /// 最大生命值上限，默认为 70。
    /// </summary>
    [Export]
    public int MaxHeath { set; get; } = 70;

    /// <summary>
    /// 最大护盾值上限，默认为 999。
    /// </summary>
    [Export]
    public int MaxBlock { set; get; } = 999;

    /// <summary>
    /// 角色的艺术资源纹理。
    /// </summary>
    [Export]
    public Texture Art { private set; get; }

    private int _health;
    private int _block;

    /// <summary>
    /// 获取或设置当前的生命值。
    /// 设置时将自动限制在 [0, MaxHeath] 范围内。
    /// 若值发生更改，则会触发 StatsChanged 信号。
    /// </summary>
    public int Health
    {
        get => _health;
        set
        {
            // 限制生命值在合理范围内
            var newHealth = Mathf.Clamp(value, 0, MaxHeath);
            if (_health == newHealth)
            {
                return;
            }

            _health = newHealth;
            EmitSignal(SlayTheSpireLike.scripts.resources.Stats.SignalName.StatsChanged);
        }
    }

    /// <summary>
    /// 获取或设置当前的护盾值。
    /// 设置时将自动限制在 [0, MaxBlock] 范围内。
    /// 若值发生更改，则会触发 StatsChanged 信号。
    /// </summary>
    public int Block
    {
        set
        {
            var newBlock = Mathf.Clamp(value, 0, MaxBlock);
            if (_block == newBlock)
            {
                return;
            }

            _block = newBlock;
            EmitSignal(SlayTheSpireLike.scripts.resources.Stats.SignalName.StatsChanged);
        }
        get => _block;
    }

    /// <summary>
    /// 对该对象造成指定数值的伤害。
    /// 护盾优先抵消伤害，剩余部分扣除生命值。
    /// 忽略小于等于零的伤害输入。
    /// </summary>
    /// <param name="damage">要造成的伤害点数。</param>
    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        var initialDamage = damage;
        damage = Mathf.Clamp(damage - Block, 0, damage);
        Block = Mathf.Clamp(Block - initialDamage, 0, Block);
        Health -= damage;
    }

    /// <summary>
    /// 恢复指定数量的生命值。
    /// 生命值不会超过最大生命值上限。
    /// 忽略小于等于零的恢复量。
    /// </summary>
    /// <param name="amount">要恢复的生命值点数。</param>
    public void Heal(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Health = Mathf.Clamp(Health + amount, 0, MaxHeath);
    }

    /// <summary>
    /// 创建资源实例
    /// </summary>
    /// <returns>返回创建的资源实例</returns>
    public virtual Stats CreateInstance()
    {
        return CreateInstance<Stats>();
    }

    /// <summary>
    /// 创建此 Stats 实例的一个副本并初始化其 Health 值为 MaxHeath。
    /// </summary>
    /// <returns>一个新的 Stats 实例；如果复制失败则返回 null 并打印错误信息。</returns>
    public T CreateInstance<T>() where T : Stats, new()
    {
        var instance = new T();
        // 复制属性值
        instance.Art = Art;
        instance.Health = MaxHeath;
        instance.Block = 0;
        return instance;
    }
}
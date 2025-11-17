using Godot;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// 敌人属性统计类，继承自基础属性类Stats
/// 用于定义游戏中敌人的各项属性和AI行为
/// </summary>
[GlobalClass]
public partial class EnemyStats : Stats
{
    /// <summary>
    /// 敌人的人工智能场景引用
    /// 用于实例化敌人的AI行为逻辑
    /// </summary>
    [Export]
    public PackedScene Ai { get; set; }
    
    /// <summary>
    /// 敌人的伤害值属性
    /// 表示敌人攻击时造成的伤害点数
    /// </summary>
    [Export]
    public int Damage { get; set; }

    /// <summary>
    /// 创建一个新的EnemyStats实例，并复制当前对象的AI和伤害属性值到新实例中
    /// </summary>
    /// <returns>返回一个配置了相同AI和伤害值的新EnemyStats实例</returns>
    public override EnemyStats CreateInstance()
    {
        // 调用基类方法创建新的EnemyStats实例
        var instance = base.CreateInstance<EnemyStats>();
        // 复制AI和伤害属性到新实例
        instance.Ai = Ai;
        instance.Damage = Damage;
        return instance;
    }

}

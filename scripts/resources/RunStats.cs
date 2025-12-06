using Godot;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
/// 运行状态资源类，用于跟踪游戏运行期间的统计信息
/// </summary>
[GlobalClass]
public partial class RunStats : Resource
{
    [Export]
    private int _gold = 700;

    /// <summary>
    /// 基础卡牌奖励数值常量
    /// </summary>
    public const int BaseCardRewardsValue = 3;

    /// <summary>
    /// 基础普通卡牌权重值常量
    /// </summary>
    public const float BaseCommonWeightValue = 6.0f;

    /// <summary>
    /// 基础稀有卡牌权重值常量
    /// </summary>
    public const float BaseUncommonWeightValue = 3.7f;

    /// <summary>
    /// 基础罕见卡牌权重值常量
    /// </summary>
    public const float BaseRareWeightValue = 0.3f;


    /// <summary>
    /// 基础卡牌奖励数量属性
    /// 用于设置或获取玩家获得的基础卡牌奖励数量
    /// 默认值为3
    /// </summary>
    [Export]
    public int BaseCardRewards { get; set; } = 3;

    /// <summary>
    /// 基础普通卡牌权重属性
    /// 用于设置或获取普通稀有度卡牌的生成权重
    /// 默认值为6.0f
    /// </summary>
    [Export(PropertyHint.Range, "0,10")]
    public float BaseCommonWeight { get; set; } = 6.0f;

    /// <summary>
    /// 基础罕见卡牌权重属性
    /// 用于设置或获取罕见稀有度卡牌的生成权重
    /// 默认值为3.7f
    /// </summary>
    [Export(PropertyHint.Range, "0,10")]
    public float BaseUncommonWeight { get; set; } = 3.7f;

    /// <summary>
    /// 基础稀有卡牌权重属性
    /// 用于设置或获取稀有稀有度卡牌的生成权重
    /// 默认值为0.3f
    /// </summary>
    [Export(PropertyHint.Range, "0,10")]
    public float BaseRareWeight { get; set; } = 0.3f;

    /// <summary>
    /// 金币变化事件处理器委托
    /// 当金币数量发生变化时触发此信号
    /// </summary>
    [Signal]
    public delegate void GoldChangedEventHandler();


    /// <summary>
    /// 获取或设置当前金币数量
    /// 当金币数量被修改时，会自动触发GoldChanged信号通知监听者
    /// </summary>
    [Export]
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            // 发送金币变化信号通知UI等组件更新显示
            EmitSignal(SignalName.GoldChanged);
        }
    }

    /// <summary>
    /// 将所有卡牌权重和基础奖励重置为默认值
    /// </summary>
    public void ResetWeights()
    {
        BaseCommonWeight = BaseCommonWeightValue;
        BaseUncommonWeight = BaseUncommonWeightValue;
        BaseRareWeight = BaseRareWeightValue;
        BaseCardRewards = BaseCardRewardsValue;
    }
}

using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;

namespace SlayTheSpireLike.scripts.random;

/// <summary>
/// 随机数提供器类，用于生成和管理随机数以及提供基于随机数的数组操作功能
/// </summary>
public class RandomNumberProvider
{
    /// <summary>
    /// 获取随机数提供器的单例实例
    /// </summary>
    public static RandomNumberProvider Instance { get; } = new();
    
    /// <summary>
    /// 获取随机数生成器实例
    /// </summary>
    public RandomNumberGenerator RandomNumberGenerator { get;private set; }
    
    /// <summary>
    /// 初始化随机数提供器实例
    /// </summary>
    public RandomNumberProvider()
    {
        Initialize();
    }

    /// <summary>
    /// 初始化随机数生成器并进行随机化处理
    /// </summary>
    private void Initialize()
    {
        RandomNumberGenerator = new RandomNumberGenerator();
        RandomNumberGenerator.Randomize();
    }

    /// <summary>
    /// 根据指定的种子和状态设置新的随机数生成器
    /// </summary>
    /// <param name="whichSeed">要设置的随机数种子，默认值为0</param>
    /// <param name="state">要设置的随机数状态，默认值为0</param>
    public void SetRandomNumberGeneratorBySeedAndState(ulong whichSeed = 0, ulong state = 0)
    {
        RandomNumberGenerator = new RandomNumberGenerator();
        RandomNumberGenerator.Seed = whichSeed;
        RandomNumberGenerator.State = state;
    }
    
    /// <summary>
    /// 使用Fisher-Yates洗牌算法对数组进行随机打乱
    /// </summary>
    /// <param name="array">要进行随机打乱的数组</param>
    public void ArrayShuffle <[MustBeVariant]T>(Array<T> array)
    {
        // 如果数组元素少于2个，无需打乱
        if (array.Count < 2)
        {
            return;
        }

        // Fisher-Yates洗牌算法实现
        for (var i = array.Count - 1; i > 0; i--)
        {
            var j = (int)(RandomNumberGenerator.Randi() % (i + 1));
            // 交换元素
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
    
}

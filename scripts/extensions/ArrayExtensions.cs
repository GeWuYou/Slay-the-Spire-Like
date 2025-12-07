
using System;
using System.Collections.Generic;
using SlayTheSpireLike.scripts.global;
using SlayTheSpireLike.scripts.random;

namespace SlayTheSpireLike.scripts.extensions;


/// <summary>
/// 数组扩展方法类，提供对数组和列表的扩展功能
/// </summary>
public static class ArrayExtensions
{

    /// <summary>
    /// 从只读列表中随机选择一个元素
    /// </summary>
    /// <typeparam name="T">列表元素的类型</typeparam>
    /// <param name="list">要从中选择元素的只读列表</param>
    /// <returns>从列表中随机选择的元素</returns>
    /// <exception cref="InvalidOperationException">当列表为null或为空时抛出异常</exception>
    public static T PickRandom<T>(this IReadOnlyList<T> list)
    {
        // 检查列表是否为空或null，如果是则抛出异常
        if (list == null || list.Count == 0)
            throw new InvalidOperationException("Cannot pick a random element from an empty list.");

        // 使用全局随机数生成器在列表索引范围内生成随机索引，并返回对应元素
        return list[RandomNumberProvider.Instance.RandomNumberGenerator.RandiRange(0, list.Count - 1)];
    }
}
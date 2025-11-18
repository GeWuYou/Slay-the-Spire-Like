using System;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.global;

/// <summary>
///     资源工厂类，用于集中管理各种资源的实例化工厂
/// </summary>
public static class ResourceFactories
{
    /// <summary>
    ///     卡牌UI工厂函数
    /// </summary>
    public static readonly Func<CardUi> CardUiFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<CardUi>(GameConstants.ResourcePaths.CardUiScene);
}
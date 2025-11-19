using System;
using Godot;
using SlayTheSpireLike.scripts.resources;
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

    /// <summary>
    /// 角色选择器工厂函数，用于创建CharacterSelector实例
    /// </summary>
    /// <returns>返回一个新的CharacterSelector对象实例</returns>
    /// <remarks>
    /// 该工厂函数通过ResourceLoaderManager注册并获取CharacterSelector场景的工厂方法，
    /// 使用GameConstants中定义的角色选择器场景资源路径进行初始化
    /// </remarks>
    public static readonly Func<CharacterSelector> CharacterSelectorFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<CharacterSelector>(
            GameConstants.ResourcePaths.CharacterSelectorScene);


    /// <summary>
    ///     白色精灵材质工厂函数
    /// </summary>
    public static readonly Func<Material> WhiteSpriteMatFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Material>(
            GameConstants.ResourcePaths.WhiteSpriteMaterial, false);

    /// <summary>
    /// 刺客角色属性工厂函数，用于创建刺客角色的基础属性对象
    /// </summary>
    /// <returns>返回一个新的刺客角色属性实例</returns>
    public static readonly Func<CharacterStats> AssassinStatsFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<CharacterStats>(
            GameConstants.ResourcePaths.AssassinStats);

    /// <summary>
    /// 战士角色属性工厂函数，用于创建战士角色的基础属性对象
    /// </summary>
    /// <returns>返回一个新的战士角色属性实例</returns>
    public static readonly Func<CharacterStats> WarriorStatsFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<CharacterStats>(
            GameConstants.ResourcePaths.WarriorStats);

    /// <summary>
    /// 法师角色属性工厂函数，用于创建法师角色的基础属性对象
    /// </summary>
    /// <returns>返回一个新的法师角色属性实例</returns>
    public static readonly Func<CharacterStats> WizardStatsFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<CharacterStats>(
            GameConstants.ResourcePaths.WizardStats);
}
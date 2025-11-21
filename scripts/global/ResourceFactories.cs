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
    ///     角色选择器工厂函数，用于创建CharacterSelector实例
    /// </summary>
    /// <returns>返回一个新的CharacterSelector对象实例</returns>
    /// <remarks>
    ///     该工厂函数通过ResourceLoaderManager注册并获取CharacterSelector场景的工厂方法，
    ///     使用GameConstants中定义的角色选择器场景资源路径进行初始化
    /// </remarks>
    public static readonly Func<CharacterSelector> CharacterSelectorFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<CharacterSelector>(
            GameConstants.ResourcePaths.CharacterSelectorScene);


    /// <summary>
    ///     战斗场景工厂函数，用于创建Battle场景实例
    /// </summary>
    /// <returns>返回一个新的Battle场景对象实例</returns>
    public static readonly Func<Node> BattleSceneFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<Node>(
            GameConstants.ResourcePaths.BattleScene);

    /// <summary>
    ///     战斗奖励场景工厂函数，用于创建BattleReward场景实例
    /// </summary>
    /// <returns>返回一个新的BattleReward场景对象实例</returns>
    public static readonly Func<Node> BattleRewardSceneFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<Node>(
            GameConstants.ResourcePaths.BattleRewardScene);

    /// <summary>
    ///     营火场景工厂函数，用于创建Campfire场景实例
    /// </summary>
    /// <returns>返回一个新的Campfire场景对象实例</returns>
    public static readonly Func<Node> CampfireSceneFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<Node>(
            GameConstants.ResourcePaths.CampfireScene);

    /// <summary>
    ///     地图场景工厂函数，用于创建Map场景实例
    /// </summary>
    /// <returns>返回一个新的Map场景对象实例</returns>
    public static readonly Func<Node> MapSceneFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<Node>(
            GameConstants.ResourcePaths.MapScene);

    /// <summary>
    ///     商店场景工厂函数，用于创建Shop场景实例
    /// </summary>
    /// <returns>返回一个新的Shop场景对象实例</returns>
    public static readonly Func<Node> ShopSceneFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<Node>(
            GameConstants.ResourcePaths.ShopScene);

    /// <summary>
    ///     宝箱场景工厂函数，用于创建Treasure场景实例
    /// </summary>
    /// <returns>返回一个新的Treasure场景对象实例</returns>
    public static readonly Func<Node> TreasureSceneFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<Node>(
            GameConstants.ResourcePaths.TreasureScene);
    
    /// <summary>
    ///     卡牌菜单UI工厂函数，用于创建CardMenuUi场景实例
    /// </summary>
    /// <returns>返回一个新的CardMenuUi场景对象实例</returns>
    public static readonly Func<CardMenuUi> CardMenuUiFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<CardMenuUi>(
            GameConstants.ResourcePaths.CardMenuUiScene);
    

    /// <summary>
    ///     白色精灵材质工厂函数
    /// </summary>
    public static readonly Func<Material> WhiteSpriteMatFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Material>(
            GameConstants.ResourcePaths.WhiteSpriteMaterial, false);

    /// <summary>
    ///     刺客角色属性工厂函数，用于创建刺客角色的基础属性对象
    /// </summary>
    /// <returns>返回一个新的刺客角色属性实例</returns>
    public static readonly Func<CharacterStats> AssassinStatsFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<CharacterStats>(
            GameConstants.ResourcePaths.AssassinStats);

    /// <summary>
    ///     战士角色属性工厂函数，用于创建战士角色的基础属性对象
    /// </summary>
    /// <returns>返回一个新的战士角色属性实例</returns>
    public static readonly Func<CharacterStats> WarriorStatsFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<CharacterStats>(
            GameConstants.ResourcePaths.WarriorStats);

    /// <summary>
    ///     法师角色属性工厂函数，用于创建法师角色的基础属性对象
    /// </summary>
    /// <returns>返回一个新的法师角色属性实例</returns>
    public static readonly Func<CharacterStats> WizardStatsFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<CharacterStats>(
            GameConstants.ResourcePaths.WizardStats);
    
    /// <summary>
    ///     卡片基础样式盒子工厂函数，用于创建卡片控件的基础样式对象
    /// </summary>
    /// <returns>返回一个新的卡片基础样式盒子实例</returns>
    public static readonly Func<StyleBox> CardBaseStyleBoxFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<StyleBox>(
            GameConstants.ResourcePaths.CardBaseStyleBox);
    
    /// <summary>
    ///     卡片悬停样式盒子工厂函数，用于创建卡片控件在鼠标悬停状态下的样式对象
    /// </summary>
    /// <returns>返回一个新的卡片悬停样式盒子实例</returns>
    public static readonly Func<StyleBox> CardHoverStyleBoxFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<StyleBox>(
            GameConstants.ResourcePaths.CardHoverStyleBox);

}
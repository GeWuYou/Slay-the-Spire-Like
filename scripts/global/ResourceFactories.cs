using System;
using Godot;
using SlayTheSpireLike.scripts.map;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.shop;
using SlayTheSpireLike.scripts.status_handler;
using SlayTheSpireLike.scripts.ui;
using SlayTheSpireLike.scripts.ui.components;
using SlayTheSpireLike.scripts.win;
using Status = SlayTheSpireLike.scripts.statuses.Status;

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
    ///     属性项工厂函数，用于创建StatItem实例
    /// </summary>
    /// <returns>返回一个新的StatItem对象实例</returns>
    /// <remarks>
    ///     该工厂函数通过ResourceLoaderManager注册并获取StatItem场景的工厂方法，
    ///     使用GameConstants中定义的属性项场景资源路径进行初始化
    /// </remarks>
    public static readonly Func<StatItem> StatItemFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<StatItem>(
            GameConstants.ResourcePaths.StatItem);



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
    ///     奖励按钮工厂函数，用于创建奖励按钮实例
    /// </summary>
    /// <returns>返回一个新的RewardButton实例</returns>
    public static readonly Func<RewardButton> RewardButtonFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<RewardButton>(
            GameConstants.ResourcePaths.BattleRewardButtonScene);

    /// <summary>
    ///     卡牌奖励UI工厂函数，用于创建CardRewards场景实例
    /// </summary>
    /// <returns>返回一个新的CardRewards场景对象实例</returns>
    public static readonly Func<CardRewards> CardRewardsFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<CardRewards>(
            GameConstants.ResourcePaths.CardRewardsScene);

    /// <summary>
    ///     地图房间UI工厂函数，用于创建MapRoom场景实例
    /// </summary>
    /// <returns>返回一个新的MapRoom场景对象实例</returns>
    public static readonly Func<MapRoom> MapRoomFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<MapRoom>(
            GameConstants.ResourcePaths.MapRoomScene);
    
    public static readonly Func<WinScreen> WinScreenFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<WinScreen>(
            GameConstants.ResourcePaths.WinScreenScene);

    /// <summary>
    ///     地图连线UI工厂函数，用于创建Line2D场景实例
    /// </summary>
    /// <returns>返回一个新的Line2D场景对象实例</returns>
    public static readonly Func<Line2D> MapLineFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<Line2D>(
            GameConstants.ResourcePaths.MapLineScene);

    /// <summary>
    /// 金币纹理工厂函数，用于加载金币纹理资源
    /// </summary>
    /// <returns>返回金币纹理的Texture2D对象</returns>
    public static readonly Func<Texture2D> GoldTextureFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Texture2D>(
            GameConstants.ResourcePaths.GoldTexture);

    /// <summary>
    /// 卡牌纹理工厂函数，用于加载卡牌纹理资源
    /// </summary>
    /// <returns>返回卡牌纹理的Texture2D对象</returns>
    public static readonly Func<Texture2D> CardTextureFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Texture2D>(
            GameConstants.ResourcePaths.CardTexture);
    
    /// <summary>
    ///  力量形态图标
    /// </summary>
    public static readonly Func<Status> TrueStrengthFormStatusFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Status>(
            GameConstants.ResourcePaths.TrueStrengthFormStatus,true);
    /// <summary>
    ///  肌肉状态资源工厂函数，用于加载和创建肌肉状态资源
    /// </summary>
    public static readonly Func<Status> MuscleStatusFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Status>(
            GameConstants.ResourcePaths.MuscleStatus,true);

    /// <summary>
    ///  exposes 状态资源工厂函数，用于加载和创建 exposes 状态资源
    /// </summary>
    public static readonly Func<Status> ExposedStatusFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Status>(
            GameConstants.ResourcePaths.ExposedStatus,true);
            
    /// <summary>
    /// 怪物纹理资源工厂函数，用于加载和创建怪物纹理资源
    /// </summary>
    /// <returns>返回一个Texture2D类型的纹理资源</returns>
    public static readonly Func<Texture2D> MonsterFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Texture2D>(
            GameConstants.ResourcePaths.MonsterTexture);

    /// <summary>
    /// 宝藏纹理资源工厂函数，用于加载和创建宝藏纹理资源
    /// </summary>
    /// <returns>返回一个Texture2D类型的纹理资源</returns>
    public static readonly Func<Texture2D> TreasureFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Texture2D>(
            GameConstants.ResourcePaths.TreasureTexture);

    /// <summary>
    /// 营火纹理资源工厂函数，用于加载和创建营火纹理资源
    /// </summary>
    /// <returns>返回一个Texture2D类型的纹理资源</returns>
    public static readonly Func<Texture2D> CampfireFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Texture2D>(
            GameConstants.ResourcePaths.CampfireTexture);

    /// <summary>
    /// 商店纹理资源工厂函数，用于加载和创建商店纹理资源
    /// </summary>
    /// <returns>返回一个Texture2D类型的纹理资源</returns>
    public static readonly Func<Texture2D> ShopFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Texture2D>(
            GameConstants.ResourcePaths.ShopTexture);

    /// <summary>
    /// Boss纹理资源工厂函数，用于加载和创建Boss纹理资源
    /// </summary>
    /// <returns>返回一个Texture2D类型的纹理资源</returns>
    public static readonly Func<Texture2D> BossFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Texture2D>(
            GameConstants.ResourcePaths.BossTexture);
    
    /// <summary>
    /// 状态UI场景资源工厂函数，用于加载和创建状态UI场景资源
    /// </summary>
    /// <returns>返回一个StatusUi类型的场景资源</returns>
    public static readonly Func<StatusUi> StatusUiFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<StatusUi>(
            GameConstants.ResourcePaths.StatusUiScene);

    /// <summary>
    /// 状态提示UI场景资源工厂函数，用于加载和创建状态提示UI场景资源
    /// </summary>
    public static readonly Func<StatusTooltip> StatusTooltipFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<StatusTooltip>(
            GameConstants.ResourcePaths.StatusTooltip);
    
    /// <summary>
    ///     遗物UI场景资源工厂函数，用于加载和创建遗物UI场景资源
    /// </summary>
    public static readonly Func<RelicUi> RelicUiFactory =
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<RelicUi>(
            GameConstants.ResourcePaths.RelicUiScene);
    
    public static readonly Func<ShopCard> ShopCardFactory = 
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<ShopCard>(
            GameConstants.ResourcePaths.ShopCardScene);
        
    public static readonly Func<ShopRelic> ShopRelicFactory = 
        ResourceLoaderManager.Instance.GetOrRegisterSceneFactory<ShopRelic>(
            GameConstants.ResourcePaths.ShopRelicScene);
    /// <summary>
    ///     白色精灵材质工厂函数
    /// </summary>
    public static readonly Func<Material> WhiteSpriteMatFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Material>(
            GameConstants.ResourcePaths.WhiteSpriteMaterial);

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
    
    public static readonly Func<Card> ToxinCardFactory =
        ResourceLoaderManager.Instance.GetOrRegisterResourceFactory<Card>(
            GameConstants.ResourcePaths.ToxinCard,true);
}
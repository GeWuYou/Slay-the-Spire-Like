namespace SlayTheSpireLike.scripts.global;

/// <summary>
///     游戏中的全局常量定义类
///     包含各种组名、标签和其他常用字符串常量
/// </summary>
public static class GameConstants
{
    /// <summary>
    ///     节点组相关常量
    /// </summary>
    public static class Groups
    {
        /// <summary>
        ///     玩家组名
        /// </summary>
        public const string Player = "player";

        /// <summary>
        ///     敌人组名
        /// </summary>
        public const string Enemies = "enemies";

        /// <summary>
        ///     盟友组名
        /// </summary>
        public const string Allies = "allies";
    }

    /// <summary>
    ///     信号相关常量
    /// </summary>
    public static class Signals
    {
        /// <summary>
        ///     准备就绪信号
        /// </summary>
        public const string Ready = "ready";
    }

    /// <summary>
    ///     资源路径常量
    /// </summary>
    public static class ResourcePaths
    {
        /// <summary>
        ///     卡牌UI场景路径
        /// </summary>
        public const string CardUiScene = "res://scenes/ui/card_ui.tscn";

        /// <summary>
        ///     选择角色场景路径
        /// </summary>
        public const string CharacterSelectorScene = "res://scenes/ui/character_selector.tscn";

        /// <summary>
        ///     战斗场景路径
        /// </summary>
        public const string BattleScene = "res://scenes/battle/battle.tscn";

        /// <summary>
        ///     战斗奖励场景路径
        /// </summary>
        public const string BattleRewardScene = "res://scenes/battle/battle_reward.tscn";

        /// <summary>
        ///     营火场景路径
        /// </summary>
        public const string CampfireScene = "res://scenes/campfire/campfire.tscn";

        /// <summary>
        ///     地图场景路径
        /// </summary>
        public const string MapScene = "res://scenes/map/map.tscn";

        /// <summary>
        ///     商店场景路径
        /// </summary>
        public const string ShopScene = "res://scenes/shop/shop.tscn";

        /// <summary>
        ///     宝箱场景路径
        /// </summary>
        public const string TreasureScene = "res://scenes/room/treasure/treasure.tscn";

        /// <summary>
        ///     白色精灵材质路径
        /// </summary>
        public const string WhiteSpriteMaterial = "res://art/white_sprite_material.tres";


        /// <summary>
        ///     刺客角色属性配置文件路径
        /// </summary>
        public const string AssassinStats = "res://resources/characters/assassin/assassin.tres";

        /// <summary>
        ///     战士角色属性配置文件路径
        /// </summary>
        public const string WarriorStats = "res://resources/characters/warrior/warrior.tres";

        /// <summary>
        ///     法师角色属性配置文件路径
        /// </summary>
        public const string WizardStats = "res://resources/characters/wizard/wizard.tres";
    }
}
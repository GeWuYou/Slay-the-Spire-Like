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
    /// 文本常量类，用于存储游戏中使用的固定文本字符串
    /// </summary>
    public static class Texts
    {
        /// <summary>
        /// 金币数量格式化字符串，用于显示玩家当前金币数量
        /// </summary>
        public const string Gold = "{0} 金币";

        /// <summary>
        /// 添加新卡牌文本常量，用于界面按钮或操作提示
        /// </summary>
        public const string AddNewCard = "添加新卡牌";
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
        public const string WinScreenScene = "res://scenes/win/win_screen.tscn";
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
        ///     运行场景路径
        /// </summary>
        public const string RunScene = "res://scenes/run/run.tscn";

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
        ///     卡牌菜单UI场景路径
        /// </summary>
        public const string CardMenuUiScene = "res://scenes/ui/card_menu_ui.tscn";

        /// <summary>
        /// 战斗奖励场景资源路径常量
        /// </summary>
        public const string CardRewardsScene = "res://scenes/ui/card_rewards.tscn";

        /// <summary>
        /// 战斗奖励按钮场景资源路径常量
        /// </summary>
        public const string BattleRewardButtonScene = "res://scenes/ui/reward_button.tscn";

        /// <summary>
        /// 地图房间场景资源路径常量
        /// </summary>
        public const string MapRoomScene = "res://scenes/map/map_room.tscn";

        /// <summary>
        /// 地图连线场景资源路径常量
        /// </summary>
        public const string MapLineScene = "res://scenes/map/map_line.tscn";

        /// <summary>
        /// 统计项控件资源路径常量
        /// </summary>
        public const string StatItem = "res://scenes/ui/components/stat_item.tscn";

        /// <summary>
        /// 状态处理器资源路径常量
        /// </summary>
        public const string StatusUiScene = "res://scenes/status_handler/status_ui.tscn";
        
        /// <summary>
        /// 状态提示框资源路径常量
        /// </summary>
        public const string StatusTooltip = "res://scenes/status_handler/status_tooltip.tscn";
        
        /// <summary>
        /// 遗物处理器资源路径常量
        /// </summary>
        public const string RelicUiScene = "res://scenes/relic_handler/relic_ui.tscn";

        public const string ShopCardScene = "res://scenes/shop/商店卡牌.tscn";

        public const string ShopRelicScene = "res://scenes/shop/商店遗物.tscn";
        
        public const string MainMenuScene = "res://scenes/ui/main_menu.tscn";
        /// <summary>
        /// 金币纹理资源路径常量
        /// </summary>
        public const string GoldTexture = "res://art/gold.png";


        /// <summary>
        /// 卡牌纹理资源路径常量
        /// </summary>
        public const string CardTexture = "res://art/rarity.png";

        /// <summary>
        /// 怪物图标纹理资源路径常量
        /// </summary>
        public const string MonsterTexture = "res://art/tile_0103.png";

        /// <summary>
        /// 宝藏图标纹理资源路径常量
        /// </summary>
        public const string TreasureTexture = "res://art/tile_0089.png";

        /// <summary>
        /// 营火图标纹理资源路径常量
        /// </summary>
        public const string CampfireTexture = "res://art/player_heart.png";

        /// <summary>
        /// 商店图标纹理资源路径常量
        /// </summary>
        public const string ShopTexture = "res://art/gold.png";

        /// <summary>
        /// Boss图标纹理资源路径常量
        /// </summary>
        public const string BossTexture = "res://art/tile_0105.png";

        /// <summary>
        ///   true_strength_from 状态资源路径常量
        /// </summary>
        public const string TrueStrengthFormStatus = "res://resources/status/true_strength_form.tres";
        /// <summary>
        ///   muscle 状态资源路径常量
        /// </summary>
        public const string MuscleStatus = "res://resources/status/muscle.tres";
        /// <summary>
        ///   exposes 状态资源路径常量
        /// </summary>
        public const string ExposedStatus = "res://resources/status/exposed.tres";
        
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

        /// <summary>
        ///     定义卡片控件的基础样式资源路径常量
        /// </summary>
        public const string CardBaseStyleBox = "res://scenes/ui/card_base_style_box_.tres";

        /// <summary>
        ///     定义卡片控件的悬停样式资源路径常量
        /// </summary>
        public const string CardHoverStyleBox = "res://scenes/ui/card_hover_style_box_.tres";
        
        public const string ToxinCard = "res://resources/cards/status/toxin.tres";
    }
}
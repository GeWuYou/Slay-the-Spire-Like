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
        ///     白色精灵材质路径
        /// </summary>
        public const string WhiteSpriteMaterial = "res://art/white_sprite_material.tres";
    }
}
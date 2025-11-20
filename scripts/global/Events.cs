using System;
using Godot;
using SlayTheSpireLike.scripts.enemies;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.ui;

namespace SlayTheSpireLike.scripts.global;

/// <summary>
///     事件管理类，用于定义和处理游戏中的各种事件
///     使用C#事件系统替代Godot信号系统
/// </summary>
public partial class Events : SingletonNode<Events>
{
    #region 卡牌事件

    /// <summary>
    ///     卡牌瞄准开始事件
    ///     当玩家开始瞄准一张卡牌时触发此事件
    /// </summary>
    public event Action<CardUi> CardAimingStarted;

    /// <summary>
    ///     卡牌瞄准结束事件
    ///     当玩家结束瞄准一张卡牌时触发此事件
    /// </summary>
    public event Action<CardUi> CardAimingEnded;

    /// <summary>
    ///     卡牌拖拽开始事件
    /// </summary>
    public event Action<CardUi> CardDraggingStarted;

    /// <summary>
    ///     卡牌拖拽结束事件
    /// </summary>
    public event Action<CardUi> CardDraggingEnded;

    /// <summary>
    ///     卡牌打出事件
    /// </summary>
    public event Action<Card> CardPlayed;

    /// <summary>
    ///     卡片工具提示显示请求事件
    ///     当需要显示卡片工具提示时触发此事件
    /// </summary>
    public event Action<Texture, string> CardToolTipShowRequest;

    /// <summary>
    ///     卡片工具提示隐藏请求事件
    ///     当需要隐藏卡片工具提示时触发此事件
    /// </summary>
    public event Action CardToolTipHideRequest;

    #endregion

    #region 玩家

    /// <summary>
    ///     玩家手牌绘制事件
    /// </summary>
    /// <remarks>
    ///     当玩家手牌被绘制时触发此事件
    /// </remarks>
    public event Action PlayerHandDrawn;

    /// <summary>
    ///     玩家手牌弃置事件
    /// </summary>
    public event Action PlayerHandDiscarded;

    /// <summary>
    ///     玩家回合结束事件
    /// </summary>
    public event Action PlayerTurnEnded;

    /// <summary>
    ///     玩家死亡事件
    /// </summary>
    public event Action PlayerDied;

    /// <summary>
    ///     玩家被攻击事件
    /// </summary>
    public event Action PlayerHit;

    #endregion

    #region 敌人

    /// <summary>
    ///     敌人行为完成事件
    /// </summary>
    public event Action<Enemy> EnemyActionCompleted;

    /// <summary>
    ///     敌人回合结束事件
    /// </summary>
    /// <remarks>
    ///     该委托用于定义敌人回合结束时触发的事件处理方法签名。
    ///     当敌人的行动回合完成时，会调用此委托关联的所有事件处理方法。
    /// </remarks>
    public event Action EnemyTurnEnded;

    #endregion

    #region 战斗事件

    /// <summary>
    ///     战斗结束界面请求事件
    /// </summary>
    public event Action<string, BattleOverPanel.Type> BattleOverScreenRequested;

    /// <summary>
    ///     战斗胜利事件
    /// </summary>
    public event Action BattleWon;

    /// <summary>
    ///     战斗奖励界面退出事件
    /// </summary>
    public event Action BattleRewardExited;

    #endregion

    #region 地图事件

    /// <summary>
    ///     地图退出事件
    /// </summary>
    public event Action MapExited;

    /// <summary>
    ///     地图进入事件
    /// </summary>
    public event Action MapEntered;

    #endregion

    #region 商店事件

    /// <summary>
    ///     商店退出事件
    /// </summary>
    public event Action ShopExited;

    /// <summary>
    ///     商店进入事件
    /// </summary>
    public event Action ShopEntered;

    #endregion

    #region 营火事件

    /// <summary>
    ///     营火退出事件
    /// </summary>
    public event Action CampfireExited;

    /// <summary>
    ///     营火进入事件
    /// </summary>
    public event Action CampfireEntered;

    #endregion

    #region 宝箱房间事件

    /// <summary>
    ///     宝箱房退出事件
    /// </summary>
    public event Action TreasureRoomExited;

    /// <summary>
    ///     宝箱房进入事件
    /// </summary>
    public event Action TreasureRoomEntered;


    #region 事件触发方法

    /// <summary>触发卡牌瞄准开始事件</summary>
    public void RaiseCardAimingStarted(CardUi cardUi)
    {
        CardAimingStarted?.Invoke(cardUi);
    }

    /// <summary>触发卡牌瞄准结束事件</summary>
    public void RaiseCardAimingEnded(CardUi cardUi)
    {
        CardAimingEnded?.Invoke(cardUi);
    }

    /// <summary>触发卡牌拖拽开始事件</summary>
    public void RaiseCardDraggingStarted(CardUi cardUi)
    {
        CardDraggingStarted?.Invoke(cardUi);
    }

    /// <summary>触发卡牌拖拽结束事件</summary>
    public void RaiseCardDraggingEnded(CardUi cardUi)
    {
        CardDraggingEnded?.Invoke(cardUi);
    }

    /// <summary>触发卡牌打出事件</summary>
    public void RaiseCardPlayed(Card card)
    {
        CardPlayed?.Invoke(card);
    }

    /// <summary>触发卡片工具提示显示请求事件</summary>
    public void RaiseCardToolTipShowRequest(Texture icon, string text)
    {
        CardToolTipShowRequest?.Invoke(icon, text);
    }

    /// <summary>触发卡片工具提示隐藏请求事件</summary>
    public void RaiseCardToolTipHideRequest()
    {
        CardToolTipHideRequest?.Invoke();
    }

    /// <summary>触发玩家手牌绘制事件</summary>
    public void RaisePlayerHandDrawn()
    {
        PlayerHandDrawn?.Invoke();
    }

    /// <summary>触发玩家手牌弃置事件</summary>
    public void RaisePlayerHandDiscarded()
    {
        PlayerHandDiscarded?.Invoke();
    }

    /// <summary>触发玩家回合结束事件</summary>
    public void RaisePlayerTurnEnded()
    {
        PlayerTurnEnded?.Invoke();
    }

    /// <summary>触发玩家死亡事件</summary>
    public void RaisePlayerDied()
    {
        PlayerDied?.Invoke();
    }

    /// <summary>触发玩家被攻击事件</summary>
    public void RaisePlayerHit()
    {
        PlayerHit?.Invoke();
    }

    /// <summary>触发敌人行为完成事件</summary>
    public void RaiseEnemyActionCompleted(Enemy enemy)
    {
        EnemyActionCompleted?.Invoke(enemy);
    }

    /// <summary>触发敌人回合结束事件</summary>
    public void RaiseEnemyTurnEnded()
    {
        EnemyTurnEnded?.Invoke();
    }

    /// <summary>触发战斗结束界面请求事件</summary>
    public void RaiseBattleOverScreenRequested(string text, BattleOverPanel.Type type)
    {
        BattleOverScreenRequested?.Invoke(text, type);
    }

    /// <summary>触发战斗胜利事件</summary>
    public void RaiseBattleWon()
    {
        BattleWon?.Invoke();
    }

    /// <summary>触发战斗奖励界面退出事件</summary>
    public void RaiseBattleRewardExited()
    {
        BattleRewardExited?.Invoke();
    }

    /// <summary>触发地图退出事件</summary>
    public void RaiseMapExited()
    {
        MapExited?.Invoke();
    }

    /// <summary>触发地图进入事件</summary>
    public void RaiseMapEntered()
    {
        MapEntered?.Invoke();
    }

    /// <summary>触发商店退出事件</summary>
    public void RaiseShopExited()
    {
        ShopExited?.Invoke();
    }

    /// <summary>触发商店进入事件</summary>
    public void RaiseShopEntered()
    {
        ShopEntered?.Invoke();
    }

    /// <summary>触发营火退出事件</summary>
    public void RaiseCampfireExited()
    {
        CampfireExited?.Invoke();
    }

    /// <summary>触发营火进入事件</summary>
    public void RaiseCampfireEntered()
    {
        CampfireEntered?.Invoke();
    }

    /// <summary>触发宝箱房退出事件</summary>
    public void RaiseTreasureRoomExited()
    {
        TreasureRoomExited?.Invoke();
    }

    /// <summary>触发宝箱房进入事件</summary>
    public void RaiseTreasureRoomEntered()
    {
        TreasureRoomEntered?.Invoke();
    }

    #endregion

    #endregion
}
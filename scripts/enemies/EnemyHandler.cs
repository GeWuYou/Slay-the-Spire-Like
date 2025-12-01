using System;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.enemies;

/// <summary>
///     敌人处理器类，负责管理敌人的行动顺序和回合控制
///     继承自Node2D节点，作为敌人管理的容器节点
/// </summary>
public partial class EnemyHandler : Node2D
{
    /// <summary>
    ///     活动敌人
    /// </summary>
    private Array<Enemy> _activeEnemies = [];
    /// <summary>
    ///     节点准备就绪时调用的方法
    ///     注册敌人行动完成事件的监听器
    /// </summary>
    public override void _Ready()
    {
        Events.Instance.EnemyActionCompleted += OnEnemyActionCompleted;
        Events.Instance.EnemyDied += OnEnemyDied;
    }

    private void OnEnemyDied(Enemy obj)
    {
        _activeEnemies.Remove(obj);
        if (_activeEnemies.Count > 0)
        {
            StartNextEnemyTurn();   
        }
    }

    public override void _ExitTree()
    {
        Events.Instance.EnemyActionCompleted -= OnEnemyActionCompleted;
        Events.Instance.EnemyDied -= OnEnemyDied;
    }

    /// <summary>
    ///     重置所有敌人的行动状态
    ///     将每个敌人的当前行动置空并更新行动显示
    /// </summary>
    public void ResetEnemyAcitons()
    {
        // 遍历所有子节点，重置敌人的行动状态
        foreach (var child in GetChildren())
        {
            if (child is not Enemy enemy) continue;

            enemy.CurrentAction = null;
            enemy.UpdateAction();
        }
    }

    /// <summary>
    ///     开始敌人回合
    /// </summary>
    public void StartTurn()
    {
        // 检查是否有子节点（敌人）
        if (GetChildCount() == 0) return;
        _activeEnemies.Clear();
        foreach (var child in GetChildren())
        {
            if (child is Enemy enemy)
            {
                _activeEnemies.Add(enemy);
            }
        }

        StartNextEnemyTurn();
    }

    private void StartNextEnemyTurn()
    {
        GD.Print($"当前敌人数量{_activeEnemies.Count}");
        if (_activeEnemies.Count == 0)
        {
            Events.Instance.RaiseEnemyTurnEnded();
            return;
        }
        _activeEnemies[0].StatusHandler.ApplyStatusesByType(Status.StatusType.StartOfTurn);
    }

    /// <summary>
    ///     处理敌人行动完成事件
    ///     当一个敌人完成行动后，触发下一个敌人的行动
    ///     如果是最后一个敌人，则发出敌人回合结束信号
    /// </summary>
    /// <param name="enemy">完成行动的敌人实例</param>
    private void OnEnemyActionCompleted(Enemy enemy)
    {
        enemy.StatusHandler.ApplyStatusesByType(Status.StatusType.EndOfTurn);
    }

    /// <summary>
    /// 设置战斗场景中的敌人对象
    /// </summary>
    /// <param name="battleStats">包含敌人信息的战斗统计数据对象</param>
    public void SetupEnemies(BattleStats battleStats)
    {
        // 检查输入参数是否为空
        if (battleStats is null)
        {
            return;
        }

        // 清除当前所有子节点
        foreach (var child in GetChildren())
        {
            child.QueueFree();
        }

        // 实例化所有敌人对象
        var allEnemies = battleStats.Enemies.Instantiate();

        // 遍历并添加有效的敌人节点
        foreach (var child in allEnemies.GetChildren())
        {
            // 只处理Enemy类型的节点
            if (child is not Enemy enemy)
            {
                continue;
            }
            // 在重新设置父节点前，先清除所有权关系
            if (enemy.Owner != null)
            {
                enemy.Owner = null;
            }
            enemy.Reparent(this);
            enemy.StatusHandler.StatusesApplied += type => OnEnemyStatusesApplied(type, enemy);
        }
        
        // 释放临时的敌人容器节点
        allEnemies.QueueFree();
    }

    private void OnEnemyStatusesApplied(Status.StatusType type,Enemy enemy)
    {
        switch (type)
        {
            case Status.StatusType.StartOfTurn:
                GD.Print("敌人开始回合！");
                enemy.DoTurn();
                break;
            case Status.StatusType.EndOfTurn:
                _activeEnemies.Remove(enemy);
                GD.Print("敌人回合结束！");
                StartNextEnemyTurn();
                break;
            case Status.StatusType.EventBased:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
using global::SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.enemies;

/// <summary>
///     敌人处理器类，负责管理敌人的行动顺序和回合控制
///     继承自Node2D节点，作为敌人管理的容器节点
/// </summary>
public partial class EnemyHandler : Node2D
{
    /// <summary>
    ///     节点准备就绪时调用的方法
    ///     注册敌人行动完成事件的监听器
    /// </summary>
    public override void _Ready()
    {
        Events.Instance.EnemyActionCompleted += OnEnemyActionCompleted;
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
    ///     触发第一个敌人的行动
    /// </summary>
    public void StartTurn()
    {
        // 检查是否有子节点（敌人）
        if (GetChildCount() == 0) return;
        var firstEnemy = GetChild(0) as Enemy;
        firstEnemy?.DoTurn();
    }

    /// <summary>
    ///     处理敌人行动完成事件
    ///     当一个敌人完成行动后，触发下一个敌人的行动
    ///     如果是最后一个敌人，则发出敌人回合结束信号
    /// </summary>
    /// <param name="enemy">完成行动的敌人实例</param>
    private void OnEnemyActionCompleted(Enemy enemy)
    {
        // 判断是否为最后一个敌人
        if (enemy.GetIndex() == GetChildCount() - 1)
        {
            Events.Instance.EmitSignal(Events.SignalName.EnemyTurnEnded);
            return;
        }

        // 获取并触发下一个敌人的行动
        var nextEnemy = GetChild(enemy.GetIndex() + 1) as Enemy;
        nextEnemy?.DoTurn();
    }
}
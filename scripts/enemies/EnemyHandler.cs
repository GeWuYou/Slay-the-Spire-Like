using global::SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.enemies;

public partial class EnemyHandler : Node2D
{
    public override void _Ready()
    {
        Events.Instance.EnemyActionCompleted += OnEnemyActionCompleted;
    }
    

    public void ResetEnemyAcitons()
    {
        foreach (var child in GetChildren())
        {
            if (child is not Enemy enemy)
            {
                continue;
            }

            enemy.CurrentAction = null;
            enemy.UpdateAction();
        }
    }

    public void StartTurn()
    {
        GD.Print("开始新的敌人回合");
        if (GetChildCount() == 0)
        {
            return;
        }
        var firstEnemy = GetChild(0) as Enemy;
        firstEnemy?.DoTurn();
    }

    private void OnEnemyActionCompleted(Enemy enemy)
    {
        if (enemy.GetIndex() == GetChildCount() - 1)
        {
            Events.Instance.EmitSignal(Events.SignalName.EnemyTurnEnded);
            return;
        }

        var nextEnemy = GetChild(enemy.GetIndex() + 1) as Enemy;
        nextEnemy?.DoTurn();
    }
}
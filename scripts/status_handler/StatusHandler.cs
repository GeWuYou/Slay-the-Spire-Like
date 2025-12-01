using System.Linq;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.status_handler;

public partial class StatusHandler : GridContainer
{
    [Export] public Node2D StatusOwner { get; set; }

    [Export] public float StatusApplyInterval { get; set; } = 0.25f;
    
    [Signal]
    public delegate void StatusesAppliedEventHandler(Status.StatusType type);

    public void ApplyStatusByType(Status.StatusType type)
    {
        if (type == Status.StatusType.EventBased)
        {
            return;
        }

        var statusQueue = GetAllStatuses().Where(status => status.Type == type).ToArray();
        if (!statusQueue.Any())
        {
            EmitSignal(Status.SignalName.StatusApplied, (int)type);
        }

        var tween = CreateTween();
        foreach (var status in statusQueue)
        {
            tween.TweenCallback(Callable.From(() => status.ApplyStatus(StatusOwner)));
            tween.TweenInterval(StatusApplyInterval);
        }

        tween.TweenInterval(0.0f);
        tween.Finished += () => EmitSignal(Status.SignalName.StatusApplied, (int)type);
    }

    public void AddStatus(Status status)
    {
        var stackable = status.StatusStackType != Status.StackType.None;
        if (!HasStatus(status.Id))
        {
            var newStatusUi = ResourceFactories.StatusUiFactory();
            newStatusUi.Status = status;
            newStatusUi.Status.StatusApplied += OnStatusApplied;
            newStatusUi.Status.InitializeStatus(StatusOwner);
            AddChild(newStatusUi);
            return;
        }

        if (!status.CanExpire && !stackable)
        {
            return;
        }

        if (status.CanExpire && status.StatusStackType == Status.StackType.Duration)
        {
            GetStatus(status.Id).Duration += status.Duration;
            return;
        }

        if (status.StatusStackType == Status.StackType.Intensity)
        {
            GetStatus(status.Id).Stacks += status.Stacks;
        }
    }

    private void OnStatusApplied(Status status)
    {
        if (status.CanExpire)
        {
            status.Duration -= 1;
        }
    }

    private Array<Status> GetAllStatuses()
    {
        var statuses = new Array<Status>();
        foreach (var child in GetChildren())
        {
            if (child is StatusUi statusUi)
            {
                statuses.Add(statusUi.Status);
            }
        }

        return statuses;
    }

    private Status GetStatus(string statusId)
    {
        foreach (var node in GetChildren())
        {
            if (node is StatusUi statusUi && statusUi.Status.Id == statusId)
            {
                return statusUi.Status;
            }
        }

        return null;
    }

    private bool HasStatus(string statusId)
    {
        foreach (var node in GetChildren())
        {
            if (node is StatusUi statusUi && statusUi.Status.Id == statusId)
            {
                return true;
            }
        }

        return false;
    }
}
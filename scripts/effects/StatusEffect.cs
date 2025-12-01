using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.component;
using SlayTheSpireLike.scripts.resources;
using Status = SlayTheSpireLike.scripts.statuses.Status;

namespace SlayTheSpireLike.scripts.effects;

public partial class StatusEffect : Effect
{
    [Export] public Status Status { get; set; }

    public override void Execute(Array<Node> targets)
    {
        foreach (var target in targets)
        {
            if (target is IStatusComponent statusComponent)
            {
                statusComponent.StatusHandler.AddStatus(Status);
            }
        }
    }
}
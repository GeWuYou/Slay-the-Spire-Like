using global::SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.map;

public partial class Map : Control
{
    [Export]
    public Button GoBackButton { get; set; }

    public override void _Ready()
    {
        GoBackButton ??= GetNode<Button>("Button");
        Events.Instance.EmitSignal(Events.SignalName.MapExited);
    }
}
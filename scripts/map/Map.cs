using SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.map;

public partial class Map : Control
{
    [Export]
    public Button GoBackButton { get; set; }

    public override void _Ready()
    {
        GoBackButton.Pressed += () => Events.Instance.RaiseMapExited();
    }
}
using global::SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.campfire;

public partial class Campfire : Control
{
    [Export]
    public Button GoBackButton { get; set; }

    public override void _Ready()
    {
        GoBackButton.Pressed += () => Events.Instance.RaiseCampfireExited();
    }
}
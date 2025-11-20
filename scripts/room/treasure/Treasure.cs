using SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.room.treasure;

public partial class Treasure : Control
{
    [Export]
    public Button GoBackButton { get; set; }

    public override void _Ready()
    {
        GoBackButton.Pressed += () => Events.Instance.RaiseTreasureRoomExited();
    }
}
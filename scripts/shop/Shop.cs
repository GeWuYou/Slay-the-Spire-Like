using SlayTheSpireLike.scripts.global;
using Godot;

namespace SlayTheSpireLike.scripts.shop;

public partial class Shop : Control
{
    [Export]
    public Button GoBackButton { get; set; }

    public override void _Ready()
    {
        GoBackButton.Pressed += () => Events.Instance.RaiseShopExited();
    }
}
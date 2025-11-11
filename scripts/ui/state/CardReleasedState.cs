
using Godot;

namespace DeckBuilderTutorial.scripts.ui.state;

/// <summary>
/// CardBaseState
/// </summary>
public partial class CardReleasedState: CardState
{
    public override void Enter()
    {
        CardUi.ColorRect.Color = Colors.DarkViolet;
    }
}
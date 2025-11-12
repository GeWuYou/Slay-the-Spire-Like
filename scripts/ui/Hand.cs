using Godot;

namespace DeckBuilderTutorial.scripts.ui;

public partial class Hand : HBoxContainer
{
    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is CardUi cardUi)
            {
                cardUi.Connect(CardUi.SignalName.ReparentRequested,
                    Callable.From((CardUi card) => OnCardReparentRequested(card)));
            }
        }
    }

    public void OnCardReparentRequested(CardUi cardUi)
    {
        cardUi.Reparent(this);
    }
}
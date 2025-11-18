using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class IntentUi : HBoxContainer
{
    [Export] public TextureRect Icon { get; set; }

    [Export] public Label Number { get; set; }

    public void UpdateIntent(Intent intent)
    {
        if (intent == null)
        {
            Hide();
            return;
        }

        Icon.Texture = intent.Icon as Texture2D;
        Icon.Visible = intent.Icon != null;
        Number.Text = intent.Number;
        Number.Visible = intent.Number?.Length > 0;
        Show();
    }
}
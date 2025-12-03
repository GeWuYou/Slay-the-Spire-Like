using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class IntentUi : HBoxContainer
{
    [Export] public TextureRect Icon { get; set; }

    [Export] public Label Label { get; set; }

    public void UpdateIntent(Intent intent)
    {
        if (intent == null)
        {
            Hide();
            return;
        }

        Icon.Texture = intent.Icon as Texture2D;
        Icon.Visible = intent.Icon != null;
        Label.Text = intent.CurrentText;
        Label.Visible = intent.CurrentText?.Length > 0;
        Show();
    }
}
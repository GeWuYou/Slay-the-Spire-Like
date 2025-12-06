using Godot;
using SlayTheSpireLike.scripts.save;

namespace SlayTheSpireLike.global;

public partial class GameManager : Node
{
    public static GameManager Instance { get;private set; }
    public static SaveManager SaveManager { get;private set; }
    public override void _Ready()
    {
        Instance = this;
        SaveManager = new SaveManager();
    }
}
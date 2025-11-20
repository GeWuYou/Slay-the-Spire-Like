using Godot;

namespace SlayTheSpireLike.scripts.resources;

[GlobalClass]
public partial class RunStartup : Resource
{
    [Export] public Type RunType { get; set; }
    [Export] public CharacterStats PlayerStats { get; set; }

    public enum Type
    {
        NewRun,
        ContinueRun
    }
}
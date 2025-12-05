using System.Linq;
using SlayTheSpireLike.scripts.global;
using Godot;
using Godot.Collections;
using SlayTheSpireLike.scripts.extensions;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.room.treasure;

public partial class Treasure : Control
{
    [Export] public Array<Relic> TreasureRelicPool { get; set; }
    [Export] public RelicHandler RelicHandler { get; set; }
    [Export] public CharacterStats PlayerStats { get; set; }

    [Export] public AnimationPlayer AnimationPlayer { get; set; }

    [Export] public TextureRect 宝箱纹理 { get; set; }
    private Relic _foundRelic;

    public override void _Ready()
    {
        宝箱纹理.GuiInput += OnGuiInput;
    }

    public void GenerateRelic()
    {
        _foundRelic = TreasureRelicPool
            .Where(relic => relic.CanAppearAsReward(PlayerStats) && !RelicHandler.HasRelic(relic.Id)).ToArray()
            .PickRandom();
    }
    public void OnTreasureOpen()
    {
        Events.Instance.RaiseTreasureRoomExited(_foundRelic);
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (AnimationPlayer.CurrentAnimation == "打开")
        {
            return;
        } 
        if (@event.IsActionPressed("left_mouse"))
        {
            AnimationPlayer.Play("打开");
        }
        
    }
}
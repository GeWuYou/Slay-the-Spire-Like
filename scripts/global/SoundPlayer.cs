using Godot;

namespace SlayTheSpireLike.scripts.global;

public partial class SoundPlayer : Node
{
    public static SoundPlayer Instance { get;private set; }
    public override void _Ready()
    {
        Instance = this;
    }

    public void Play(AudioStream audio, bool single = false)
    {
        if (audio is null)
        {
            return;
        }

        if (single)
        {
            Stop();
        }
        foreach (var child in GetChildren())
        {
            if (child is not AudioStreamPlayer player)
            {
                continue;
            }

            if (player.Playing)
            {
                continue;
            }

            player.Stream = audio;
            player.Play();
            break;
        }
    }

    private void Stop()
    {
        foreach (var child in GetChildren())
        {
            if (child is not AudioStreamPlayer player)
            {
                continue;
            }
            player.Stop();
        }
    }
}
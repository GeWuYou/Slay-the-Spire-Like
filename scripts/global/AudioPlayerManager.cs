using System.Collections.Generic;
using System.Linq;
using Godot;

namespace SlayTheSpireLike.scripts.global;

/// <summary>
///     通用的音频播放器管理器 —— 管理两类池：Music 与 SFX
///     将该脚本挂在一个 Autoload 节点上（Project Settings -> AutoLoad -> Name = AudioPlayerManager）
/// </summary>
public partial class AudioPlayerManager : Node
{
    /// <summary>
    ///     定义播放器类型枚举，用于区分音乐和音效。
    /// </summary>
    public enum PlayerType
    {
        /// <summary>
        ///     音乐类型播放器。
        /// </summary>
        Music,

        /// <summary>
        ///     音效类型播放器。
        /// </summary>
        Sfx
    }

    // 池
    private readonly List<AudioStreamPlayer> _musicPlayers = new();
    private readonly List<AudioStreamPlayer> _sfxPlayers = new();

    /// <summary>
    ///     获取当前实例的静态访问属性。
    /// </summary>
    public static AudioPlayerManager Instance { get; private set; }

    // 在编辑器中调整每种播放器的数量
    [Export] public int MusicPlayerCount { get; set; } = 4;
    [Export] public int SfxPlayerCount { get; set; } = 8;

    // 可选：用于创建的节点名字前缀，便于在场景树下辨别
    [Export] public string MusicPrefix { get; set; } = "MusicPlayer_";
    [Export] public string SfxPrefix { get; set; } = "SfxPlayer_";
    [Export] public string MusicBus { get; set; } = "Music";
    [Export] public string SfxBus { get; set; } = "SoundEffects";

    /// <summary>
    ///     初始化方法，在节点准备完成时调用。初始化播放器池并设置单例实例。
    /// </summary>
    public override void _Ready()
    {
        Instance = this;
        // 创建不足的播放器节点
        EnsurePlayerCount(_musicPlayers, MusicPlayerCount, MusicPrefix, MusicBus);
        EnsurePlayerCount(_sfxPlayers, SfxPlayerCount, SfxPrefix, SfxBus);
    }

    /// <summary>
    ///     当节点从场景树中移除时调用。清理单例实例引用。
    /// </summary>
    public override void _ExitTree()
    {
        if (Instance == this) Instance = null;
        base._ExitTree();
    }

    // ========== 公共 API ==========

    /// <summary>
    ///     播放音乐类型的音频流。
    /// </summary>
    /// <param name="audio">要播放的音频流。</param>
    /// <param name="single">是否停止其他正在播放的音乐后再播放，默认为 false。</param>
    public void PlayMusic(AudioStream audio, bool single = false)
    {
        PlayInPool(_musicPlayers, audio, single);
    }

    /// <summary>
    ///     播放音效类型的音频流。
    /// </summary>
    /// <param name="audio">要播放的音频流。</param>
    /// <param name="single">是否停止其他正在播放的音效后再播放，默认为 false。</param>
    public void PlaySfx(AudioStream audio, bool single = false)
    {
        PlayInPool(_sfxPlayers, audio, single);
    }

    /// <summary>
    ///     根据指定类型播放对应的音频流。
    /// </summary>
    /// <param name="audio">要播放的音频流。</param>
    /// <param name="type">播放器类型（音乐或音效）。</param>
    /// <param name="single">是否停止同类型其他正在播放的声音，默认为 false。</param>
    public void Play(AudioStream audio, PlayerType type, bool single = false)
    {
        if (type == PlayerType.Music) PlayMusic(audio, single);
        else PlaySfx(audio, single);
    }

    /// <summary>
    ///     停止所有正在播放的音乐。
    /// </summary>
    public void StopMusic()
    {
        StopPool(_musicPlayers);
    }

    /// <summary>
    ///     停止所有正在播放的音效。
    /// </summary>
    public void StopSfx()
    {
        StopPool(_sfxPlayers);
    }

    // ========== 内部实现 ==========

    /// <summary>
    ///     在指定的播放器池中播放音频流。
    /// </summary>
    /// <param name="pool">目标播放器列表。</param>
    /// <param name="audio">要播放的音频流。</param>
    /// <param name="single">是否停止其他正在播放的音频后再播放。</param>
    private static void PlayInPool(List<AudioStreamPlayer> pool, AudioStream audio, bool single)
    {
        if (audio is null) return;
        if (single) StopPool(pool);

        var player = pool.FirstOrDefault(p => IsInstanceValid(p) && !p.Playing);
        if (player != null)
        {
            player.Stream = audio;
            player.Play();
            return;
        }

        // 所有都在播放时，取第一个替换（可根据需求改为轮询替换）
        if (pool.Count <= 0) return;

        var p = pool[0];
        p.Stop();
        p.Stream = audio;
        p.Play();
    }

    /// <summary>
    ///     停止指定播放器池中的所有音频播放。
    /// </summary>
    /// <param name="pool">需要停止播放的播放器列表。</param>
    private static void StopPool(List<AudioStreamPlayer> pool)
    {
        foreach (var p in pool.Where(IsInstanceValid)) p.Stop();
    }

    /// <summary>
    ///     确保播放器池中有足够的播放器对象，并根据需要创建新的播放器。
    /// </summary>
    /// <param name="pool">目标播放器池。</param>
    /// <param name="desiredCount">期望的播放器数量。</param>
    /// <param name="prefix">新创建播放器名称的前缀。</param>
    private void EnsurePlayerCount(
        List<AudioStreamPlayer> pool,
        int desiredCount,
        string prefix,
        string bus)
    {
        // 创建缺少的播放器
        while (pool.Count < desiredCount)
        {
            var idx = pool.Count;
            var p = new AudioStreamPlayer
            {
                Name = $"{prefix}{idx}",
                Bus = bus
            };

            AddChild(p);
            pool.Add(p);
        }

        // 数量减少时，仅缩池，避免破坏场景树
        if (pool.Count > desiredCount)
            pool.RemoveRange(desiredCount, pool.Count - desiredCount);
    }
}
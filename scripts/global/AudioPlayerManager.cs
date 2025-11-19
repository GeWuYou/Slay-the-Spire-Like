using Godot;
using System.Collections.Generic;
using System.Linq;

namespace SlayTheSpireLike.scripts.global;

/// <summary>
/// 通用的音频播放器管理器 —— 管理两类池：Music 与 SFX
/// 将该脚本挂在一个 Autoload 节点上（Project Settings -> AutoLoad -> Name = AudioPlayerManager）
/// </summary>
public partial class AudioPlayerManager : Node
{
    /// <summary>
    /// 获取当前实例的静态访问属性。
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

    // 池
    private readonly List<AudioStreamPlayer> _musicPlayers = new();
    private readonly List<AudioStreamPlayer> _sfxPlayers = new();

    /// <summary>
    /// 初始化方法，在节点准备完成时调用。初始化播放器池并设置单例实例。
    /// </summary>
    public override void _Ready()
    {
        Instance = this;

        // 如果场景中已经预置了一些 AudioStreamPlayer，优先使用它们（按名字或者顺序）
        CollectExistingPlayers();

        // 创建不足的播放器节点
        EnsurePlayerCount(_musicPlayers, MusicPlayerCount, MusicPrefix);
        EnsurePlayerCount(_sfxPlayers, SfxPlayerCount, SfxPrefix);
    }

    /// <summary>
    /// 当节点从场景树中移除时调用。清理单例实例引用。
    /// </summary>
    public override void _ExitTree()
    {
        if (Instance == this) Instance = null;
        base._ExitTree();
    }

    // ========== 公共 API ==========

    /// <summary>
    /// 播放音乐类型的音频流。
    /// </summary>
    /// <param name="audio">要播放的音频流。</param>
    /// <param name="single">是否停止其他正在播放的音乐后再播放，默认为 false。</param>
    public void PlayMusic(AudioStream audio, bool single = false) => PlayInPool(_musicPlayers, audio, single);

    /// <summary>
    /// 播放音效类型的音频流。
    /// </summary>
    /// <param name="audio">要播放的音频流。</param>
    /// <param name="single">是否停止其他正在播放的音效后再播放，默认为 false。</param>
    public void PlaySfx(AudioStream audio, bool single = false) => PlayInPool(_sfxPlayers, audio, single);

    /// <summary>
    /// 根据指定类型播放对应的音频流。
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
    /// 停止所有正在播放的音乐。
    /// </summary>
    public void StopMusic() => StopPool(_musicPlayers);

    /// <summary>
    /// 停止所有正在播放的音效。
    /// </summary>
    public void StopSfx() => StopPool(_sfxPlayers);

    // ========== 内部实现 ==========

    /// <summary>
    /// 在指定的播放器池中播放音频流。
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
        if (pool.Count <= 0)
        {
            return;
        }

        var p = pool[0];
        p.Stop();
        p.Stream = audio;
        p.Play();
    }

    /// <summary>
    /// 停止指定播放器池中的所有音频播放。
    /// </summary>
    /// <param name="pool">需要停止播放的播放器列表。</param>
    private static void StopPool(List<AudioStreamPlayer> pool)
    {
        foreach (var p in pool.Where(IsInstanceValid))
        {
            p.Stop();
        }
    }

    /// <summary>
    /// 确保播放器池中有足够的播放器对象，并根据需要创建新的播放器。
    /// </summary>
    /// <param name="pool">目标播放器池。</param>
    /// <param name="desiredCount">期望的播放器数量。</param>
    /// <param name="prefix">新创建播放器名称的前缀。</param>
    private void EnsurePlayerCount(List<AudioStreamPlayer> pool, int desiredCount, string prefix)
    {
        // 如果已有足够数量就返回
        while (pool.Count < desiredCount)
        {
            var idx = pool.Count;
            var p = new AudioStreamPlayer();
            p.Name = $"{prefix}{idx}";
            p.Bus = idx < MusicPlayerCount ? MusicBus : SfxBus;
            AddChild(p);
            pool.Add(p);
        }

        // 如果编辑器里设置了比现有更少的数量，不自动删除节点，但从池中移除多余引用（避免破坏场景）
        if (pool.Count > desiredCount)
        {
            // 只在内存池中截断，不删除节点
            pool.RemoveRange(desiredCount, pool.Count - desiredCount);
        }
    }

    /// <summary>
    /// 尝试收集场景中已存在的 AudioStreamPlayer 节点：
    /// - 以名字前缀匹配优先（MusicPrefix / SfxPrefix）
    /// - 否则按遍历顺序分配（先分配给 Music，后分配给 SFX）
    /// </summary>
    private void CollectExistingPlayers()
    {
        var children = GetChildren();
        // 第一遍：按前缀匹配
        foreach (var child in children)
        {
            if (child is not AudioStreamPlayer asp)
            {
                continue;
            }

            var name = asp.Name.ToString();
            if (name.StartsWith(MusicPrefix))
            {
                asp.Bus = MusicBus;
                _musicPlayers.Add(asp);
            }
            else if (name.StartsWith(SfxPrefix))
            {
                asp.Bus = SfxBus;
                _sfxPlayers.Add(asp);
            }
        }

        // 第二遍：未按前缀匹配的，按顺序分配（先 Music 再 SFX），仅当池未满时分配
        foreach (var child in children)
        {
            if (child is not AudioStreamPlayer asp)
            {
                continue;
            }

            if (_musicPlayers.Contains(asp) || _sfxPlayers.Contains(asp))
            {
                continue;
            }

            if (_musicPlayers.Count < MusicPlayerCount)
            {
                asp.Bus = MusicBus;
                _musicPlayers.Add(asp);
            }
            else if (_sfxPlayers.Count < SfxPlayerCount)
            {
                asp.Bus = SfxBus;
                _sfxPlayers.Add(asp);
            }
            // 若两个池都已满，剩余节点不加入池（但仍在场景中）
        }
    }

    /// <summary>
    /// 定义播放器类型枚举，用于区分音乐和音效。
    /// </summary>
    public enum PlayerType
    {
        /// <summary>
        /// 音乐类型播放器。
        /// </summary>
        Music,

        /// <summary>
        /// 音效类型播放器。
        /// </summary>
        Sfx
    }
}

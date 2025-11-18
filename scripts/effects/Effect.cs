using Godot;
using Godot.Collections;

namespace SlayTheSpireLike.scripts.effects;

/// <summary>
///     效果类，用于执行各种游戏效果
/// </summary>
public partial class Effect : RefCounted
{
    /// <summary>
    ///     音效音频流源
    /// </summary>
    public AudioStream Sound { get; set; }
    /// <summary>
    ///     执行效果的方法，需要子类重写具体实现
    /// </summary>
    /// <param name="targets">效果作用的目标节点数组</param>
    public virtual void Execute(Array<Node> targets)
    {
    }
}
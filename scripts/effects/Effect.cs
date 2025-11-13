using Godot;
using Godot.Collections;

namespace DeckBuilderTutorial.scripts.effects;

/// <summary>
/// 效果类，用于执行各种游戏效果
/// </summary>
public partial class Effect : RefCounted
{
    /// <summary>
    /// 执行效果的方法，需要子类重写具体实现
    /// </summary>
    /// <param name="targets">效果作用的目标节点数组</param>
    protected virtual void Execute(Array<Node> targets)
    {
        
    }
}

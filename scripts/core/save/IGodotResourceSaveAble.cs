using Godot;
using SlayTheSpireLike.scripts.save;

namespace SlayTheSpireLike.scripts.core.save;

/// <summary>
/// 定义了一个继承自ISaveAble接口的Godot资源保存接口
/// 该接口专门用于处理Godot Resource类型的保存操作
/// </summary>
public interface IGodotResourceSaveAble:ISaveAble<Resource>;


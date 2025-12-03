using Godot;

namespace SlayTheSpireLike.scripts.resources;

/// <summary>
///     Intent类表示一个意图资源，继承自Resource基类。
///     该类被标记为全局类，可以通过全局访问方式引用。
/// </summary>
[GlobalClass]
public partial class Intent : Resource
{
    /// <summary>
    ///     获取或设置意图的编号字符串。
    ///     该属性被导出，可以在编辑器中进行配置。
    /// </summary>
    [Export]
    public string BaseText { get; set; } = "{0}";

    /// <summary>
    ///     获取或设置意图的图标纹理。
    ///     该属性被导出，可以在编辑器中进行配置。
    /// </summary>
    [Export]
    public Texture Icon { get; set; }
    
    public string CurrentText{ get; set; }
}
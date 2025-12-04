// meta-name: 遗物模板
// meta-description: 作为遗物逻辑的模板
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.relic;

public partial class _CLASS_ : Reilc
{
    public int MemberVariable { get; set; }
	
    /// <summary>
    /// 初始化遗物UI显示
    /// </summary>
    /// <param name="relicUi">需要初始化的遗物UI组件</param>
    public override void InitializeRelic(RelicUi relicUi)
    {
        //todo 初始化遗物UI
    }

    /// <summary>
    /// 激活遗物UI效果
    /// </summary>
    /// <param name="relicUi">需要激活的遗物UI组件</param>
    public override void ActivateRelic(RelicUi relicUi)
    {
        //todo 激活遗物UI
    }

    /// <summary>
    /// 停用遗物UI效果
    /// </summary>
    /// <param name="relicUi">需要停用的遗物UI组件</param>
    public override void DeactivateRelic(RelicUi relicUi)
    {
        //todo 停用遗物UI
    }

    /// <summary>
    /// 获取遗物的提示文本信息
    /// </summary>
    /// <returns>返回当前遗物的提示文本字符串</returns>
    public override string GetTooltip()
    {
        return Tooltip;
    }
}

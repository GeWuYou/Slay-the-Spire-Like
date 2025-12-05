using System.Threading.Tasks;
using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.extensions;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.shop;

public partial class ShopRelic : VBoxContainer
{
    [Export] public Button BuyButton { get; set; }
    [Export] public CenterContainer RelicContainer { get; set; }
    [Export] public HBoxContainer Price { get; set; }
    [Export] public Label PriceLabel { get; set; }
    private int _goldCost = GlobalBean.RandomNumberGenerator.RandiRange(100, 300);
    private Relic _relic;
    [Export]
    public Relic Relic
    {
        get => _relic;
        set
        {
            _relic = value;
            _ = SetRelic();
        }
    }
    /// <summary>
    /// 根据玩家当前金币数量更新UI状态（如价格颜色、按钮是否可用）。
    /// </summary>
    /// <param name="runStats">包含玩家当前游戏运行数据的对象，用于获取金币数量。</param>
    public void Update(RunStats runStats)
    {
        // 检查必要组件是否存在
        if (RelicContainer is null || Price is null || BuyButton is null)
        {
            return;
        }

        PriceLabel.Text = _goldCost.ToString();

        // 判断玩家是否有足够金币购买该卡牌
        if (runStats.Gold > _goldCost)
        {
            PriceLabel.RemoveThemeColorOverride("font_color");
            BuyButton.Disabled = false;
        }
        else
        {
            PriceLabel.AddThemeColorOverride("font_color", Colors.Red);
            BuyButton.Disabled = true;
        }
    }

    private async Task SetRelic()
    {
        await this.WaitUntilReady();
        // 清理已有子元素
        foreach (var child in RelicContainer.GetChildren())
        {
            child.QueueFreeX();
        }

        // 创建新卡牌UI并添加到容器中
        var relicUi = ResourceFactories.RelicUiFactory();
        relicUi.Relic = Relic;
        RelicContainer.AddChild(relicUi);
    }
}
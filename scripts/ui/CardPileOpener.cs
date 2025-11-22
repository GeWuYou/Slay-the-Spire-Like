using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

/// <summary>
/// CardPileOpener 是一个用于展示并监听卡牌堆变化的 UI 控件。
/// 它继承自 TextureButton，并通过绑定一个 CardPile 实例来实时显示其中卡片的数量。
/// 当 CardPile 中的卡片数量发生变化时，会自动更新界面上的计数标签。
/// </summary>
public partial class CardPileOpener : TextureButton
{
    private CardPile _cardPile;
    private Callable _callable;

    /// <summary>
    /// 获取或设置用于显示卡片数量的标签控件。
    /// </summary>
    [Export] public Label Counter { get; set; }

    /// <summary>
    /// 获取或设置当前绑定的 CardPile 实例。
    /// 设置新值时会自动断开与旧实例的信号连接，并建立与新实例的连接。
    /// 若新值为 null，则将计数器文本设为 "0"。
    /// </summary>
    public CardPile CardPile
    {
        get => _cardPile;
        set
        {
            // 如果之前绑定了一个 CardPile，先断开旧连接，避免重复触发
            if (_cardPile != null &&  _cardPile.HasSignal(CardPile.SignalName.CardPileSizeChanged) && _cardPile.IsConnected(CardPile.SignalName.CardPileSizeChanged, _callable))
            {
                _cardPile.Disconnect(CardPile.SignalName.CardPileSizeChanged, _callable);
            }

            _cardPile = value;

            if (_cardPile == null)
            {
                if (Counter != null) Counter.Text = "0";
                return;
            }

            // 使用固定的 callable 指向当前对象的处理方法（正确签名：接受 int）
            _callable = new Callable(this, nameof(OnCardPileSizeChanged));

            var signalName = CardPile.SignalName.CardPileSizeChanged;

            // 先检查目标对象是否真的声明了这个信号，避免 C++ 层面的 Nonexistent signal 错误
            if (_cardPile.HasSignal(signalName))
            {
                // 只有在还没有连接时才 Connect（注意：这里用 !IsConnected）
                if (!_cardPile.IsConnected(signalName, _callable))
                    _cardPile.Connect(signalName, _callable);
            }
            else
            {
                GD.PrintErr($"CardPile 对象没有信号 '{signalName}'，请确认 CardPile 类是否声明了该信号（名字区分大小写）。");
            }

            // 立即更新一次显示（初始化）
            OnCardPileSizeChanged(_cardPile.Cards?.Count ?? 0);
        }
    }

    /// <summary>
    /// 处理 CardPile 的卡片数量变更事件。
    /// 将传入的卡片数量更新到 Counter 标签上。
    /// </summary>
    /// <param name="count">当前 CardPile 中的卡片数量。</param>
    private void OnCardPileSizeChanged(int count)
    {
        if (Counter == null) return;
        Counter.Text = count.ToString();
    }
}

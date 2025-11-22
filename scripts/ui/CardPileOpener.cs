using Godot;
using SlayTheSpireLike.scripts.resources;

namespace SlayTheSpireLike.scripts.ui;

public partial class CardPileOpener : TextureButton
{
    private CardPile _cardPile;
    private Callable _callable;
    [Export] public Label Counter { get; set; }

    [Export]
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

    private void OnCardPileSizeChanged(int count)
    {
        if (Counter == null) return;
        Counter.Text = count.ToString();
    }
}
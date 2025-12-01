using Godot;
using SlayTheSpireLike.scripts.resources;
using Status = SlayTheSpireLike.scripts.statuses.Status;

namespace SlayTheSpireLike.scripts.status_handler;

public partial class StatusUi : Control
{
    private Status _status;

    [Export]
    public Status Status
    {
        get => _status;
        set
        {
            if (_status == value)
                return;

            if (_status != null)
            {
                _status.StatusChanged -= OnStatusChanged;
            }
            _status = value;
            CallDeferred(nameof(SetStatus), value);
        }
    }

    private TextureRect _icon;
    private Label _duration;
    private Label _stacks;

    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("Icon");
        _duration = GetNode<Label>("Duration");
        _stacks = GetNode<Label>("Stacks");
    }

    private void SetStatus(Status status)
    {
        _icon.Texture = status.Icon as Texture2D;
        
        _duration.Visible = status.StatusStackType == Status.StackType.Duration;
        _stacks.Visible = status.StatusStackType == Status.StackType.Intensity;
        CustomMinimumSize = _icon.Size;
        if (_duration.Visible)
        {
            CustomMinimumSize = _duration.Size + _duration.Position;
        }else if (_stacks.Visible)
        {
            CustomMinimumSize = _stacks.Size + _stacks.Position;
        }
        Status.StatusChanged+=OnStatusChanged;
        OnStatusChanged();
    }

    private void OnStatusChanged()
    {
        if (Status is null)
        {
            return;
        }

        if (Status.CanExpire && Status.Duration <= 0)
        {
            QueueFree();
        }

        if (Status.StatusStackType == Status.StackType.Intensity && Status.Stacks == 0)
        {
            QueueFree();
        }
        // 在访问UI元素之前检查它们是否仍然有效
        if (IsInstanceValid(_duration))
        {
            _duration.Text = Status.Duration.ToString();
        }
        
        if (IsInstanceValid(_stacks))
        {
            _stacks.Text = Status.Stacks.ToString();
        }
    }
}
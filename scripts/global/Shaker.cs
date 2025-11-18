using Godot;

namespace SlayTheSpireLike.scripts.global;

/// <summary>
/// 震动效果管理器，用于对2D节点应用屏幕震动效果
/// </summary>
public partial class Shaker : SingletonNode<Shaker>
{
    /// <summary>
    /// 对指定的2D节点应用震动效果
    /// </summary>
    /// <param name="thing">需要震动的2D节点对象</param>
    /// <param name="strength">震动强度，数值越大震动幅度越大</param>
    /// <param name="duration">震动持续时间，默认为0.2秒</param>
    /// <param name="shakeCount">震动次数，默认为10次</param>
    public void Shake(Node2D thing, float strength, float duration = 0.2f,float shakeCount = 10)
    {
        // 参数校验，如果节点为空则直接返回
        if (thing is null)
        {
            return;
        }

        // 记录节点的原始位置，用于震动结束后恢复位置
        var originalPosition = thing.Position;
        var tween = CreateTween();
        
        // 执行多次震动动画
        for (var i = 0; i < shakeCount; i++)
        {
            // 生成随机偏移方向
            var offset = new Vector2((float)GD.RandRange(-1.0, 1.0), (float)GD.RandRange(-1.0, 1.0));
            var target = originalPosition + strength * offset;
            
            // 每隔一次震动回到原始位置，形成来回震动效果
            if ((i & 1) == 0)
            {
                target = originalPosition;
            }

            // 添加位置变化动画
            tween.TweenProperty(thing, "position", target, duration / shakeCount);
            // 逐渐减小震动强度，形成衰减效果
            strength *= 0.75f;
        }

        // 震动结束后将节点位置重置为原始位置
        tween.Finished += () => thing.Position = originalPosition;
    }
}

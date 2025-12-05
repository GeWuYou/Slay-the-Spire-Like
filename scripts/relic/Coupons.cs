using global::SlayTheSpireLike.scripts.global;
using Godot;
using SlayTheSpireLike.scripts.modifier_handler;
using SlayTheSpireLike.scripts.relic_handler;
using SlayTheSpireLike.scripts.resources;
using SlayTheSpireLike.scripts.shop;

namespace SlayTheSpireLike.scripts.relic;

public partial class Coupons : Relic
{
    [Export(PropertyHint.Range, "0,1")] public float DiscountedValue { get; set; } = 0.5f;
    private RelicUi _relicUi;
    private const string CouponsSource = "Coupons";

    public override void InitializeRelic(RelicUi relicUi)
    {
        Events.Instance.ShopEntered += AddShopModifier;
        _relicUi = relicUi;
    }

    public override void DeactivateRelic(RelicUi relicUi)
    {
        Events.Instance.ShopEntered -= AddShopModifier;
    }

    public void AddShopModifier(Shop shop)
    {
        _relicUi.Flash();
        var modifierHandler = shop.ModifierHandler;
        var shopCostModifier = modifierHandler.GetModifier(Modifier.ModifierType.ShopCost);
        var shopCostModifierValue = shopCostModifier.GetValue(CouponsSource);
        if (shopCostModifierValue != null)
        {
            return;
        }

        shopCostModifierValue =
            ModifierValue.CreateNewModifier(CouponsSource, ModifierValue.ModifierValueType.PercentBased);
        shopCostModifierValue.PercentValue = -DiscountedValue;
        shopCostModifier.AddValue(shopCostModifierValue);
    }
}
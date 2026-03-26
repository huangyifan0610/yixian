using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yixian.Powers;

/// <summary>Star Power Power.</summary>
/// <remarks>A power that modifies the damage for cards on star point.</remarks>
public sealed class YxStarPowerPower : PowerModel
{
    /// <summary>Buff for positive amount and debuff for negative.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>A negative amount would decrease the damage.</summary>
    public override bool AllowNegative => true;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<YxStarPointPower>()];

    /// <summary>Modifies the damage if the <paramref name="cardSource"/> is on star point.</summary>
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // Modifies damage only if we (Owner) deal the damage.
        if (Owner != dealer) { return 0; }

        // Modifies damage only if it's powerable.
        if (props.HasFlag(ValueProp.Unpowered) || !props.HasFlag(ValueProp.Move)) { return 0; }

        // Modifies damage only if the card is on star point
        if (cardSource == null || !YxStarPointPower.Test(cardSource)) { return 0; }

        // Modifies damage according to the bonus.
        return cardSource.DynamicVars.TryGetValue(StarPowerBonusVar.DEFAULT, out var bonus)
            ? Amount * bonus.BaseValue
            : Amount;
    }
}

/// <summary>Dynamic variable representing the damage bonus from <see cref="YxStarPowerPower"/>.</summary>
public sealed class StarPowerBonusVar(decimal amount) : DynamicVar(DEFAULT, amount)
{
    public const string DEFAULT = "StarPowerBonus";
}

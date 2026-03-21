using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Patches;

namespace Yixian.Powers;

/// <summary>
/// Star Power increases damage for cards on the Star Point.
/// </summary>
public sealed class StarPower : PowerModel
{
    /// <summary>
    /// Buff for positive amount and debuff for negative amount.
    /// </summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>
    /// Star Power does count.
    /// </summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// Allow negative Star Powers.
    /// </summary>
    public override bool AllowNegative => true;

    /// <summary>
    /// Relates Star Point power.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<StarPoint>(),
    ];

    /// <summary>
    /// Also see <see cref="StrengthPower"/>.
    /// </summary>
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // The damage must be dealt by self;
        if (Owner != dealer) { return 0m; }

        // The damage must be powerable;
        if (props.HasFlag(ValueProp.Unpowered) || !props.HasFlag(ValueProp.Move)) { return 0m; }

        // The card must be on star point.
        if (cardSource == null || !cardSource.IsOnStarPoint()) return 0m;

        // Applies bonus on star power.
        if (cardSource.DynamicVars.TryStarPowerBonus(out var bonus))
        {
            return Amount * bonus.BaseValue;
        }
        else
        {
            return Amount;
        }
    }
}
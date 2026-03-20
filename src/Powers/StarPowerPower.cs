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
public sealed class StarPowerPower : PowerModel
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
        HoverTipFactory.FromPower<StarPointPower>(),
    ];

    /// <summary>
    /// Also see <see cref="StrengthPower"/>.
    /// </summary>
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // Modifies the damage if and only if:
        // 1) the damage is dealt by self;
        // 2) the damage is powerable;
        // 3) the card is on star point. 
        return (Owner == dealer && props.HasFlag(ValueProp.Move) && !props.HasFlag(ValueProp.Unpowered) && cardSource != null && cardSource.IsOnStarPoint()) ? Amount : 0m;
    }
}
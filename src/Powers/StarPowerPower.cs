using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

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
}
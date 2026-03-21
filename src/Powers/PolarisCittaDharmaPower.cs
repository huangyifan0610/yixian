using System.Collections.Generic;
using System.Linq;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>
/// A power indicating every slot is star point.
/// </summary>
public sealed class PolarisCittaDharmaPower : PowerModel
{
    /// <summary>
    /// Positive effect.
    /// </summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>
    /// Unquie status for every character.
    /// </summary>
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>
    /// Adds star point power and star power power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<StarPoint>(),
        HoverTipFactory.FromPower<StarPower>(),
    ]);
}

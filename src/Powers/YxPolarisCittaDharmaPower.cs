using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>Polaris Citta Dharma Power.</summary>
/// <remarks>All slots in hand are star points.</remarks>
public sealed class YxPolarisCittaDharmaPower : PowerModel
{
    /// <summary>Possitive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>Marker buff.</summary>
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxStarPointPower>(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];
}
using System;
using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>Star Moon Folding Fan Power.</summary>
/// <remarks>A power that decreases the energy costs for cards on star point.</remarks>
public sealed class YxStarMoonFoldingFanPower : PowerModel
{
    /// <summary>Positive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<YxStarPointPower>()];

    /// <summary>Modifies the energy cost if the <paramref name="cardSource"/> is on star point.</summary>
    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {

        if (YxStarPointPower.Test(card))
        {
            modifiedCost = Math.Max(originalCost - Amount, 0m);
            if (modifiedCost != originalCost)
            {
                return true;
            }
        }

        modifiedCost = originalCost;
        return false;
    }
}

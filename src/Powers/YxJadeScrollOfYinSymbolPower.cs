using System.Collections.Generic;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yixian.Powers;

/// <summary>Jade Scroll Of Yin Symbol Power.</summary>
/// <remarks>Grow the effects of Weak and Vulnerable.</remarks>
public sealed class YxJadeScrollOfYinSymbolPower : PowerModel
{
    /// <summary>Positive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
    ];

    /// <remarks>Grow the effects of Weak and Vulnerable.</remarks>
    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (dealer == Owner && target != Owner)
        {
            var vulnerable = target?.GetPower<VulnerablePower>();
            if (vulnerable != null)
            {
                return 1m + (Amount / 100m);
            }
        }
        else if (dealer != Owner && target == Owner)
        {
            var weak = dealer?.GetPower<WeakPower>();
            if (weak != null)
            {
                return 1m - (Amount / 100m);
            }
        }

        return 1m;
    }
}

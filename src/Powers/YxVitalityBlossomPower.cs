using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>Vitality Blossom Power.</summary>
/// <remarks>Convert damage to healing after the combat.</remarks>
public sealed class YxVitalityBlossomPower : PowerModel
{
    /// <summary>Possitive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The buff counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <remarks>Convert damage to healing after the combat.</remarks>
    public async Task OnCombatEnd(YxTemporaryHpPower temporaryHp)
    {
        Flash();
        await CreatureCmd.Heal(Owner, Math.Min(temporaryHp.Amount, Amount));
        await PowerCmd.Remove(this);
    }
}

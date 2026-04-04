using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>Stillness Citta Dharma Power.</summary>
/// <remarks>Gain temporary HP after spent energy.</remarks>
public sealed class YxStillnessCittaDharmaPower : PowerModel
{
    /// <summary>Positive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <remarks>Gain temporary HP after spent energy.</remarks>
    public override Task AfterEnergySpent(CardModel card, int amount) =>
        PowerCmd.Apply<YxTemporaryHpPower>(Owner, Amount * amount, Owner, null);
}

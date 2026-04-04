using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Powers;

/// <summary>Spiritual Divination Power.</summary>
/// <remarks>Gain hexagram after spent energy.</remarks>
public sealed class YxSpiritualDivinationPower : PowerModel
{
    /// <summary>Positive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <remarks>Gain temporary HP after spent energy.</remarks>
    public override Task AfterEnergySpent(CardModel card, int amount) =>
        PowerCmd.Apply<YxHexagramPower>(Owner, Amount * amount, Owner, null);
}

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Cards;

namespace Yixian.Powers;

/// <summary>Thunder Citta Dharma Power.</summary>
/// <remarks>Thunder cards deal additional damage.</remarks>
public sealed class YxThunderCittaDharmaPower : PowerModel
{
    /// <summary>Possitive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The buff counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Modifies the damage if the <paramref name="cardSource"/> is 'Thunder'.</summary>
    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // Modifies damage only if we (Owner) deal the damage.
        if (Owner != dealer) { return 0; }

        // Modifies damage only if it's powerable.
        if (props.HasFlag(ValueProp.Unpowered) || !props.HasFlag(ValueProp.Move)) { return 0; }

        // Modifies the damage.
        return (cardSource is YxCardModel thunderCard && thunderCard.IsThunder)
            ? 1m + (Amount / 100m)
            : 1m;
    }
}
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yixian.Powers;

/// <summary>Covert Shift Power.</summary>
/// <remarks>Reduce HP is reversed to heal HP.</remarks>
public sealed class YxCovertShiftPower : PowerModel
{
    /// <summary>Positive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Reduce HP is reversed to heal HP.</summary>
    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner && amount > 0)
        {
            CreatureCmd.Heal(Owner, amount);
            PowerCmd.Decrement(this);
            return 0m;
        }
        else
        {
            return amount;
        }
    }
}

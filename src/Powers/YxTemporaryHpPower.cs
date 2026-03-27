using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace Yixian.Powers;

/// <summary>Temporary HP.</summary>
public sealed class YxTemporaryHpPower : PowerModel
{
    /// <summary>Neither buff or debuff.</summary>
    public override PowerType Type => PowerType.None;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this)
        {
            decimal oldMaxHp = Owner.MaxHp;
            decimal oldHp = Owner.CurrentHp;
            decimal newMaxHp = oldMaxHp + amount;
            decimal newHp = Math.Clamp(oldHp + amount, 1, newMaxHp);
            Main.LOGGER.Info("============ BeforeApplied " + amount + "     " + newMaxHp);

            if (oldMaxHp != newMaxHp)
            {
                await CreatureCmd.SetMaxHp(Owner, newMaxHp);
            }

            if (oldHp != newHp)
            {
                await CreatureCmd.SetCurrentHp(Owner, newHp);
            }
        }
    }

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        Flash();

        decimal oldMaxHp = Owner.MaxHp;
        decimal oldHp = Owner.CurrentHp;
        decimal newMaxHp = Math.Max(oldMaxHp - Amount, 1m);
        decimal newHp = Math.Clamp(oldHp - Amount, 1m, newMaxHp);

        if (oldHp != newHp)
        {
            await CreatureCmd.SetCurrentHp(Owner, newHp);
        }

        if (oldMaxHp != newMaxHp)
        {
            await CreatureCmd.SetMaxHp(Owner, newMaxHp);
        }

        await PowerCmd.Remove(this);
    }
}

using System;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yixian.Powers;

/// <summary>Temporary HP.</summary>
public sealed class YxTemporaryHpPower : PowerModel
{
    /// <summary>Neither buff or debuff.</summary>
    public override PowerType Type => PowerType.None;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Gains HP and Max HP after power applied.</summary>
    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this)
        {
            decimal newMaxHp = Owner.MaxHp + amount;
            decimal newHp = Owner.CurrentHp + amount;

            await CreatureCmd.SetMaxHp(Owner, newMaxHp);
            await CreatureCmd.SetCurrentHp(Owner, newHp);
        }
    }

    /// <summary>Loses HP and Max HP after the combat.</summary>
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        var vitalityBlossom = Owner.GetPower<YxVitalityBlossomPower>();
        if (vitalityBlossom != null)
        {
            await vitalityBlossom.OnCombatEnd(this);
        }

        decimal oldMaxHp = Owner.MaxHp;
        decimal oldHp = Owner.CurrentHp;
        decimal newMaxHp = Math.Max(oldMaxHp - Amount, 1m);
        decimal newHp = Math.Clamp(oldHp - Amount, 1m, newMaxHp);

        Flash();

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

    /// <summary>Loses Max HP after damage received.</summary>
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner)
        {
            int oldAmount = Amount;
            if (result.UnblockedDamage < oldAmount)
            {
                SetAmount(oldAmount - result.UnblockedDamage);
                await CreatureCmd.SetMaxHp(Owner, Math.Max(1m, Owner.MaxHp - result.UnblockedDamage));
            }
            else
            {
                RemoveInternal();
                await CreatureCmd.SetMaxHp(Owner, Math.Max(1m, Owner.MaxHp - oldAmount));
            }
        }
    }
}

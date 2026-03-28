using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Cards;

namespace Yixian.Powers;

/// <summary>Fury Thunder Power.</summary>
/// <remarks>Let next Thunder card deal additional damage.</remarks>
public sealed class YxFuryThunderPower : PowerModel
{
    /// <summary>Positive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>This power is applied on the recorded attack.</summary>
    protected override object InitInternalData() => new Data();
    private class Data { public AttackCommand? attackCommand; }

    /// <summary>Before Attacking: record the attack command.</summary>
    public override async Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker == Owner
            && command.DamageProps.HasFlag(ValueProp.Move)
            && !command.DamageProps.HasFlag(ValueProp.Unpowered)
            && command.ModelSource is YxCardModel thunderAttack
            && thunderAttack.Type == CardType.Attack
            && thunderAttack.IsThunder)
        {
            var data = GetInternalData<Data>();
            data.attackCommand ??= command;
        }
    }
    /// <summary>After Attacking: remove the power for the recorded attack command.</summary>
    public override async Task AfterAttack(AttackCommand command)
    {
        var data = GetInternalData<Data>();
        if (data.attackCommand == command)
        {
            RemoveInternal();
        }
    }

    /// <summary>Modify the damage.</summary>
    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // 1) if attack command is not recorded, modify damage for thunder attack cards.
        // 2) if attack command is recorded, only modify damage for the recorded one.
        var data = GetInternalData<Data>();
        return (data.attackCommand == null
                && dealer == Owner
                && props.HasFlag(ValueProp.Move)
                && !props.HasFlag(ValueProp.Unpowered)
                && target != Owner
                && cardSource is YxCardModel thunderAttack
                && thunderAttack.Type == CardType.Attack
                && thunderAttack.IsThunder)
            || (data.attackCommand != null
                && data.attackCommand.ModelSource == cardSource
                && data.attackCommand.Attacker == dealer)
            ? 1m + (Amount / 100m)
            : 1m;
    }
}

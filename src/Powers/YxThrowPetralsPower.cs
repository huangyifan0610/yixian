using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yixian.Powers;

/// <summary>Throw Petrals Power.</summary>
/// <remarks>Apply poison after deal damage.</remarks>
public sealed class YxThrowPetralsPower : PowerModel
{
    /// <summary>Possitive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The buff counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Adds necessary hover tips.</summary>
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<PoisonPower>(),
    ];

    /// <remarks>Apply poison after deal damage.</remarks>
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer == Owner && props.HasFlag(ValueProp.Move) && !props.HasFlag(ValueProp.Unpowered))
        {
            await PowerCmd.Apply<PoisonPower>(target, Amount, Owner, null);
        }
    }
}

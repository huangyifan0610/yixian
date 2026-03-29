using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yixian.Powers;

/// <summary>Hexagram Formacide Power.</summary>
/// <remarks>Deal damage to ALL enemies when you gain hexagram.</remarks>
public sealed class YxHexagramFormacidePower : PowerModel
{
    /// <summary>Positive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <remarks>Deal damage to ALL enemies when you gain hexagram.</remarks>
    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is YxHexagramPower && amount > 0)
        {
            await CreatureCmd.Damage(
                new ThrowingPlayerChoiceContext(),
                CombatState.HittableEnemies,
                amount * Amount,
                ValueProp.Unpowered,
                Owner,
                null
            );
        }
    }
}

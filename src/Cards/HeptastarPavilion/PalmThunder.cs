using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// <c>Palm Thunder</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class PalmThunder : HeptastarPavilionCardModel
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal at least 2 damage.
        new DamageVar(DAMAGE_LOWER_BOUND_VAR, 2, ValueProp.Move),
        // Deal at most 10 hexagrams.
        new DamageVar(DAMAGE_UPPER_BOUND_VAR, 10, ValueProp.Move),
    ]);
    private const string DAMAGE_LOWER_BOUND_VAR = "DamageLowerBound"; 
    private const string DAMAGE_UPPER_BOUND_VAR = "DamageUpperBound"; 

    /// <summary>
    /// Adds star point power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<HexagramPower>(),
    ]);

    /// <summary>
    /// The default constructor.
    /// </summary>
    public PalmThunder() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    /// <summary>
    /// Deal random damage.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
        {
            throw new ArgumentNullException("cardPlay.Target");
        }

        var hexagramPower = Owner.Creature.GetPower<HexagramPower>();
        decimal lowerBound = DynamicVars[DAMAGE_LOWER_BOUND_VAR].BaseValue;
        decimal upperBound = DynamicVars[DAMAGE_UPPER_BOUND_VAR].BaseValue;

        if (hexagramPower?.Amount > 0)
        {
            // Takes maximum value.
            await DamageCmd
                .Attack(upperBound)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);

            // Consumes one hexagram.
            await PowerCmd.ModifyAmount(hexagramPower, -1m, Owner.Creature, this);
        }
        else
        {
            // Deal random damage.
            // FIXME: We need a specialized RNG for hexagrams.
            await DamageCmd
                .Attack(Owner.RunState.Rng.Niche.NextInt((int)lowerBound, (int)upperBound + 1))
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }
    }

    /// <summary>
    /// Upgrade the damage.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars[DAMAGE_LOWER_BOUND_VAR].UpgradeValueBy(3);
        DynamicVars[DAMAGE_UPPER_BOUND_VAR].UpgradeValueBy(3);
    }
}
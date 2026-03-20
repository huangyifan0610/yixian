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
        if (cardPlay.Target != null)
        {
            int lowerBound = (int)DynamicVars[DAMAGE_LOWER_BOUND_VAR].BaseValue;
            int upperBound = (int)DynamicVars[DAMAGE_UPPER_BOUND_VAR].BaseValue;
            decimal damage = await HexagramPower.Range(Owner.Creature, this, Owner.RunState, lowerBound, upperBound);

            // Takes maximum value.
            await DamageCmd
                .Attack(damage)
                .FromCard(this)
                .WithHitFx("vfx/vfx_attack_lightning")
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }
    }

    /// <summary>
    /// Upgrade the damage.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars[DAMAGE_LOWER_BOUND_VAR].UpgradeValueBy(2);
        DynamicVars[DAMAGE_UPPER_BOUND_VAR].UpgradeValueBy(4);
    }
}
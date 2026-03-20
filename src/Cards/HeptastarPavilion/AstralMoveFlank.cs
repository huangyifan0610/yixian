using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.HoverTips;
using Yixian.Patches;
using Yixian.Powers;


namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// <c>Astral Move - Flank</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class AstralMoveFlank() : HeptastarPavilionCardModel(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    /// <summary>
    /// The star point damage variable.
    /// </summary>
    private const string STAR_POINT_DAMAGE_VAR = "StarPointDamage";

    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal 6 damage.
        new DamageVar(6, ValueProp.Move),
        // Star Point: Deal 3 damage.
        new DamageVar(STAR_POINT_DAMAGE_VAR, 3, ValueProp.Move),
    ]);

    /// <summary>
    /// Adds star point power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<StarPointPower>(),
    ]);

    /// <summary>
    /// The card tags.
    /// </summary>
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    /// <summary>
    /// Glow if on Star Point.
    /// </summary>
    protected override bool ShouldGlowGoldInternal => this.IsOnStarPoint();

    /// <summary>
    /// Deal damages.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            // Deal damage.
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);

            // Continue to deal star point damage.
            if (this.IsOnStarPoint())
            {
                await DamageCmd
                    .Attack(DynamicVars[STAR_POINT_DAMAGE_VAR].BaseValue)
                    .FromCard(this)
                    .Targeting(cardPlay.Target)
                    .Execute(choiceContext);
            }
        }
    }

    /// <summary>
    /// Upgrade the damage.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars[STAR_POINT_DAMAGE_VAR].UpgradeValueBy(3);
    }
}


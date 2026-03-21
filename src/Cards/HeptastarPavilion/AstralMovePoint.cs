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
/// <c>Astral Move - Point</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class AstralMovePoint() : HeptastarPavilionCardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal 6 damage.
        new DamageVar(6, ValueProp.Move),
        // Deal 9 damage to all enemies if on star point.
        new ExtraDamageVar(9),
    ]);

    /// <summary>
    /// Adds star point power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<StarPoint>(),
    ]);

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
            // Deal damage once.
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitFx("vfx/vfx_starry_impact")
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }

        // Deal damage to all enemies if on star point.
        if (this.IsOnStarPoint() && CombatState != null)
        {
            await DamageCmd
                .Attack(DynamicVars.ExtraDamage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .Execute(choiceContext);
        }
    }

    /// <summary>
    /// Upgrade the extra damage.
    /// </summary>
    protected override void OnUpgrade() => DynamicVars.ExtraDamage.UpgradeValueBy(4);
}
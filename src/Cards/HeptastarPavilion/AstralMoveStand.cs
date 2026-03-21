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
/// <c>Astral Move - Stand</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class AstralMoveStand() : HeptastarPavilionCardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal 9 damage to all enemies.
        new DamageVar(9, ValueProp.Move),
        // Gain 2 energy.
        new EnergyVar(2),
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
    /// Deal damages and gain energy.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
        {
            // Deal damage once.
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitFx("vfx/vfx_starry_impact")
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .Execute(choiceContext);
        }

        // Deal damage to all enemies if on star point.
        if (this.IsOnStarPoint())
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }

    /// <summary>
    /// Gain more energy.
    /// </summary>
    protected override void OnUpgrade() => DynamicVars.Energy.UpgradeValueBy(1);
}
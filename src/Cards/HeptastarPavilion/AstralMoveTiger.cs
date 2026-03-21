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
using MegaCrit.Sts2.Core.Models.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// <c>Astral Move - Tiger</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class AstralMoveTiger() : HeptastarPavilionCardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal 1 damage three times.
        new DamageVar(1, ValueProp.Move),
        // Apply 2 weak.
        new PowerVar<WeakPower>(2),
    ]);

    /// <summary>
    /// Adds star point power and weak power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<StarPoint>(),
        HoverTipFactory.FromPower<WeakPower>(),
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
            // Deal damage three times.
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitFx("vfx/vfx_starry_impact")
                .WithHitCount(3)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);

            // Apply weak if on star point.
            if (this.IsOnStarPoint())
            {
                await PowerCmd.Apply<WeakPower>(cardPlay.Target, DynamicVars.Weak.BaseValue, Owner.Creature, this);
            }
        }
    }

    /// <summary>
    /// Apply more weak after upgrade.
    /// </summary>
    protected override void OnUpgrade() => DynamicVars.Weak.UpgradeValueBy(2);
}
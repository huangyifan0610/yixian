using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.HoverTips;
using Yixian.Patches;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// <c>Incessant</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class Incessant() : HeptastarPavilionCardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal 6 damage.
        new DamageVar(6, ValueProp.Move),
        // Deal 3 damage on post action.
        new DamageVar(POST_ACTION_DAMAGE_VAR, 3, ValueProp.Move),
    ]);
    private const string POST_ACTION_DAMAGE_VAR = "PostActionDamage";

    /// <summary>
    /// Adds star point power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactoryExtension.Static(StaticHoverTipExtension.PostAction),
    ]);

    /// <summary>
    /// Glow if post action takes effects.
    /// </summary>
    protected override bool ShouldGlowGoldInternal => this.HasPlayed();

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

            // Continue post action.
            if (this.HasPlayed())
            {
                await DamageCmd
                    .Attack(DynamicVars[POST_ACTION_DAMAGE_VAR].BaseValue)
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
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars[POST_ACTION_DAMAGE_VAR].UpgradeValueBy(3);
    }
}
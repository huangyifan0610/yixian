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
/// <c>Astral Move - Fly</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class AstralMoveFly() : HeptastarPavilionCardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal 5 damage twice.
        new DamageVar(5, ValueProp.Move),
        // Draw 2 cards if on star point.
        new CardsVar(2),
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
            // Deal damage.
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(2)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }
    }

    /// <summary>
    /// Upgrade the damage and star power bonus.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
}
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
/// <c>Astral Move - Cide</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class AstralMoveCide() : HeptastarPavilionCardModel(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal 16 damage.
        new DamageVar(16, ValueProp.Move),
    ]);

    /// <summary>
    /// Adds star point power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<StarPointPower>(),
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
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);

            // Skips monster's next intent.
            if (this.IsOnStarPoint() && cardPlay.Target.Monster?.MoveStateMachine != null)
            {
                cardPlay.Target.Monster.MoveStateMachine.OnMovePerformed(cardPlay.Target.Monster.NextMove);
                cardPlay.Target.Monster.RollMove(cardPlay.Target.Monster.CombatState.PlayerCreatures);
                cardPlay.Target.Monster.SetMoveImmediate(cardPlay.Target.Monster.NextMove, true);
            }
        }
    }

    /// <summary>
    /// Upgrade the damage.
    /// </summary>
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(6);
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Astral Move Cide.</summary>
public sealed class YxAstralMoveCide() : YxCardModel(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Astral Move.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.AstralMove];

    /// <summary>Deal damage; Skip target's next intent on star point.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(18, ValueProp.Move),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.AstralMove.GetHoverTip(),
        HoverTipFactory.FromPower<YxStarPointPower>(),
    ];

    /// <summary>Glow if on star point.</summary>
    protected override bool ShouldGlowGoldInternal => IsOnStarPoint;

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(6);

    /// <summary>Deal damage; Skip target's next intent on star point.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        if (IsOnStarPoint)
        {
            // Run carefully because the target enemy may have been killed.
            if (cardPlay.Target.Monster?.MoveStateMachine != null && cardPlay.Target.Monster.NextMove != null)
            {
                cardPlay.Target.Monster.MoveStateMachine?.OnMovePerformed(cardPlay.Target.Monster.NextMove);
            }
            if (cardPlay.Target.Monster?.CombatState != null)
            {
                cardPlay.Target.Monster.RollMove(cardPlay.Target.Monster.CombatState.PlayerCreatures);
            }
            if (cardPlay.Target.Monster?.MoveStateMachine != null && cardPlay.Target.Monster.NextMove != null)
            {
                cardPlay.Target.Monster.SetMoveImmediate(cardPlay.Target.Monster.NextMove, true);
            }
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Yixian.Cards;

namespace Yixian.Powers;

/// <summary>Hunter Hunting Hunter Power.</summary>
/// <remarks>Whenever you played Post Action card, deal damage to random enemy.</remarks>
public sealed class YxHunterHuntingHunterPower : PowerModel
{
    /// <summary>Positive effect.</summary>
    public override PowerType Type => PowerType.Buff;

    /// <summary>The amount counts.</summary>
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.PostAction.GetHoverTip()
    ];

    /// <summary>Reduce HP is reversed to heal HP.</summary>
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay.Card is YxCardModel postActionCard && postActionCard.IsPostAction)
        {
            Creature? target = cardPlay.Target ?? Owner.Player?.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
            if (target != null)
            {
                await DamageCmd
                    .Attack(Amount)
                    .WithWaitBeforeHit(0.2f, 0.3f)
                    .FromCard(cardPlay.Card)
                    .Targeting(target)
                    .Execute(context);
            }
        }
    }
}

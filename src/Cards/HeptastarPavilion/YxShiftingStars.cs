using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Characters;
using Yixian.Patches;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Shifting Stars.</summary>
public sealed class YxShiftingStars() : YxCardModel(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Deal damage; Gain energy on star point; Become star point.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3, ValueProp.Move),
        new EnergyVar(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxStarPointPower>(),
    ];

    /// <summary>Glow if on star point.</summary>
    protected override bool ShouldGlowGoldInternal => IsOnStarPoint;

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3);

    /// <summary>Deal damage; Gain energy on star point; Become star point.</summary>
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
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
        else
        {
            var starPointPower = Owner.Creature.GetPower<YxStarPointPower>()
                ?? await PowerCmd.Apply<YxStarPointPower>(Owner.Creature, 1, Owner.Creature, this);
            int index = this.IndexInHand();

            if (starPointPower != null && 0 <= index && index < CardPile.maxCardsInHand)
            {
                starPointPower[index] = true;
            }
        }
    }
}

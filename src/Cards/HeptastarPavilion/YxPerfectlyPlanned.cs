using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Patches;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Perfectly Planned.</summary>
public sealed class YxPerfectlyPlanned() : YxCardModel(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Exhaust.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    /// <summary>Character Exclusive.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.CharacterExclusiveWuCe];

    /// <summary>Gain energy; Gain hexagram; Become star point.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(1),
        new PowerVar<YxHexagramPower>(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.CharacterExclusiveWuCe.GetHoverTip(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
        HoverTipFactory.FromPower<YxStarPointPower>(),
    ];

    /// <summary>Adds Innate.</summary>
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);

    /// <summary>Gain energy; Gain hexagram; Become star point.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await PowerCmd.Apply<YxHexagramPower>(
            Owner.Creature,
            DynamicVars[nameof(YxHexagramPower)].BaseValue,
            Owner.Creature,
            this
        );

        if (!IsOnStarPoint)
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

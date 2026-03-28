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

/// <summary>Heptastar Pavilion - Water Hexagram.</summary>
public sealed class YxWaterHexagram() : YxCardModel(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Hexagram.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Hexagram];

    /// <summary>Gain hexagram; Gain energy on star point; Become star point.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxHexagramPower>(2),
        new EnergyVar(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Hexagram.GetHoverTip(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
        HoverTipFactory.FromPower<YxStarPointPower>(),
    ];

    /// <summary>Glow if on star point.</summary>
    protected override bool ShouldGlowGoldInternal => IsOnStarPoint;

    /// <summary>Gain more hexagram.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxHexagramPower)].UpgradeValueBy(1);

    /// <summary>Gain hexagram; Gain energy on star point; Become star point.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxHexagramPower>(
            Owner.Creature,
            DynamicVars[nameof(YxHexagramPower)].BaseValue,
            Owner.Creature,
            this
        );

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

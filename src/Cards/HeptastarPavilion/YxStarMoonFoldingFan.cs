using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Star Moon Folding Fan.</summary>
public sealed class YxStarMoonFoldingFan() : YxCardModel(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Character Exclusive.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.CharacterExclusiveWuCe];

    /// <summary>Gain 'Star Moon Folding Fan' power; Draw cards if star power is enough.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxStarMoonFoldingFanPower>(1),
        new PowerVar<YxStarPowerPower>(3),
        new CardsVar(2),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.CharacterExclusiveWuCe.GetHoverTip(),
        HoverTipFactory.FromPower<YxStarPointPower>(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Glow if we have enough star power.</summary>
    protected override bool ShouldGlowGoldInternal =>
        Owner.Creature.GetPower<YxStarPowerPower>()?.Amount >= DynamicVars[nameof(YxStarPowerPower)].BaseValue;

    /// <summary>Reduce energy cost.</summary>
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    /// <summary>Gain 'Star Moon Folding Fan' power; Draw cards if star power is enough.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxStarMoonFoldingFanPower>(
            Owner.Creature,
            DynamicVars[nameof(YxStarMoonFoldingFanPower)].BaseValue,
            Owner.Creature,
            this
        );

        if (ShouldGlowGoldInternal)
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }
}

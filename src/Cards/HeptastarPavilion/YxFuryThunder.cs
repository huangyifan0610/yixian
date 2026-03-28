using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Fury Thunder.</summary>
public sealed class YxFuryThunder() : YxCardModel(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Character Exclusive.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [
        YxCardKeyword.CharacterExclusiveYanChen,
        YxCardKeyword.Thunder,
    ];

    /// <summary>Gain 'Fury Thunder' power; Draw cards.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxFuryThunderPower>(15),
        new CardsVar(1),
    ];

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxFuryThunderPower)].UpgradeValueBy(5);

    /// <summary>Gain 'Fury Thunder' power; Draw cards.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxFuryThunderPower>(
            Owner.Creature,
            DynamicVars[nameof(YxFuryThunderPower)].BaseValue,
            Owner.Creature,
            this
        );
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
}

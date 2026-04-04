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

/// <summary>Heptastar Pavilion - Covert Shift.</summary>
public sealed class YxCovertShift() : YxCardModel(3, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Ethereal, Exhaust.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, CardKeyword.Exhaust];

    /// <summary>Gain Covert Shift power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxCovertShiftPower>(1),
    ];

    /// <summary>Remove ethereal keyword.</summary>
    protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Ethereal);

    /// <summary>Gain Covert Shift power.</summary>
    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay)
            .ContinueWith(_ => PowerCmd.Apply<YxCovertShiftPower>(
                Owner.Creature,
                DynamicVars[nameof(YxCovertShiftPower)].BaseValue,
                Owner.Creature,
                this
            ));
}

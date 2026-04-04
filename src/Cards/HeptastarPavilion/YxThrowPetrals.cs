using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Throw Petrals.</summary>
public sealed class YxThrowPetrals() : YxCardModel(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain 'Throw Petrals' power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxThrowPetralsPower>(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<PoisonPower>(),
    ];

    /// <summary>Become innate.</summary>
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);

    /// <summary>Gain 'Throw Petrals' power.</summary>
    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay)
            .ContinueWith(_ => PowerCmd.Apply<YxThrowPetralsPower>(
                Owner.Creature,
                DynamicVars[nameof(YxThrowPetralsPower)].BaseValue,
                Owner.Creature,
                this
            ));
}

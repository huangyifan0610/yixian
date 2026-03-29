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

/// <summary>Heptastar Pavilion - Preemptive Strike.</summary>
public sealed class YxPreemptiveStrike() : YxCardModel(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain Preemptive Strike power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxPreemptiveStrikePower>(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.PostAction.GetHoverTip(),
    ];

    /// <summary>Become innate.</summary>
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);

    /// <summary>Gain Preemptive Strike power.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxPreemptiveStrikePower>(
            Owner.Creature,
            DynamicVars[nameof(YxPreemptiveStrikePower)].BaseValue,
            Owner.Creature,
            this
        );
    }
}

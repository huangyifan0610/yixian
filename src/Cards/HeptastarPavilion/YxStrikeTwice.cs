using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Strike Twice.</summary>
public sealed class YxStrikeTwice() : YxCardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private static readonly LocString SELECTION_PROMPT = new("card_selection", "YX_STRIKE_TWICE");

    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Exhaust.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    /// <summary>Choose card to gain replay.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic)
    ];

    /// <summary>Reduce energy cost.</summary>
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    /// <summary>Choose card to gain replay.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        var selected = await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            new CardSelectorPrefs(SELECTION_PROMPT, DynamicVars.Cards.IntValue),
            null,
            this
        );

        foreach (var card in selected)
        {
            ++card.BaseReplayCount;
        }
    }
}

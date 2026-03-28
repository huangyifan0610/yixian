using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Solitary Void Golden Scroll.</summary>
public sealed class YxSolitaryVoidGoldenScroll() : YxCardModel(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Character Exclusive.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.CharacterExclusiveFengXu];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.CharacterExclusiveFengXu.GetHoverTip(),
        HoverTipFactory.FromCard<YxFaceIsolation>(IsUpgraded),
        HoverTipFactory.FromCard<YxStrikeVacuity>(IsUpgraded),
    ];

    /// <summary>Adds 'Face Isolation' into draw pile; Adds 'Strike Vacuity' into discard pile.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, nameof(CombatState));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        var faceIsolation = CombatState.CreateCard<YxFaceIsolation>(Owner);
        var strikeVacuity = CombatState.CreateCard<YxStrikeVacuity>(Owner);

        if (IsUpgraded)
        {
            CardCmd.Upgrade(faceIsolation);
            CardCmd.Upgrade(strikeVacuity);
        }

        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(faceIsolation, PileType.Draw, true, CardPilePosition.Random));
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(strikeVacuity, PileType.Discard, true, CardPilePosition.Random));
    }
}

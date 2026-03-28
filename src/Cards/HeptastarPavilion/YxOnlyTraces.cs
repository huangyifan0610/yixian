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

/// <summary>Heptastar Pavilion - Only Traces.</summary>
public sealed class YxOnlyTraces() : YxCardModel(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Character Exclusive; Post Action.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [
        YxCardKeyword.CharacterExclusiveTanShuyan,
        YxCardKeyword.PostAction,
    ];

    /// <summary>Post Action: Gain enegry; Draw cards.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxTemporaryHpPower>(2),
        new CardsVar(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.CharacterExclusiveTanShuyan.GetHoverTip(),
        YxCardKeyword.PostAction.GetHoverTip(),
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <summary>Glow for post action.</summary>
    protected override bool ShouldGlowGoldInternal => PostAction;

    /// <summary>Draw more cards.</summary>
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1);

    /// <summary>Post Action: Gain enegry; Draw cards.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxTemporaryHpPower>(
            Owner.Creature,
            DynamicVars[nameof(YxTemporaryHpPower)].BaseValue,
            Owner.Creature,
            this
        );

        if (await TryPostAction(choiceContext, cardPlay))
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }
}

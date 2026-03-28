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

/// <summary>Heptastar Pavilion - Great Spirit.</summary>
public sealed class YxGreatSpirit() : YxCardModel(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Post Action.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.PostAction];

    /// <summary>Draw cards; Post Action: Gain temporary HP.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(4),
        new PowerVar<YxTemporaryHpPower>(18),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.PostAction.GetHoverTip(),
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <summary>Glow for post action.</summary>
    protected override bool ShouldGlowGoldInternal => PostAction;

    /// <summary>Gain more temporary HP.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxTemporaryHpPower)].UpgradeValueBy(6);

    /// <summary>Draw cards; Post Action: Gain temporary HP.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);

        if (await TryPostAction(choiceContext, cardPlay))
        {
            await PowerCmd.Apply<YxTemporaryHpPower>(
                Owner.Creature,
                DynamicVars[nameof(YxTemporaryHpPower)].BaseValue,
                Owner.Creature,
                this
            );
        }
    }
}

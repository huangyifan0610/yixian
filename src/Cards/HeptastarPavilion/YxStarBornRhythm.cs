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

/// <summary>Heptastar Pavilion - Star Born Rhythm.</summary>
public sealed class YxStarBornRhythm() : YxCardModel(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Post Action.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.PostAction];

    /// <summary>Gain Star Power; Post Action: Draw cards.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxStarPowerPower>(2),
        new CardsVar(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.PostAction.GetHoverTip(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Glow for post action.</summary>
    protected override bool ShouldGlowGoldInternal => PostAction;

    /// <summary>Gain more Star Power.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxStarPowerPower)].UpgradeValueBy(1);

    /// <summary>Gain Star Power; Post Action: Draw cards.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxStarPowerPower>(
            Owner.Creature,
            DynamicVars[nameof(YxStarPowerPower)].BaseValue,
            Owner.Creature,
            this
        );

        if (await TryPostAction(choiceContext, cardPlay))
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }
}

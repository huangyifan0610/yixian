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

/// <summary>Heptastar Pavilion - Imposing.</summary>
public sealed class YxImposing() : YxCardModel(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Post Action.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.PostAction];

    /// <summary>Gain temporary HP; Post Action: gain energy.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxTemporaryHpPower>(4),
        new EnergyVar(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.PostAction.GetHoverTip(),
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <summary>Glow for post action.</summary>
    protected override bool ShouldGlowGoldInternal => PostAction;

    /// <summary>Gain more energy.</summary>
    protected override void OnUpgrade() => DynamicVars.Energy.UpgradeValueBy(1);

    /// <summary>Gain temporary HP; Post Action: gain energy.</summary>
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
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }
}

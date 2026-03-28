using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Hunter Becomes Preyer.</summary>
public sealed class YxHunterBecomesPreyer() : YxCardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Post Action.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.PostAction];

    /// <summary>Gain temporary HP; Post Action: deal damage to all enemies.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxTemporaryHpPower>(6),
        new DamageVar(4, ValueProp.Move),
        new RepeatVar(3),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.PostAction.GetHoverTip(),
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <summary>Glow for post action.</summary>
    protected override bool ShouldGlowGoldInternal => PostAction;

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2);

    /// <summary>Gain temporary HP; Post Action: deal damage to all enemies.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, nameof(CombatState));
        await PowerCmd.Apply<YxTemporaryHpPower>(
            Owner.Creature,
            DynamicVars[nameof(YxTemporaryHpPower)].BaseValue,
            Owner.Creature,
            this
        );

        if (await TryPostAction(choiceContext, cardPlay))
        {
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .WithWaitBeforeHit(0.25f, 0.35f)
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .Execute(choiceContext);
        }
    }
}

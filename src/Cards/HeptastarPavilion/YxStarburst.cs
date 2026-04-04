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

/// <summary>Heptastar Pavilion - Starburst.</summary>
public sealed class YxStarburst() : YxCardModel(0, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Character Exclusive.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.CharacterExclusiveJiangXiming];

    /// <summary>Deal damage; Double damage after consuming Star Power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(9, ValueProp.Move),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.CharacterExclusiveJiangXiming.GetHoverTip(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Glow if we have star power.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPower<YxStarPowerPower>()?.Amount >= 1m;

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));

        decimal damage = DynamicVars.Damage.BaseValue;
        var starPower = Owner.Creature.GetPower<YxStarPowerPower>();
        if (starPower?.Amount >= 1m)
        {
            await PowerCmd.ModifyAmount(starPower, -1, Owner.Creature, this);
            damage += DynamicVars.Damage.BaseValue;
        }

        await DamageCmd
            .Attack(damage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }
}

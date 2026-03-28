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
using Yixian.Vars;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Five Thunders.</summary>
public sealed class YxFiveThunders() : YxCardModel(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    /// <summary>Five, Literally in the card name.</summary>
    private const int FIVE = 5;

    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Thunder.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Thunder];

    /// <summary>Repeat five times: have chance to deal damage to all enemies.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ChanceVar(30),
        new DamageVar(12, ValueProp.Move),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Thunder.GetHoverTip(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have enough hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPower<YxHexagramPower>()?.Amount >= FIVE;

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2);

    /// <summary>Repeat five times: have chance to deal damage to all enemies.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        ArgumentNullException.ThrowIfNull(CombatState, nameof(CombatState));
        var hexagram = Owner.Creature.GetPower<YxHexagramPower>();

        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(hexagram.Test(RunState, DynamicVars[ChanceVar.KEY].BaseValue, FIVE, out int _))
            .WithHitFx("vfx/vfx_attack_lightning")
            .WithWaitBeforeHit(0.35f, 0.4f)
            .FromCard(this)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);
    }
}

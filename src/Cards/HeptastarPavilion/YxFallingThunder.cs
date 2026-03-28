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

/// <summary>Heptastar Pavilion - Falling Thunder.</summary>
public sealed class YxFallingThunder() : YxCardModel(2, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Thunder.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Thunder];

    /// <summary>Deal random damage to all enemies.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar("MinDamage", 12m, ValueProp.Move),
        new DamageVar("MaxDamage", 22m, ValueProp.Move),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Thunder.GetHoverTip(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<YxHexagramPower>();

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars["MaxDamage"].UpgradeValueBy(6);

    /// <summary>Deal random damage to all enemies.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        ArgumentNullException.ThrowIfNull(CombatState, nameof(CombatState));
        await DamageCmd
            .Attack(Owner.Creature.GetPower<YxHexagramPower>().Range(RunState, DynamicVars["MinDamage"].IntValue, DynamicVars["MaxDamage"].IntValue, out bool _))
            .WithHitFx("vfx/vfx_attack_lightning")
            .FromCard(this)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);
    }
}

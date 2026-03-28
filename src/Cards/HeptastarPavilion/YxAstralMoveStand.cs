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

/// <summary>Heptastar Pavilion - Astral Move Stand.</summary>
public sealed class YxAstralMoveStand() : YxCardModel(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Astral Move.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.AstralMove];

    /// <summary>Deal damage; Draw energies on star point.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6, ValueProp.Move),
        new EnergyVar(2),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.AstralMove.GetHoverTip(),
        HoverTipFactory.FromPower<YxStarPointPower>(),
    ];

    /// <summary>Glow if on star point.</summary>
    protected override bool ShouldGlowGoldInternal => IsOnStarPoint;

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3);

    /// <summary>Deal damage; Draw energies on star point.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        if (IsOnStarPoint)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }
}

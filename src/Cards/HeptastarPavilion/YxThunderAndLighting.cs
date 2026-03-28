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

/// <summary>Heptastar Pavilion - Thunder And Lighting.</summary>
public sealed class YxThunderAndLighting() : YxCardModel(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Thunder.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Thunder];

    /// <summary>Repeat: deal random damage; gain energy if hexagram is consumed.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new RepeatVar(2),
        new DamageVar(1, ValueProp.Move),
        new ExtraDamageVar(10),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Thunder.GetHoverTip(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have enough hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPower<YxHexagramPower>()?.Amount >= DynamicVars.Repeat.BaseValue;

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.ExtraDamage.UpgradeValueBy(3);

    /// <summary>Repeat: deal random damage; gain energy if hexagram is consumed.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));

        int repeat = DynamicVars.Repeat.IntValue;
        var hexagram = Owner.Creature.GetPower<YxHexagramPower>();

        for (int times = 0; times < repeat; ++times)
        {
            await DamageCmd
                .Attack(hexagram.Range(RunState, DynamicVars.Damage.IntValue, DynamicVars.ExtraDamage.IntValue, out bool used))
                .WithWaitBeforeHit(0.25f, 0.35f)
                .WithHitFx("vfx/vfx_attack_lightning")
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);

            if (used)
            {
                await PlayerCmd.GainEnergy(1, Owner);
            }
        }
    }
}

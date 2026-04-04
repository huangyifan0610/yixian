using System;
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
using Yixian.Vars;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Thin On The Ground.</summary>
public sealed class YxThinOnTheGround() : YxCardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Exhaust.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    /// <summary>Gain temporary HP and Max HP; Have chance to double.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxTemporaryHpPower>(10),
        new MaxHpVar(1),
        new ChanceVar(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<YxHexagramPower>();

    /// <summary>Gain more Max HP.</summary>
    protected override void OnUpgrade() => DynamicVars.MaxHp.UpgradeValueBy(1);

    /// <summary>Gain temporary HP and Max HP; Have chance to double.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        decimal doubled = Owner.Creature.GetPower<YxHexagramPower>().Test(RunState, DynamicVars[ChanceVar.KEY].BaseValue, out bool _) ? 2m : 1m;
        await PowerCmd.Apply<YxTemporaryHpPower>(
            Owner.Creature,
            DynamicVars[nameof(YxTemporaryHpPower)].BaseValue * doubled,
            Owner.Creature,
            this
        );
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue * doubled);
    }
}

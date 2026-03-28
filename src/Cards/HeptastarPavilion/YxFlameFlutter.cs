using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Flame Flutter.</summary>
public sealed class YxFlameFlutter() : YxCardModel(0, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Character Exclusive.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.CharacterExclusiveYaoLing];

    /// <summary>Apply random poison.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PoisonPower>("MinPoisonPower", 1),
        new PowerVar<PoisonPower>("MaxPoisonPower", 5),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.CharacterExclusiveYaoLing.GetHoverTip(),
        HoverTipFactory.FromPower<PoisonPower>(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<YxHexagramPower>();

    /// <summary>Apply more poison.</summary>
    protected override void OnUpgrade() => DynamicVars["MaxPoisonPower"].UpgradeValueBy(2);

    /// <summary>Apply random poison.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));

        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<PoisonPower>(
            cardPlay.Target,
            Owner.Creature.GetPower<YxHexagramPower>().Range(
                RunState,
                DynamicVars["MinPoisonPower"].IntValue,
                DynamicVars["MaxPoisonPower"].IntValue,
                out bool _
            ),
            Owner.Creature,
            this
        );
    }
}

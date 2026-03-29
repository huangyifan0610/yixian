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

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Destiny Catastrophe.</summary>
public sealed class YxDestinyCatastrophe() : YxCardModel(3, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Exhaust.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    /// <summary>If have exact hexagram, set target's Max HP.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxHexagramPower>(8),
        new MaxHpVar(8),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have enough hexagram.</summary>
    protected override bool ShouldGlowGoldInternal =>
        Owner.Creature.GetPower<YxHexagramPower>()?.Amount == DynamicVars[nameof(YxHexagramPower)].BaseValue;

    /// <summary>Remove exhaust.</summary>
    protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Exhaust);

    /// <summary>If have exact hexagram, set target's Max HP.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        if (ShouldGlowGoldInternal)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await CreatureCmd.SetMaxHp(cardPlay.Target, DynamicVars.MaxHp.BaseValue);
        }
    }
}

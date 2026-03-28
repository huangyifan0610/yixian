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

/// <summary>Heptastar Pavilion - Zhen Hexagram.</summary>
public sealed class YxZhenHexagram() : YxCardModel(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Hexagram.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Hexagram];

    /// <summary>Gain hexagram; Apply vulnerable.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxHexagramPower>(2),
        new PowerVar<VulnerablePower>(3),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Hexagram.GetHoverTip(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
    ];

    /// <summary>If upgraded, apply vulnerable to all enemies.</summary>
    public override TargetType TargetType => IsUpgraded ? TargetType.AllEnemies : TargetType.AnyEnemy;

    /// <summary>Gain hexagram; Apply vulnerable.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxHexagramPower>(Owner.Creature, DynamicVars[nameof(YxHexagramPower)].BaseValue, Owner.Creature, this);

        if (IsUpgraded)
        {
            ArgumentNullException.ThrowIfNull(CombatState, nameof(CombatState));
            await PowerCmd.Apply<VulnerablePower>(CombatState.Enemies, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
            await PowerCmd.Apply<VulnerablePower>(cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }
    }
}

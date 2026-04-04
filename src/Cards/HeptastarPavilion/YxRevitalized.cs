using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Revitalized.</summary>
public sealed class YxRevitalized() : YxCardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Deal damage for gained temporary HP.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(10),
        new ExtraDamageVar(1),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(CalculatedDamageMultiplier),
        new PowerVar<YxTemporaryHpPower>(3),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <summary>Multiplier for <see cref="CalculatedDamageVar"/>.</summary>
    private static decimal CalculatedDamageMultiplier(CardModel card, Creature? target)
    {
        ArgumentNullException.ThrowIfNull(card.CombatState, nameof(card.CombatState));

        decimal gained = 0;
        foreach (var entry in CombatManager.Instance.History.Entries)
        {
            if (entry.RoundNumber == card.CombatState.RoundNumber
                && entry is PowerReceivedEntry recv
                && recv.Amount > 0
                && recv.Power.Owner == card.Owner.Creature
                && recv.Power is YxTemporaryHpPower)
            {
                gained += recv.Amount;
            }
        }

        return gained / card.DynamicVars[nameof(YxTemporaryHpPower)].BaseValue;
    }

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxTemporaryHpPower)].UpgradeValueBy(-1);

    /// <summary>Deal damage for gained temporary HP.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        await DamageCmd
            .Attack(DynamicVars.CalculatedDamage.Calculate(cardPlay.Target))
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }
}

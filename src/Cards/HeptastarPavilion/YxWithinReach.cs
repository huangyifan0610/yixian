using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Characters;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Within Reach.</summary>
public sealed class YxWithinReach() : YxCardModel(0, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Character Exclusive.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.CharacterExclusiveYanChen];

    /// <summary>Deal damage; Deal extra damage for each DEBUFF.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(3),
        new ExtraDamageVar(3),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(CalculatedDamageMultiplier),
    ];

    /// <summary>Multiplier for <see cref="CalculatedDamageVar"/>.</summary>
    private static decimal CalculatedDamageMultiplier(CardModel card, Creature? target)
    {
        decimal amount = 0;
        if (target != null)
        {
            foreach (var power in target.Powers)
            {
                if (power.Type == PowerType.Debuff)
                {
                    amount += power.Amount;
                }
            }
        }
        return amount;
    }

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.ExtraDamage.UpgradeValueBy(1);

    /// <summary>Deal damage; Deal extra damage for each DEBUFF.</summary>
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

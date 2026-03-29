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
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Characters;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Snake In Reflection.</summary>
public sealed class YxSnakeInReflection() : YxCardModel(0, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Apply poison; Trigger poison on all enemies.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PoisonPower>(3),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<PoisonPower>(),
    ];

    /// <summary>Apply more poison.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(PoisonPower)].UpgradeValueBy(2);

    /// <summary>Apply poison; Trigger poison on all enemies.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, nameof(CombatState));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        await PowerCmd.Apply<PoisonPower>(
            CombatState.HittableEnemies,
            DynamicVars.Poison.BaseValue,
            Owner.Creature,
            this
        );

        foreach (var enemy in CombatState.HittableEnemies)
        {
            var poison = enemy.GetPower<PoisonPower>();
            if (poison != null)
            {
                await CreatureCmd.Damage(
                    choiceContext,
                    enemy,
                    poison.Amount,
                    ValueProp.Unblockable | ValueProp.Unpowered,
                    Owner.Creature,
                    this
                );

                if (enemy.IsAlive)
                {
                    await PowerCmd.Decrement(poison);
                }
                else
                {
                    await Cmd.CustomScaledWait(0.1f, 0.25f);
                }
            }
        }
    }
}

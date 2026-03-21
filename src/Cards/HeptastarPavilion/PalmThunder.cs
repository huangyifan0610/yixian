using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Yixian.Powers;
using Yixian.Patches;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// <c>Palm Thunder</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class PalmThunder() : HeptastarPavilionCardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal at least 2 damage.
        new MinDamageVar(2, ValueProp.Move),
        // Deal at most 10 damage.
        new MaxDamageVar(10, ValueProp.Move),
    ]);

    /// <summary>
    /// Adds star point power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<Hexagram>(),
    ]);

    /// <summary>
    /// Deal random damage.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            await Hexagram.Range(Owner.Creature, this, Owner.RunState, DynamicVars.MinDamage().IntValue, DynamicVars.MaxDamage().IntValue)
                .ContinueWith(async damage => DamageCmd
                    .Attack(await damage)
                    .FromCard(this)
                    .WithHitFx("vfx/vfx_attack_lightning")
                    .Targeting(cardPlay.Target)
                    .Execute(choiceContext)
                );
        }
    }

    /// <summary>
    /// Upgrade the damage.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars.MinDamage().UpgradeValueBy(2);
        DynamicVars.MaxDamage().UpgradeValueBy(4);
    }
}
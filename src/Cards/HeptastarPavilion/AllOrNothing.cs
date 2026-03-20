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
using Yixian.Vars;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// <c>All Or Nothing</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class AllOrNothing() : HeptastarPavilionCardModel(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal at least 1 damage.
        new MinDamageVar(1, ValueProp.Move),
        // Deal at most 20 damage.
        new MaxDamageVar(20, ValueProp.Move),
        // Gain at least 1 block. 
        new MinBlockVar(1, ValueProp.Move),
        // Gain at most 20 block. 
        new MaxBlockVar(20, ValueProp.Move),
    ]);

    /// <summary>
    /// Adds star point power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<HexagramPower>(),
    ]);

    /// <summary>
    /// Deal random damage and gain random block.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            await HexagramPower.Range(Owner.Creature, this, Owner.RunState, DynamicVars.MinDamage().IntValue, DynamicVars.MaxDamage().IntValue)
                .ContinueWith(async damage => DamageCmd
                    .Attack(await damage)
                    .FromCard(this)
                    .Targeting(cardPlay.Target)
                    .Execute(choiceContext)
                );
        }

        await HexagramPower.Range(Owner.Creature, this, Owner.RunState, DynamicVars.MinBlock().IntValue, DynamicVars.MaxBlock().IntValue)
            .ContinueWith(async block => CreatureCmd.GainBlock(Owner.Creature, await block, ValueProp.Move, cardPlay));
    }

    /// <summary>
    /// Upgrade the max damage and max block.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars.MaxDamage().UpgradeValueBy(4);
        DynamicVars.MaxBlock().UpgradeValueBy(4);
    }
}
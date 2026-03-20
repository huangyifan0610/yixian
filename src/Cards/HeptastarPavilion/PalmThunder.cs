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
        new DamageVar(MIN_DAMAGE_VAR, 2, ValueProp.Move),
        // Deal at most 10 hexagrams.
        new DamageVar(MAX_DAMAGE_VAR, 10, ValueProp.Move),
    ]);
    private const string MIN_DAMAGE_VAR = "MinDamage";
    private const string MAX_DAMAGE_VAR = "MaxDamage";

    /// <summary>
    /// Adds star point power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<HexagramPower>(),
    ]);

    /// <summary>
    /// Deal random damage.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            int min = (int)DynamicVars[MIN_DAMAGE_VAR].BaseValue;
            int max = (int)DynamicVars[MAX_DAMAGE_VAR].BaseValue;
            decimal damage = await HexagramPower.Range(Owner.Creature, this, Owner.RunState, min, max);

            // Takes maximum value.
            await DamageCmd
                .Attack(damage)
                .FromCard(this)
                .WithHitFx("vfx/vfx_attack_lightning")
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }
    }

    /// <summary>
    /// Upgrade the damage.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars[MIN_DAMAGE_VAR].UpgradeValueBy(2);
        DynamicVars[MAX_DAMAGE_VAR].UpgradeValueBy(4);
    }
}
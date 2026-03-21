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
/// <c>Astral Fleche</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class AstralFleche() : HeptastarPavilionCardModel(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Deal 3 damage.
        new DamageVar(3, ValueProp.Move),
        // Gain 2 star power.
        new StarPowerVar(2),
    ]);

    /// <summary>
    /// Adds star point power and star power power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<StarPointPower>(),
        HoverTipFactory.FromPower<StarPowerPower>(),
    ]);

    /// <summary>
    /// Deal damage and gain star power.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .Execute(choiceContext);
        }

        await PowerCmd.Apply<StarPowerPower>(Owner.Creature, DynamicVars.StarPower().BaseValue, Owner.Creature, this);

        Main.Logger.Info($"============ {cardPlay.PlayIndex} - {cardPlay.PlayCount}");
    }

    /// <summary>
    /// Upgrade the damage and star power.
    /// </summary>
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars.StarPower().UpgradeValueBy(2);
    }
}
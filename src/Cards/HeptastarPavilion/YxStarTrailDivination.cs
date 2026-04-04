using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Star Trail Divination.</summary>
public sealed class YxStarTrailDivination() : YxCardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Consume hexagram; gain energy and star power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedVar("CalculatedHexagram").WithMultiplier(CalculatedHexagramMultiplier),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Multiplier for <see cref="CalculatedVar"/>.</summary>
    private static decimal CalculatedHexagramMultiplier(CardModel card, Creature? target) =>
        card.Owner.Creature.GetPower<YxHexagramPower>()?.Amount ?? 0;

    /// <summary>Reduce energy cost.</summary>
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    /// <summary>Consume hexagram; gain energy and star power.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var hexagramVar = (CalculatedVar)DynamicVars["CalculatedHexagram"];
        var hexagram = hexagramVar.Calculate(null);
        if (hexagram > 0)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<YxStarPowerPower>(Owner.Creature, hexagram, Owner.Creature, this);
            await PlayerCmd.GainEnergy(hexagram, Owner);
        }
    }
}

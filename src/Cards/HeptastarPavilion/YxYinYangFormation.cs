using System.Collections.Generic;
using System.Threading.Tasks;
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

/// <summary>Heptastar Pavilion - Yin Yang Formation.</summary>
public sealed class YxYinYangFormation() : YxCardModel(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain block and temporary HP for each hexagram.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedVar("CalculatedHexagram").WithMultiplier(CalculatedHexagramMultiplier),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <summary>Gain block if upgraded.</summary>
    public override bool GainsBlock => IsUpgraded;

    /// <summary>Multiplier for <see cref="CalculatedVar"/>.</summary>
    private static decimal CalculatedHexagramMultiplier(CardModel card, Creature? target) =>
        card.Owner.Creature.GetPower<YxHexagramPower>()?.Amount ?? 0;

    /// <summary>Gain block and temporary HP for each hexagram.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var hexagramVar = (CalculatedVar)DynamicVars["CalculatedHexagram"];
        var hexagram = hexagramVar.Calculate(null);
        if (hexagram > 0)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<YxTemporaryHpPower>(Owner.Creature, hexagram, Owner.Creature, this);

            if (IsUpgraded)
            {
                await CreatureCmd.GainBlock(Owner.Creature, hexagram, ValueProp.Move, cardPlay);
            }
        }
    }
}

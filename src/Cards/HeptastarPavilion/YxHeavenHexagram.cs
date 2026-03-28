using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Powers;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Heaven Hexagram.</summary>
public sealed class YxHeavenHexagram() : YxCardModel(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Hexagram.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Hexagram];

    /// <summary>Gain energy; Gain hexagram; Draw cards if hexagram is enough.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(1),
        new PowerVar<YxHexagramPower>(1),
        new PowerVar<YxHexagramPower>("YxRequiredHexagram", 3),
        new CardsVar(2),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Hexagram.GetHoverTip(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if hexagram is enough.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPower<YxHexagramPower>()?.Amount >= DynamicVars["YxRequiredHexagram"].BaseValue;

    /// <summary>Gain more hexagram.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxHexagramPower)].UpgradeValueBy(1);

    /// <summary>Gain energy; Gain hexagram; Draw cards if enough hexagram.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        var hexagram = await PowerCmd.Apply<YxHexagramPower>(Owner.Creature, DynamicVars[nameof(YxHexagramPower)].BaseValue, Owner.Creature, this);
        if (hexagram?.Amount >= DynamicVars["YxRequiredHexagram"].BaseValue)
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
    }
}

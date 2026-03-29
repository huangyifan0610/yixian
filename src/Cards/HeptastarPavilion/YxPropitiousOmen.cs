using System;
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

/// <summary>Heptastar Pavilion - Propitious Omen.</summary>
public sealed class YxPropitiousOmen() : YxCardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Exhaust.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    /// <summary>Select to gain energy, hexagram or star power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(5),
        new PowerVar<YxHexagramPower>(5),
        new PowerVar<YxStarPowerPower>(5),
    ];

    /// <summary>Gain more energy, hexagram or star power.</summary>
    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(2);
        DynamicVars[nameof(YxHexagramPower)].UpgradeValueBy(2);
        DynamicVars[nameof(YxStarPowerPower)].UpgradeValueBy(2);
    }

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Select to gain energy, hexagram or star power.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, nameof(CombatState));
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        var energy = CombatState.CreateCard<YxPropitiousOmenEnergy>(Owner);
        var hexagram = CombatState.CreateCard<YxPropitiousOmenHexagram>(Owner);
        var starPower = CombatState.CreateCard<YxPropitiousOmenStarPower>(Owner);

        if (IsUpgraded)
        {
            CardCmd.Upgrade(energy);
            CardCmd.Upgrade(hexagram);
            CardCmd.Upgrade(starPower);
        }

        energy.DynamicVars.Energy.BaseValue = DynamicVars.Energy.BaseValue;
        hexagram.DynamicVars[nameof(YxHexagramPower)].BaseValue = DynamicVars[nameof(YxHexagramPower)].BaseValue;
        starPower.DynamicVars[nameof(YxStarPowerPower)].BaseValue = DynamicVars[nameof(YxStarPowerPower)].BaseValue;

        var selected = await CardSelectCmd.FromChooseACardScreen(choiceContext, [energy, hexagram, starPower], Owner);
        if (selected == energy)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
        else if (selected == hexagram)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<YxHexagramPower>(
                Owner.Creature,
                DynamicVars[nameof(YxHexagramPower)].BaseValue,
                Owner.Creature,
                this
            );
        }
        else if (selected == starPower)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<YxStarPowerPower>(
                Owner.Creature,
                DynamicVars[nameof(YxStarPowerPower)].BaseValue,
                Owner.Creature,
                this
            );
        }
    }
}

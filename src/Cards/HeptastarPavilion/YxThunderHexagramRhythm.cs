using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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

/// <summary>Heptastar Pavilion - Thunder Hexagram Rhythm.</summary>
public sealed class YxThunderHexagramRhythm() : YxCardModel(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Thunder.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Thunder];

    /// <summary>Deal random damage; Gain used Hexagram.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(1, ValueProp.Move),
        new ExtraDamageVar(5),
        new CalculationBaseVar(0),
        new CalculationExtraVar(1),
        new CalculatedVar("CalculatedHexagram").WithMultiplier(CalculatedHexagramMultiplyer)
    ];

    /// <summary>Returns the used hexagrams.</summary>
    private static decimal CalculatedHexagramMultiplyer(CardModel card, Creature? target)
    {
        ArgumentNullException.ThrowIfNull(card.CombatState, nameof(card.CombatState));

        decimal consumed = 0;
        foreach (var entry in CombatManager.Instance.History.Entries)
        {
            if (entry.RoundNumber == card.CombatState.RoundNumber
                && entry is PowerReceivedEntry recv
                && recv.Amount < 0
                && recv.Power.Owner == card.Owner.Creature
                && recv.Power is YxHexagramPower)
            {
                consumed -= recv.Amount;
            }
        }

        return consumed;
    }

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Thunder.GetHoverTip(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Glow if we have hexagram.</summary>
    protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<YxHexagramPower>();

    /// <summary>Deal more damage.</summary>
    protected override void OnUpgrade() => DynamicVars.ExtraDamage.UpgradeValueBy(5);

    /// <summary>Deal random damage; Gain used Hexagram.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
        ArgumentNullException.ThrowIfNull(RunState, nameof(RunState));
        await DamageCmd
            .Attack(Owner.Creature.GetPower<YxHexagramPower>().Range(RunState, DynamicVars.Damage.IntValue, DynamicVars.ExtraDamage.IntValue, out bool _))
            .WithHitFx("vfx/vfx_attack_lightning")
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        var hexagramVar = (CalculatedVar)DynamicVars["CalculatedHexagram"];
        decimal hexagram = hexagramVar.Calculate(cardPlay.Target);
        if (hexagram > 0)
        {
            await PowerCmd.Apply<YxHexagramPower>(Owner.Creature, hexagram, Owner.Creature, this);
        }
    }
}

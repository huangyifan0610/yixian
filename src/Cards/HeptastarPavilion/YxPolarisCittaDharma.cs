using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Yixian.Characters;
using Yixian.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Combat;
using System.Linq;
using MegaCrit.Sts2.Core.Combat.History.Entries;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Polaris Citta Dharma.</summary>
public sealed class YxPolarisCittaDharma() : CardModel(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain 3 star power if played first in the combat.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxStarPowerPower>(3),
    ];

    /// <summary>The card is ethereal.</summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Ethereal,
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxStarPointPower>(),
        HoverTipFactory.FromPower<YxStarPowerPower>(),
    ];

    /// <summary>Let the card be innate.</summary>
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);

    /// <summary>Glow if played first in the combat.</summary>
    protected override bool ShouldGlowGoldInternal => _shouldGlowGoldInternal &&
        (_shouldGlowGoldInternal = !CombatManager.Instance.History.Entries.Any(e => e is CardPlayFinishedEntry cardPlay));
    private bool _shouldGlowGoldInternal = true;

    /// <summary>Gain powers.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Gain polaris citta dharma power.
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxPolarisCittaDharmaPower>(Owner.Creature, 1, Owner.Creature, this);

        // Gain star power if it's played first in the combat.
        if (ShouldGlowGoldInternal)
        {
            await Cmd.CustomScaledWait(0.1f, 0.25f);
            await PowerCmd.Apply<YxStarPowerPower>(Owner.Creature, DynamicVars[nameof(YxStarPowerPower)].BaseValue, Owner.Creature, this);
        }
    }
}

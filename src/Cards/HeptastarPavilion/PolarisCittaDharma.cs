using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Yixian.Powers;
using Yixian.Vars;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// <c>Polaris Citta Dharma</c> in <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class PolarisCittaDharma() : HeptastarPavilionCardModel(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    /// <summary>
    /// The dynamic variables.
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        // Gain 3 star power if it's the first played card.
        new StarPowerVar(3),
    ]);

    /// <summary>
    /// The card is ethereal.
    /// </summary>
    public override IEnumerable<CardKeyword> CanonicalKeywords => base.CanonicalKeywords.Concat([
        CardKeyword.Ethereal,
    ]);

    /// <summary>
    /// Adds star point power and star power power to the hover tips.
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => base.ExtraHoverTips.Concat([
        HoverTipFactory.FromPower<StarPointPower>(),
        HoverTipFactory.FromPower<StarPowerPower>(),
    ]);

    /// <summary>
    /// Glow if post action takes effects.
    /// </summary>
    protected override bool ShouldGlowGoldInternal => IsFirstInCombat;
    private static bool IsFirstInCombat => !CombatManager.Instance.History.Entries.Any(entry => entry is CardPlayFinishedEntry);

    /// <summary>
    /// Gain Polaris Citta Dharma power.
    /// </summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Gain Polaris Citta Dharma power.
        // The amount does not matter.
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<PolarisCittaDharmaPower>(Owner.Creature, 1, Owner.Creature, this);

        // Gains star power if it's played first in the battle.
        if (IsFirstInCombat)
        {
            await PowerCmd.Apply<StarPowerPower>(Owner.Creature, DynamicVars.StarPower().BaseValue, Owner.Creature, this);
        }
    }

    /// <summary>
    /// Removes ethereal effect.
    /// </summary>
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
}
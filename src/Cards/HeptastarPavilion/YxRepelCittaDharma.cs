using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Yixian.Characters;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>Heptastar Pavilion - Repel Citta Dharma.</summary>
public sealed class YxRepelCittaDharma() : YxCardModel(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain thorns.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ThornsPower>(4),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<ThornsPower>(),
    ];

    /// <summary>Gain more thorns.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(ThornsPower)].UpgradeValueBy(2);

    /// <summary>Gain thorns.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<ThornsPower>(
            Owner.Creature,
            DynamicVars[nameof(ThornsPower)].BaseValue,
            Owner.Creature,
            this
        );
    }
}

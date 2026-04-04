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

/// <summary>Heptastar Pavilion - Vitality Blossom.</summary>
public sealed class YxVitalityBlossom() : YxCardModel(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain 'Vitality Blossom' power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxVitalityBlossomPower>(12),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <summary>Heal more HP.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxVitalityBlossomPower)].UpgradeValueBy(4);

    /// <summary>Gain 'Vitality Blossom' power.</summary>
    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay)
            .ContinueWith(_ => PowerCmd.Apply<YxVitalityBlossomPower>(
                Owner.Creature,
                DynamicVars[nameof(YxVitalityBlossomPower)].BaseValue,
                Owner.Creature,
                this
            ));
}

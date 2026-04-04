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

/// <summary>Heptastar Pavilion - Spiritual Divination.</summary>
public sealed class YxSpiritualDivination() : YxCardModel(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain 'Spiritual Divination Dharma' power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxSpiritualDivinationPower>(1),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
    ];

    /// <summary>Reduce energy cost.</summary>
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    /// <summary>Gain 'Spiritual Divination Dharma' power.</summary>
    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) =>
        CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay)
            .ContinueWith(_ => PowerCmd.Apply<YxSpiritualDivinationPower>(
                Owner.Creature,
                DynamicVars[nameof(YxSpiritualDivinationPower)].BaseValue,
                Owner.Creature,
                this
            ));
}

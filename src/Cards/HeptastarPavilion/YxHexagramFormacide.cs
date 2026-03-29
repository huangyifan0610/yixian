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

/// <summary>Heptastar Pavilion - Hexagram Formacide.</summary>
public sealed class YxHexagramFormacide() : YxCardModel(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Gain 'Hexagram Formacide' Power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxHexagramFormacidePower>(3),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Gain more power.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxHexagramFormacidePower)].UpgradeValueBy(1);

    /// <summary>Gain 'Hexagram Formacide' Power.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxHexagramFormacidePower>(
            Owner.Creature,
            DynamicVars[nameof(YxHexagramFormacidePower)].BaseValue,
            Owner.Creature,
            this
        );
    }
}

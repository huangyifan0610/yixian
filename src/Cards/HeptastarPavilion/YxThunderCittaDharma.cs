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

/// <summary>Heptastar Pavilion - Thunder Citta Dharma.</summary>
public sealed class YxThunderCittaDharma() : YxCardModel(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Thunder.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Thunder];

    /// <summary>Gain 'Thunder Citta Dharma' power.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxThunderCittaDharmaPower>(40),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Thunder.GetHoverTip(),
    ];

    /// <summaryGain more power.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxThunderCittaDharmaPower)].UpgradeValueBy(10);

    /// <summary>Gain 'Thunder Citta Dharma' power.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxThunderCittaDharmaPower>(
            Owner.Creature,
            DynamicVars[nameof(YxThunderCittaDharmaPower)].BaseValue,
            Owner.Creature,
            this
        );
    }
}

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

/// <summary>Heptastar Pavilion - Mountain Hexagram.</summary>
public sealed class YxMountainHexagram() : YxCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Hexagram.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Hexagram];

    /// <summary>Gain temporary HP; Gain hexagram.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxTemporaryHpPower>(6),
        new PowerVar<YxHexagramPower>(2),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Hexagram.GetHoverTip(),
        HoverTipFactory.FromPower<YxTemporaryHpPower>(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Gain more hexagram.</summary>
    protected override void OnUpgrade() => DynamicVars[nameof(YxHexagramPower)].UpgradeValueBy(1);

    /// <summary>Gain temporary HP; Gain hexagram.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxTemporaryHpPower>(Owner.Creature, DynamicVars[nameof(YxTemporaryHpPower)].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<YxHexagramPower>(Owner.Creature, DynamicVars[nameof(YxHexagramPower)].BaseValue, Owner.Creature, this);
    }
}

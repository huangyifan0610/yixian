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

/// <summary>Heptastar Pavilion - Lake Hexagram.</summary>
public sealed class YxLakeHexagram() : YxCardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    /// <summary>See <see cref="YxHeptastarPavilionCardPool"/>.</summary>
    public override CardPoolModel Pool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Hexagram.</summary>
    public override IEnumerable<YxCardKeyword> CanonicalYxKeywords => [YxCardKeyword.Hexagram];

    /// <summary>Gain hexagram. Draw cards.</summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<YxHexagramPower>(3),
        new CardsVar(2),
    ];

    /// <summary>Adds necessary hover tips.</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        YxCardKeyword.Hexagram.GetHoverTip(),
        HoverTipFactory.FromPower<YxHexagramPower>(),
    ];

    /// <summary>Draw more cards.</summary>
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1);

    /// <summary>Gain hexagram. Draw cards.</summary>
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<YxHexagramPower>(Owner.Creature, DynamicVars[nameof(YxHexagramPower)].BaseValue, Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
}

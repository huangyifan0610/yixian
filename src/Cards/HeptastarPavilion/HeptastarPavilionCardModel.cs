using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// The abstract card model of <c>Heptastar Pavilion</c>.
/// </summary>
/// <remarks>
/// Forwarded constructor.
/// </remarks>
public abstract class HeptastarPavilionCardModel(int canonicalEnergyCost, CardType type, CardRarity rarity, TargetType targetType)
    : CardModel(canonicalEnergyCost, type, rarity, targetType)
{
    /// <summary>
    /// The <c>Heptastar Pavilion</c> card pool.
    /// </summary>
    public override CardPoolModel Pool => ModelDb.CardPool<HeptastarPavilionCardPool>();
}
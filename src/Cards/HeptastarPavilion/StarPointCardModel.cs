using System.Collections.Generic;
using System.Linq;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Yixian.Cards.HeptastarPavilion;

/// <summary>
/// The abstract card model of <c>Heptastar Pavilion</c>.
/// </summary>
/// <remarks>
/// Forwarded constructor.
/// </remarks>
public abstract class StarPointCardModel(int canonicalEnergyCost, CardType type, CardRarity rarity, TargetType targetType)
    : HeptastarPavilionCardModel(canonicalEnergyCost, type, rarity, targetType)
{
    /// <summary>
    /// Boolean variable indicating whether the card is on star point.
    /// </summary>
    public BoolVar IsStarPointVar => (BoolVar)DynamicVars[IS_STAR_POINT_VAR];

    protected const string IS_STAR_POINT_VAR = "IsStarPoint";

    protected override IEnumerable<DynamicVar> CanonicalVars => base.CanonicalVars.Concat([
        new BoolVar(IS_STAR_POINT_VAR, false)
    ]);

    /// <summary>
    /// Shows glowing outline if the card is on the star point.
    /// </summary>
    protected override bool ShouldGlowGoldInternal => IsStarPointVar.BoolVal;
}
using Godot;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using Yixian.Cards.HeptastarPavilion;

namespace Yixian.Cards;

/// <summary>
/// The card pool of <c>Heptastar Pavilion</c>.
/// </summary>
public sealed class HeptastarPavilionCardPool : CardPoolModel
{
    /// <summary>
    /// The card pool title.
    /// </summary>
    public override string Title => "heptastar_pavilion";

    /// <summary>
    /// Asset "res://materials/cards/frames/card_frame_heptastar_pavilion_mat.tres".
    /// </summary>
    public override string CardFrameMaterialPath => "card_frame_" + Title;

    /// <summary>
    /// Asset "res://images/atlases/ui_atlas.sprites/card/energy_qi.tres".
    /// </summary>
    public override string EnergyColorName => "qi";

    /// <summary>
    /// Color Preview: #3070C6.
    /// </summary>
    public override Color EnergyOutlineColor => DEFAULT_ENERGY_OUTLINE_COLOR;
    public static readonly Color DEFAULT_ENERGY_OUTLINE_COLOR = new("3070C6");

    /// <summary>
    /// Color Preview: #8030D6.
    /// </summary>
    public override Color DeckEntryCardColor => DEFAULT_DECK_ENTRY_CARD_COLOR;
    public static readonly Color DEFAULT_DECK_ENTRY_CARD_COLOR = new("3070C6");

    /// <summary>
    /// Not colorless card pool.
    /// </summary>
    public override bool IsColorless => false;

    /// <summary>`
    /// Returns all cards in the pool.
    /// </summary>
    protected override CardModel[] GenerateAllCards()
    {
        return [
            ModelDb.Card<AllOrNothing>(),
            ModelDb.Card<AstralFleche>(),
            ModelDb.Card<AstralMoveBlock>(),
            ModelDb.Card<AstralMoveCide>(),
            ModelDb.Card<AstralMoveDragonSlay>(),
            ModelDb.Card<AstralMoveFlank>(),
            ModelDb.Card<AstralMoveFly>(),
            ModelDb.Card<AstralMoveHit>(),
            ModelDb.Card<AstralMovePoint>(),
            ModelDb.Card<EarthHexagram>(),
            ModelDb.Card<Incessant>(),
            ModelDb.Card<PalmThunder>(),
            ModelDb.Card<PolarisCittaDharma>(),
        ];
    }

    /// <summary>
    /// Calls <see cref="ModHelper.AddModelToPool"/> for every card in the pool.
    /// </summary>
    internal static void AddModelToPool()
    {
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AllOrNothing>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralFleche>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralMoveBlock>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralMoveCide>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralMoveDragonSlay>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralMoveFlank>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralMoveFly>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralMoveHit>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralMovePoint>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, EarthHexagram>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, Incessant>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, PalmThunder>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, PolarisCittaDharma>();
    }
}
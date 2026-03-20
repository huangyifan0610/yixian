using Godot;
using MegaCrit.Sts2.Core.Models;

namespace Yixian.Cards.HeptastarPavilion;

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

    /// <summary>
    /// Returns all cards in the pool.
    /// </summary>
    protected override CardModel[] GenerateAllCards()
    {
        return [
            ModelDb.Card<AstralMoveFlank>(),
            ModelDb.Card<AstralMoveBlock>(),
            ModelDb.Card<EarthHexagram>(),
            ModelDb.Card<PalmThunder>(),
        ];
    }
}
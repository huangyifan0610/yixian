using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using Yixian.Patches;

namespace Yixian.Characters;

/// <summary>Yixian Heptastar Pavilion Character Model.</summary>
public sealed class YxHeptastarPavilion : YxCharacterModel
{
    // Constants of Heptastar Pavilion.
    public const string ENERGY_COLOR_NAME = "yx_qi";
    public static readonly Color ENERGY_OUTLINE_COLOR = new("3070C6");
    public static readonly Color DECK_ENTRY_CARD_COLOR = new("8030D6");
    public static readonly Color LAB_OUTLINE_COLOR = new("8030D6");

    /// <summary>Unlocked by default.</summary>
    protected override CharacterModel? UnlocksAfterRunAs => null;

    /// <summary>Returns the card pool.</summary>
    public override CardPoolModel CardPool => ModelDb.CardPool<YxHeptastarPavilionCardPool>();

    /// <summary>Returns the relic pool.</summary>
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<YxHeptastarPavilionRelicPool>();

    /// <summary>Returns the potion pool.</summary>
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<YxHeptastarPavilionPotionPool>();

    /// <summary>The real character ID.</summary>
    public YxHeptastarPavilionCharacter Character
    {
        // Loads from save.
        get => (_character ??= SaveManager.Instance.GetYxSerializableRun()?.CharacterHeptastarPavilion) ?? default;
        set => _character = value;
    }
    private YxHeptastarPavilionCharacter? _character = null;

    /// <summary>Returns the character gender.</summary>
    public override CharacterGender Gender => Character.Gender();

    /// <summary>Returns the initial max HP.</summary>
    public override int StartingHp => Character.StartingHp();

    /// <summary>Returns the initial Gold.</summary>
    public override int StartingGold => Character.StartingGold();

    /// <summary>Returns the maximum energy.</summary>
    public override int MaxEnergy => Character.MaxEnergy();

    /// <summary>Returns the color of character name.</summary>
    public override Color NameColor => Character.NameColor();

    /// <summary>Returns the inital deck.</summary>
    public override IEnumerable<CardModel> StartingDeck => Character.StartingDeck();

    /// <summary>Returns the initial relics.</summary>
    public override IReadOnlyList<RelicModel> StartingRelics => Character.StartingRelics();

    /// <summary>Time in seconds to wait before attack.</summary>
    public override float AttackAnimDelay => 0.3f;

    /// <summary>Time in seconds to wait before cast.</summary>
    public override float CastAnimDelay => 0.3f;

    // TODO: Missing map marker image.
    protected override string MapMarkerPath => ImageHelper.GetImagePath("packed/map/icons/map_marker_ironclad.png");

    // TODO: Overrides abstract methods.
    public override Color EnergyLabelOutlineColor => new("801212FF");
    public override Color DialogueColor => new("590700");
    public override Color MapDrawingColor => new("CB282B");
    public override Color RemoteTargetingLineColor => new("E15847FF");
    public override Color RemoteTargetingLineOutline => new("801212FF");
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter",
    ];
}

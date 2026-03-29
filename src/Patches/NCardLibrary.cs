using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using Yixian.Characters;

namespace Yixian.Patches;

/// <summary>Patches <see cref="NCardLibrary"/> to add new card pools to card library.</summary>
[HarmonyPatch(typeof(NCardLibrary), nameof(NCardLibrary._Ready))]
public static class NCardLibrary_Ready
{
    // Private members and methods in "NCardLibrary".
    private static readonly FieldInfo _poolFilters = AccessTools.Field(typeof(NCardLibrary), "_poolFilters");
    private static readonly FieldInfo _cardPoolFilters = AccessTools.Field(typeof(NCardLibrary), "_cardPoolFilters");
    private static readonly FieldInfo _lastHoveredControl = AccessTools.Field(typeof(NCardLibrary), "_lastHoveredControl");
    private static readonly MethodInfo UpdateCardPoolFilter = AccessTools.Method(typeof(NCardLibrary), "UpdateCardPoolFilter", [typeof(NCardPoolFilter)]);

    /// <summary>Adds new card pools to card library.</summary>
    [HarmonyPostfix]
    public static void Postfix(NCardLibrary __instance)
    {
        var scene = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("screens/card_library/library_pool_toggle"));
        var parent = __instance.GetNode<GridContainer>("Sidebar/MarginContainer/TopVBox/PoolFilters");
        var poolFilters = _poolFilters.GetValue(__instance) as Dictionary<NCardPoolFilter, Func<CardModel, bool>>;
        var cardPoolFilters = _cardPoolFilters.GetValue(__instance) as Dictionary<CharacterModel, NCardPoolFilter>;

        ArgumentNullException.ThrowIfNull(poolFilters, nameof(poolFilters));
        ArgumentNullException.ThrowIfNull(cardPoolFilters, nameof(cardPoolFilters));

        var filter = __instance.AddCardPoolFilter<YxHeptastarPavilionCardPool>(scene, parent);
        poolFilters.Add(filter, card => card.Pool is YxHeptastarPavilionCardPool);
        cardPoolFilters.Add(ModelDb.Character<YxHeptastarPavilion>(), filter);
    }

    private static NCardPoolFilter AddCardPoolFilter<T>(this NCardLibrary __instance, PackedScene scene, GridContainer parent)
    where
        T : CardPoolModel
    {
        // Generates scene instance.
        var filter = scene.Instantiate<NCardPoolFilter>();
        var filterImage = filter.GetNode<TextureRect>("Image");
        var filterImageShadow = filterImage.GetNode<TextureRect>("Shadow");
        var filterLowerTitle = ModelDb.CardPool<T>().Title.ToLowerInvariant();

        // Initializes the scene.
        parent.AddChild(filter);
        filter.Name = typeof(T).Name;
        filter.UniqueNameInOwner = true;
        filter.FocusNeighborTop = "../../SearchBar/TextArea";
        filterImage.Texture = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/top_panel/character_icon_" + filterLowerTitle + ".png"));
        filterImage.Material = PreloadManager.Cache.GetMaterial("res://materials/cards/filters/card_filter_" + filterLowerTitle + "_mat.tres");
        filterImageShadow.Texture = filterImage.Texture;
        filter.Connect(NCardPoolFilter.SignalName.Toggled, Callable.From<NCardPoolFilter>(
            filter => UpdateCardPoolFilter.Invoke(__instance, [filter]))
        );
        filter.Connect(Control.SignalName.FocusEntered, Callable.From(
            () => _lastHoveredControl.SetValue(__instance, filter))
        );
        Main.LOGGER.Info("New card pool has been added to the library: " + filter.Name);
        return filter;
    }
}

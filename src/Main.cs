using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Yixian.Cards.HeptastarPavilion;
using Yixian.Relics;

namespace Yixian;

/// <summary>
/// The Mod initializer.
/// </summary>
[ModInitializer(nameof(Run))]
public sealed class Main
{
    /// <summary>
    /// The Mod author.
    /// </summary>
    public static readonly string AUTHOR = "huangyifan0610";

    /// <summary>
    /// A unique ID indentifing the Mod.
    /// </summary>
    public static readonly string ID = "yixian";

    /// <summary>
    /// GitHub repository URL.
    /// </summary>
    public static readonly string URL = "https://github.com/" + AUTHOR + "/" + ID;

    /// <summary>
    /// Mod-level Logger.
    /// </summary>
    public static Logger Logger { get; } = new(ID, LogType.Generic);

    /// <summary>
    /// The initializer method.
    /// </summary>
    public static void Run()
    {
        // Harmony Patches.
        var harmony = new Harmony(ID);
        harmony.PatchAll();

        // Card pool.
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralMoveFlank>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, AstralMoveBlock>();
        ModHelper.AddModelToPool<HeptastarPavilionCardPool, EarthHexagram>();

        // Relic pool.
        ModHelper.AddModelToPool<HeptastarPavilionRelicPool, StarPoint>();
    }
}
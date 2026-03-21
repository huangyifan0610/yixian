using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Yixian.Cards;

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
    public static readonly string Author = "huangyifan0610";

    /// <summary>
    /// A unique ID indentifing the Mod.
    /// </summary>
    public static readonly string Id = "yixian";

    /// <summary>
    /// GitHub repository URL.
    /// </summary>
    public static readonly string Url = "https://github.com/" + Author + "/" + Id;

    /// <summary>
    /// Harmony Patches.
    /// </summary>
    public static readonly Harmony HarmonyPatch = new(Id);

    /// <summary>
    /// Mod-level Logger.
    /// </summary>
    public static Logger Logger { get; } = new(Id, LogType.Generic);

    /// <summary>
    /// The initializer method.
    /// </summary>
    public static void Run()
    {
        HarmonyPatch.PatchAll();
        HeptastarPavilionCardPool.AddModelToPool();
    }
}
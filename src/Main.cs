using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Yixian.Cards.HeptastarPavilion;
using Yixian.Characters;

namespace Yixian;

/// <summary>The Mod initializer.</summary>
[ModInitializer(nameof(Run))]
public sealed class Main
{
    /// <summary>The Mod author.</summary>
    public const string AUTHOR = "huangyifan0610";

    /// <summary>The unique Mod ID.</summary>
    public const string ID = "yixian";

    /// <summary>GitHub repository URL.</summary>
    public const string URL = "https://github.com/" + AUTHOR + "/" + ID;

    /// <summary>A Mod-level Logger.</summary>
    public static readonly Logger LOGGER = new(ID, LogType.Generic);

    /// <summary>Mod initalization.</summary>
    public static void Run()
    {
        // Enables harmony patch.
        var harmony = new Harmony(ID);
        harmony.PatchAll();

        // Enables C# scripts.
        ScriptManagerBridge.LookupScriptsInAssembly(typeof(Main).Assembly);

        // 1) Heptastar Pavilion Card Pool.
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStrikeHeptastarPavilion>();
    }
}

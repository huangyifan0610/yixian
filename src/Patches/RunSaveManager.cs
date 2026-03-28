using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using Yixian.Characters;
using Yixian.Saves;

namespace Yixian.Patches;

/// <summary>Access to private members in <see cref="RunSaveManager"/>.</summary>
public static class RunSaveManagerAccess
{
    private static readonly FieldInfo _saveStore = AccessTools.Field(typeof(RunSaveManager), "_saveStore");
    private static readonly FieldInfo _profileIdProvider = AccessTools.Field(typeof(RunSaveManager), "_profileIdProvider");
    private static readonly FieldInfo _forceSynchronous = AccessTools.Field(typeof(RunSaveManager), "_forceSynchronous");

    internal static ISaveStore GetSaveStore(this RunSaveManager manager)
    {
        var saveStore = _saveStore.GetValue(manager) as ISaveStore;
        ArgumentNullException.ThrowIfNull(saveStore, nameof(saveStore));
        return saveStore;
    }

    internal static IProfileIdProvider GetProfileIdProvider(this RunSaveManager manager)
    {
        var profileIdProvider = _profileIdProvider.GetValue(manager) as IProfileIdProvider;
        ArgumentNullException.ThrowIfNull(profileIdProvider, nameof(profileIdProvider));
        return profileIdProvider;
    }

    internal static bool GetForceSynchronous(this RunSaveManager manager)
    {
        var forceSynchronous = _forceSynchronous.GetValue(manager) as bool?;
        return forceSynchronous ?? false;
    }
}

/// <summary>Extension to <see cref="RunSaveManager"/>.</summary>
public static class RunSaveManagerExtension
{
    internal static ConditionalWeakTable<RunSaveManager, YxSerializableRun> yxSerializableRun = [];

    /// <summary>Returns the loaded save.</summary>
    public static YxSerializableRun? GetYxSerializableRun(this RunSaveManager manager) =>
        yxSerializableRun.TryGetValue(manager, out var serializable) ? serializable : null;

    /// <summary>Returns path to the save.</summary>
    public static string GetCurrentRunSavePath(this RunSaveManager manager) =>
        RunSaveManager.GetRunSavePath(manager.GetProfileIdProvider().CurrentProfileId, "yx_current_run.save");

    /// <summary>Returns path to the multiplayer save.</summary>
    public static string GetCurrentMultiplayerRunSavePath(this RunSaveManager manager) =>
        RunSaveManager.GetRunSavePath(manager.GetProfileIdProvider().CurrentProfileId, "yx_current_run_mp.save");

    /// <summary>Saves the save.</summary>
    internal static async Task OnSave(this RunSaveManager __instance)
    {
        // HACK: "RunManager.Instance.State" is private.
        MethodInfo HackRunManagerState = AccessTools.PropertyGetter(typeof(RunManager), "State");
        var state = HackRunManagerState.Invoke(RunManager.Instance, []) as RunState;
        ArgumentNullException.ThrowIfNull(state, nameof(state));

        using var stream = new MemoryStream();

        var path = RunManager.Instance.NetService.Type.IsMultiplayer()
                ? __instance.GetCurrentMultiplayerRunSavePath()
                : __instance.GetCurrentRunSavePath();

        var value = new YxSerializableRun()
        {
            CharacterHeptastarPavilion = ModelDb.Character<YxHeptastarPavilion>().Character,
            HexagramRngCounter = state.Rng.GetRngForHexagram().Counter
        };

        if (__instance.GetForceSynchronous())
        {
            JsonSerializer.Serialize(stream, value, YxJsonSerializerContext.Default.YxSerializableRun);
            stream.Seek(0L, SeekOrigin.Begin);
            __instance.GetSaveStore().WriteFile(path, stream.ToArray());
        }
        else
        {
            await JsonSerializer.SerializeAsync(stream, value, YxJsonSerializerContext.Default.YxSerializableRun, default);
            stream.Seek(0L, SeekOrigin.Begin);
            await __instance.GetSaveStore().WriteFileAsync(path, stream.ToArray());
        }
    }

    /// <summary>Loads the save.</summary>
    internal static void OnLoad(this RunSaveManager manager, bool multiplayer)
    {
        try
        {
            var path = multiplayer ? manager.GetCurrentMultiplayerRunSavePath() : manager.GetCurrentRunSavePath();
            var json = manager.GetSaveStore().ReadFile(path);
            var serializable = json != null ? JsonSerializer.Deserialize(json, YxJsonSerializerContext.Default.YxSerializableRun) : null;
            if (serializable != null)
            {
                yxSerializableRun.AddOrUpdate(manager, serializable);
            }
        }
        catch (Exception exception)
        {
            Main.LOGGER.Warn("Cannot load save for Yixian Mod, caught exception: " + exception);
        }
    }
}

/// <summary>Patches <see cref="RunSaveManager.SaveRun"/>.</summary>
[HarmonyPatch(typeof(RunSaveManager), nameof(RunSaveManager.SaveRun))]
public static class RunSaveManager_SaveRun
{
    [HarmonyPostfix]
    public static void Postfix(ref Task __result, RunSaveManager __instance) => __result = __result.ContinueWith(_ => __instance.OnSave());
}

/// <summary>Patches <see cref="RunSaveManager.LoadRunSave"/>.</summary>
[HarmonyPatch(typeof(RunSaveManager), nameof(RunSaveManager.LoadRunSave))]
public static class RunSaveManager_LoadRunSave
{
    [HarmonyPostfix]
    public static void Postfix(RunSaveManager __instance) => __instance.OnLoad(multiplayer: false);

}

/// <summary>Patches <see cref="RunSaveManager.LoadMultiplayerRunSave"/>.</summary>
[HarmonyPatch(typeof(RunSaveManager), nameof(RunSaveManager.LoadMultiplayerRunSave))]
public static class RunSaveManager_LoadMultiplayerRunSave
{
    [HarmonyPostfix]
    public static void Postfix(RunSaveManager __instance) => __instance.OnLoad(multiplayer: true);
}

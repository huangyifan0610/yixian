using System;
using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using Yixian.Saves;

namespace Yixian.Patches;

/// <summary>Extension to <see cref="RunSaveManager"/>.</summary>
public static class SaveManagerExtension
{
    private static readonly FieldInfo _runSaveManager = AccessTools.Field(typeof(SaveManager), "_runSaveManager");

    /// <summary>Returns the loaded Run save of Yixian Mod.</summary>
    public static YxSerializableRun? GetYxSerializableRun(this SaveManager manager)
    {
        var runSaveManager = _runSaveManager.GetValue(manager) as RunSaveManager;
        ArgumentNullException.ThrowIfNull(runSaveManager, nameof(_runSaveManager));
        return runSaveManager.GetYxSerializableRun();
    }
}
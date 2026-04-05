using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Saves.Managers;
using MegaCrit.Sts2.Core.Timeline;

namespace Yixian.Patches;

/// <summary>Patches <see cref="ProgressSaveManager.UpdateAfterCombatWon"/>.</summary>
[HarmonyPatch(typeof(ProgressSaveManager), nameof(ProgressSaveManager.UpdateAfterCombatWon))]
public static class ProgressSaveManager_UpdateAfterCombatWon
{
    /// <summary>
    /// Allows exceptions thrown by <see cref="ProgressSaveManager"/> and <see cref="EpochModel.Get"/>.
    /// </summary>
    [HarmonyFinalizer]
    public static Exception? Finalizer(Exception? __exception) =>
        (__exception is ArgumentException) ? null : __exception;
}
using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Events;

namespace Yixian.Patches;

/// <summary>Patches <see cref="TheArchitect.WinRun"/>.</summary>
[HarmonyPatch(typeof(TheArchitect), "WinRun")]
public static class TheArchitect_WinRun
{
    // TODO: The Architect Dialogues.
    [HarmonyFinalizer]
    public static Exception? Finalizer(Exception? __exception) =>
        (__exception is NullReferenceException) ? null : __exception;
}

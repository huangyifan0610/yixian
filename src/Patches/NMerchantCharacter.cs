using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using Yixian.Nodes.Screens.Shops;

namespace Yixian.Patches;

/// <summary>Patches <see cref="NMerchantCharacter.PlayAnimation"/>.</summary>
[HarmonyPatch(typeof(NMerchantCharacter), nameof(NMerchantCharacter.PlayAnimation))]
public static class NMerchantCharacter_PlayAnimation
{
    [HarmonyPrefix]
    public static void Prefix(NMerchantCharacter __instance, ref string anim, bool loop = false)
    {
        if (__instance is MerchantCharacter)
        {
            if (anim == "relaxed_loop")
            {
                anim = "Game_Idle";
            }
            else if (anim == "die")
            {
                anim = "Dead";
            }
        }
    }
}

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
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAllOrNothing>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralFleche>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMoveBlock>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMoveCide>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMoveDragonSlay>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMoveFlank>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMoveFly>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMoveHit>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMovePoint>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMoveStand>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMoveTiger>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxAstralMoveTwinSwallows>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxCovertShift>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxCuttingWeeds>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxDanceOfTheDragonfly>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxDefendHeptastarPavilion>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxDestinyCatastrophe>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxDottedAround>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxDragMoonInSea>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxEarthHexagram>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxEscapePlan>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxFaceIsolation>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxFallingThunder>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxFiveThunders>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxFlameFlutter>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxFlameHexagram>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxFlowersAndWater>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxFlowerSentient>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxFuryThunder>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxGoldenRoosterIndependence>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxGreatSpirit>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxHeavenHexagram>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxHeptastarSoulstat>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxHexagramFormacide>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxHexagramsSpiritResurrection>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxHunterBecomesPreyer>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxHunterHuntingHunter>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxImposing>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxIncessant>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxJadeScrollOfYinSymbol>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxLakeHexagram>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxMeteoriteMeteor>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxMountainHexagram>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxOnlyTraces>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxPalmThunder>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxPerfectlyPlanned>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxPolarisCittaDharma>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxPreemptiveStrike>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxPropitiousOmen>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxPropitiousOmenEnergy>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxPropitiousOmenHexagram>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxPropitiousOmenStarPower>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxQiTherapy>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxRepelCittaDharma>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxRevitalized>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxRotaryDivinationHexagram>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxRuthlessWater>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxShiftingStars>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxSnakeInReflection>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxSolitaryVoidGoldenScroll>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxSparrowsTail>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxSpiritualDivination>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStarBornRhythm>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStarburst>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStarMoonFoldingFan>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStarryMoon>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStarTrailDivination>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStillnessCittaDharma>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStrikeHeptastarPavilion>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStrikeTwice>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxStrikeVacuity>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxSunAndMoonForGlory>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxThinOnTheGround>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxThrowingStonesForDirections>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxThrowPetrals>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxThunderAndLighting>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxThunderCittaDharma>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxThunderHexagramRhythm>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxUltimateHexagramBase>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxVitalityBlossom>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxWaitForGainsWithoutPains>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxWaterDropErosion>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxWaterHexagram>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxWhiteCraneBrightWings>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxWhiteSnake>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxWildHorsesPartTheMane>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxWindHexagram>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxWithinReach>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxYinYangFormation>();
        ModHelper.AddModelToPool<YxHeptastarPavilionCardPool, YxZhenHexagram>();
    }
}


using System;

using HarmonyLib;

using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI.BodyGenerator;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace BannerlordRestartPlus.UI
{
    public class InGameBodyGeneratorScreen : GauntletBodyGeneratorScreen
    {
        internal readonly Action? onComplete;

        public InGameBodyGeneratorScreen(BasicCharacterObject character, bool openedFromMultiplayer, IFaceGeneratorCustomFilter? filter, Action? onComplete): base(character, openedFromMultiplayer, filter)
        {
            this.onComplete = onComplete;
        }
    }

    [HarmonyPatch(typeof(GauntletBodyGeneratorScreen))]
    public static class GauntletBodyGeneratorScreenPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(OnExit))]
        public static void OnExit(ref GauntletBodyGeneratorScreen __instance)
        {
            if (__instance is InGameBodyGeneratorScreen inGameBodyGeneratorScreen)
            {
                inGameBodyGeneratorScreen?.onComplete?.Invoke();
            }
        }
    }
}

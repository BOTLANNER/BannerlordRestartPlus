using System;
using System.Reflection;

using HarmonyLib;

using SandBox.View.CharacterCreation;

using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace BannerlordRestartPlus.Patches.Runtime
{

    public class CharacterCreationStatePatch : IRuntimePatch
    {
        public void PatchAfterMenus(Harmony harmony)
        {
        }

        public void PatchSubmoduleLoad(Harmony harmony)
        {
            MethodInfo method = AccessTools.Method(typeof(CharacterCreationState), nameof(CharacterCreationState.FinalizeCharacterCreation)).GetDeclaredMember();
            harmony.Patch(method, prefix: new HarmonyMethod(typeof(CharacterCreationStatePatch), nameof(CharacterCreationStatePatch.FinalizeCharacterCreationInGame)));


            MethodInfo method2 = AccessTools.Method(typeof(CharacterCreationStageViewBase), nameof(CharacterCreationStageViewBase.HandleEscapeMenu)).GetDeclaredMember();
            harmony.Patch(method2, prefix: new HarmonyMethod(typeof(CharacterCreationStatePatch), nameof(CharacterCreationStatePatch.PreHandleEscapeMenu)), finalizer: new HarmonyMethod(typeof(CharacterCreationStatePatch), nameof(CharacterCreationStatePatch.FixHandleEscapeMenu)));
        }


        public static bool PreHandleEscapeMenu(ref CharacterCreationStageViewBase __instance, CharacterCreationStageViewBase view, ScreenLayer screenLayer)
        {
            if (screenLayer == null || screenLayer.Input == null || view == null || __instance == null)
            {
                return false;
            }

            return true;
        }

        public static Exception FixHandleEscapeMenu(ref Exception __exception)
        {
            if (__exception != null)
            {
                var e = __exception;
                Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
            }
            return null;
        }

        public static bool FinalizeCharacterCreationInGame(ref CharacterCreationState __instance)
        {
            if (CharacterCreationStateExtensions.CharacterCreationState != null && __instance == CharacterCreationStateExtensions.CharacterCreationState)
            {
                CharacterCreationStateExtensions.CharacterCreationState = null;
                __instance.FinalizeCharacterCreationInGame();
                CharacterCreationStateExtensions.MapState = null;
                CharacterCreationStateExtensions.Position = null;
                return false;
            }
            return true;
        }
    }
}

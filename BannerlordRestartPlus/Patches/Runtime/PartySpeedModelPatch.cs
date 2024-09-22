//using System;
//using System.Reflection;

//using HarmonyLib;

//using TaleWorlds.CampaignSystem;
//using TaleWorlds.CampaignSystem.ComponentInterfaces;
//using TaleWorlds.CampaignSystem.Party;
//using TaleWorlds.Library;

//namespace BannerlordRestartPlus.Patches.Runtime
//{
//    public class PartySpeedModelPatch : IRuntimePatch
//    {
//        public void PatchAfterMenus(Harmony harmony)
//        {
//        }

//        public void PatchSubmoduleLoad(Harmony harmony)
//        {
//            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
//            foreach (var assembly in assemblies)
//            {
//                foreach (var type in assembly.GetTypes())
//                {
//                    if (typeof(PartySpeedModel).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
//                    {
//                        MethodInfo? method = AccessTools.Method(type, nameof(CalculateBaseSpeed))?.GetDeclaredMember();
//                        if (method != null)
//                        {
//                            harmony.Patch(method, prefix: new HarmonyMethod(typeof(PartySpeedModelPatch), nameof(PartySpeedModelPatch.CalculateBaseSpeed)), finalizer: new HarmonyMethod(typeof(PartySpeedModelPatch), nameof(PartySpeedModelPatch.FixCalculateBaseSpeed)));
//                        }

//                        MethodInfo? method2 = AccessTools.Method(type, nameof(CalculateFinalSpeed))?.GetDeclaredMember();
//                        if (method2 != null)
//                        {
//                            harmony.Patch(method2, prefix: new HarmonyMethod(typeof(PartySpeedModelPatch), nameof(PartySpeedModelPatch.CalculateFinalSpeed)), finalizer: new HarmonyMethod(typeof(PartySpeedModelPatch), nameof(PartySpeedModelPatch.FixCalculateFinalSpeed)));
//                        }

//                    }
//                }
//            }
//        }

//        public static bool CalculateBaseSpeed(ref ExplainedNumber __result, ref PartySpeedModel __instance, object[] __args) // MobileParty party, bool includeDescriptions = false, int additionalTroopOnFootCount = 0, int additionalTroopOnHorseCount = 0
//        {
//            try
//            {
//                MobileParty party = __args[0] as MobileParty;
//                if (party == null || party.LeaderHero == null || party.Party == null || !party.IsActive || !party.IsReady)
//                {
//                    return false;
//                }
//                return true;
//            }
//            catch (Exception e)
//            {
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return true;
//            }
//        }

//        public static Exception? FixCalculateBaseSpeed(ref Exception? __exception)
//        {
//            if (__exception != null)
//            {
//                var e = __exception;
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//            }
//            return null;
//        }

//        public static bool CalculateFinalSpeed(ref ExplainedNumber __result, ref PartySpeedModel __instance, object[] __args) // MobileParty mobileParty, ExplainedNumber finalSpeed
//        {
//            try
//            {
//                MobileParty mobileParty = __args[0] as MobileParty;
//                if (mobileParty == null || mobileParty.LeaderHero == null || mobileParty.Party == null || !mobileParty.IsActive || !mobileParty.IsReady)
//                {
//                    return false;
//                }
//                return true;
//            }
//            catch (Exception e)
//            {
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return true;
//            }
//        }

//        public static Exception? FixCalculateFinalSpeed(ref Exception? __exception)
//        {
//            if (__exception != null)
//            {
//                var e = __exception;
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//            }
//            return null;
//        }
//    }
//}

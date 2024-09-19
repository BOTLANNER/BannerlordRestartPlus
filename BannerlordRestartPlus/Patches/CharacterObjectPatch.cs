﻿


//using System;
//using System.Collections.Generic;

//using HarmonyLib;

//using TaleWorlds.CampaignSystem;
//using TaleWorlds.Library;

//namespace BannerlordRestartPlus.Patches
//{

//    [HarmonyPatch(typeof(CharacterObject))]
//    public static class CharacterObjectPatch
//    {

//        [HarmonyPrefix]
//        [HarmonyPatch(nameof(AutoGeneratedGetMemberValue_heroObject))]

//        public static bool AutoGeneratedGetMemberValue_heroObject(object o, ref object __result)
//        {
//            try
//            {
//                if (o is Hero h)
//                {
//                    if (h.StringId.Contains("main") || h.StringId.Contains("player") || h.StringId.Contains(HeroPatch.StringIdWatch))
//                    {
//                        if (System.Diagnostics.Debugger.IsAttached)
//                        {
//                            Console.WriteLine("Debugger CharacterObject_heroObject " + h.StringId);
//                        }
//                    }
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
//        [HarmonyPostfix]
//        [HarmonyPatch(nameof(AutoGeneratedGetMemberValue_heroObject))]

//        public static void PostAutoGeneratedGetMemberValue_heroObject(object o, ref object __result)
//        {
//            try
//            {
//                if (o is Hero h)
//                {
//                    if (h.StringId.Contains("main") || h.StringId.Contains("player") || h.StringId.Contains(HeroPatch.StringIdWatch))
//                    {
//                        if (System.Diagnostics.Debugger.IsAttached)
//                        {
//                            Console.WriteLine("Debugger CharacterObject_heroObject " + h.StringId);
//                        }
//                    }
//                }
//                return;
//            }
//            catch (Exception e)
//            {
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return;
//            }
//        }

//        [HarmonyFinalizer]
//        [HarmonyPatch(nameof(AutoGeneratedGetMemberValue_heroObject))]
//        public static Exception FixAutoGeneratedGetMemberValue_heroObject(ref Exception __exception)
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
//        [HarmonyPrefix]
//        [HarmonyPatch(nameof(AutoGeneratedInstanceCollectObjects))]

//        public static bool AutoGeneratedInstanceCollectObjects(ref CharacterObject __instance, List<object> collectedObjects)
//        {
//            try
//            {
//                if (__instance.StringId.Contains("main") || __instance.StringId.Contains("player") || __instance.StringId.Contains(HeroPatch.StringIdWatch))
//                {
//                    if (System.Diagnostics.Debugger.IsAttached)
//                    {
//                        Console.WriteLine("Debugger CharacterObject " + __instance.StringId);
//                    }
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
//        [HarmonyPostfix]
//        [HarmonyPatch(nameof(AutoGeneratedInstanceCollectObjects))]

//        public static void PostAutoGeneratedInstanceCollectObjects(ref CharacterObject __instance, List<object> collectedObjects)
//        {
//            try
//            {
//                if (__instance.StringId.Contains("main") || __instance.StringId.Contains("player") || __instance.StringId.Contains(HeroPatch.StringIdWatch))
//                {
//                    if (System.Diagnostics.Debugger.IsAttached)
//                    {
//                        Console.WriteLine("Debugger CharacterObject " + __instance.StringId);
//                    }
//                }
//                return;
//            }
//            catch (Exception e)
//            {
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return;
//            }
//        }

//        [HarmonyFinalizer]
//        [HarmonyPatch(nameof(AutoGeneratedInstanceCollectObjects))]
//        public static Exception FixAutoGeneratedInstanceCollectObjects(ref Exception __exception)
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
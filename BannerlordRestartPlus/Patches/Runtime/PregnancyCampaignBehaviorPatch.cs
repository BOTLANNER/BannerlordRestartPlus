using System;
using System.Collections;
using System.Reflection;

using HarmonyLib;

using SandBox.View.CharacterCreation;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace BannerlordRestartPlus.Patches.Runtime
{
    public class PregnancyCampaignBehaviorPatch : IRuntimePatch
    {
        public void PatchAfterMenus(Harmony harmony)
        {
        }

        public void PatchSubmoduleLoad(Harmony harmony)
        {
            MethodInfo method = AccessTools.Method(typeof(PregnancyCampaignBehavior), nameof(CheckOffspringsToDeliver), parameters: new Type[] { typeof(Hero) }).GetDeclaredMember();
            harmony.Patch(method, prefix: new HarmonyMethod(typeof(PregnancyCampaignBehaviorPatch), nameof(PregnancyCampaignBehaviorPatch.CheckOffspringsToDeliver)), finalizer: new HarmonyMethod(typeof(PregnancyCampaignBehaviorPatch), nameof(PregnancyCampaignBehaviorPatch.FixCheckOffspringsToDeliver)));


            //var internalType = typeof(PregnancyCampaignBehavior).GetNestedType("Pregnancy", BindingFlags.NonPublic);
            //MethodInfo method2 = AccessTools.Method(typeof(PregnancyCampaignBehavior), nameof(CheckOffspringsToDeliver), parameters: new Type[] { internalType }).GetDeclaredMember();
            //harmony.Patch(method2, prefix: new HarmonyMethod(typeof(PregnancyCampaignBehaviorPatch), nameof(PregnancyCampaignBehaviorPatch.CheckOffspringsToDeliverInternal)), finalizer: new HarmonyMethod(typeof(PregnancyCampaignBehaviorPatch), nameof(PregnancyCampaignBehaviorPatch.FixCheckOffspringsToDeliverInternal)));
        }


        public static bool CheckOffspringsToDeliver(ref PregnancyCampaignBehavior __instance, Hero hero, ref IList ____heroPregnancies)
        {
            try
            {
                return true;
            }
            catch (Exception e)
            {
                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
                return true;
            }
        }

        public static Exception? FixCheckOffspringsToDeliver(ref Exception? __exception, ref PregnancyCampaignBehavior __instance, Hero hero, ref IList ____heroPregnancies)
        {
            if (__exception != null)
            {
                try
                {
                    Type? type = null;
                    FieldInfo? Mother = null;
                    FieldInfo? Father = null;
                    for (int i = ____heroPregnancies.Count - 1; i >= 0; i--)
                    {
                        var preg = ____heroPregnancies[i];
                        if (type == null)
                        {
                            type = preg.GetType();
                        }
                        if (Mother == null)
                        {
                            Mother = type.Field("Mother");
                        }
                        if (Father == null)
                        {
                            Father = type.Field("Father");
                        }
                        var mother = Mother.GetValue(preg);
                        if (Father.GetValue(preg) == hero || mother == hero)
                        {
                            ____heroPregnancies.Remove(preg);
                            if (mother is Hero mh)
                            {
                                mh.IsPregnant = false;
                            }
                        }
                    }
                    hero.IsPregnant = false;
                }
                catch (Exception ee)
                {

                }
                var e = __exception;
                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
            }
            return null;
        }

        //public static bool CheckOffspringsToDeliverInternal(ref PregnancyCampaignBehavior __instance, object[] __args)
        //{
        //    try
        //    {
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
        //        Debug.WriteDebugLineOnScreen(e.ToString());
        //        Debug.SetCrashReportCustomString(e.Message);
        //        Debug.SetCrashReportCustomStack(e.StackTrace);
        //        return true;
        //    }
        //}

        //public static Exception? FixCheckOffspringsToDeliverInternal(ref Exception? __exception)
        //{
        //    if (__exception != null)
        //    {
        //        var e = __exception;
        //        TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
        //        Debug.WriteDebugLineOnScreen(e.ToString());
        //        Debug.SetCrashReportCustomString(e.Message);
        //        Debug.SetCrashReportCustomStack(e.StackTrace);
        //    }
        //    return null;
        //}
    }
}

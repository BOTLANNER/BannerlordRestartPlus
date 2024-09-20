﻿using System;
using System.Reflection;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerlordRestartPlus.Patches
{

    [HarmonyPatch(typeof(Hero))]
    public static class HeroPatch
    {
        public static string StringIdWatch = "qwerty";
        public static FieldInfo CharacterObjectFieldOnHero = AccessTools.Field(typeof(Hero), "_characterObject");

        static MethodInfo Set_battleEquipmentProperty = AccessTools.Property(typeof(Hero), "_battleEquipment").GetSetMethod(true);
        static MethodInfo Set_civilianEquipmentProperty = AccessTools.Property(typeof(Hero), "_civilianEquipment").GetSetMethod(true);

        static MethodInfo SetIsFemaleProperty = AccessTools.Property(typeof(Hero), nameof(Hero.IsFemale)).GetSetMethod(true);
        static MethodInfo SetOccupationProperty = AccessTools.Property(typeof(Hero), nameof(Hero.Occupation)).GetSetMethod(true);


        //[HarmonyPrefix]
        //[HarmonyPatch(nameof(AutoGeneratedGetMemberValue_characterObject))]

        //public static bool AutoGeneratedGetMemberValue_characterObject(object o, ref object __result)
        //{
        //    try
        //    {
        //        if (o is CharacterObject c)
        //        {
        //            if (c.StringId.Contains("main") || c.StringId.Contains("player") || c.StringId.Contains(StringIdWatch))
        //            {
        //                if (System.Diagnostics.Debugger.IsAttached)
        //                {
        //                    Console.WriteLine("Debugger Hero_characterObject " + c.StringId);
        //                }
        //            }
        //        }
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
        //[HarmonyPostfix]
        //[HarmonyPatch(nameof(AutoGeneratedGetMemberValue_characterObject))]

        //public static void PostAutoGeneratedGetMemberValue_characterObject(object o, ref object __result)
        //{
        //    try
        //    {
        //        if (o is CharacterObject c)
        //        {
        //            if (c.StringId.Contains("main") || c.StringId.Contains("player") || c.StringId.Contains(StringIdWatch))
        //            {
        //                if (System.Diagnostics.Debugger.IsAttached)
        //                {
        //                    Console.WriteLine("Debugger Hero_characterObject " + c.StringId);
        //                }
        //            }
        //        }
        //        return;
        //    }
        //    catch (Exception e)
        //    {
        //        TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
        //        Debug.WriteDebugLineOnScreen(e.ToString());
        //        Debug.SetCrashReportCustomString(e.Message);
        //        Debug.SetCrashReportCustomStack(e.StackTrace);
        //        return;
        //    }
        //}

        //[HarmonyFinalizer]
        //[HarmonyPatch(nameof(AutoGeneratedGetMemberValue_characterObject))]
        //public static Exception FixAutoGeneratedGetMemberValue_characterObject(ref Exception __exception)
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
        //[HarmonyPrefix]
        //[HarmonyPatch(nameof(AutoGeneratedInstanceCollectObjects))]

        //public static bool AutoGeneratedInstanceCollectObjects(ref Hero __instance, List<object> collectedObjects)
        //{
        //    try
        //    {
        //        if (__instance.StringId.Contains("main") || __instance.StringId.Contains("player") || __instance.StringId.Contains(StringIdWatch))
        //        {
        //            if (System.Diagnostics.Debugger.IsAttached)
        //            {
        //                Console.WriteLine("Debugger Hero " + __instance.StringId);
        //            }
        //        }
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
        //[HarmonyPostfix]
        //[HarmonyPatch(nameof(AutoGeneratedInstanceCollectObjects))]

        //public static void PostAutoGeneratedInstanceCollectObjects(ref Hero __instance, List<object> collectedObjects)
        //{
        //    try
        //    {
        //        if (__instance.StringId.Contains("main") || __instance.StringId.Contains("player") || __instance.StringId.Contains(StringIdWatch))
        //        {
        //            if (System.Diagnostics.Debugger.IsAttached)
        //            {
        //                Console.WriteLine("Debugger Hero " + __instance.StringId);
        //            }
        //        }
        //        return;
        //    }
        //    catch (Exception e)
        //    {
        //        TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
        //        Debug.WriteDebugLineOnScreen(e.ToString());
        //        Debug.SetCrashReportCustomString(e.Message);
        //        Debug.SetCrashReportCustomStack(e.StackTrace);
        //        return;
        //    }
        //}

        //[HarmonyFinalizer]
        //[HarmonyPatch(nameof(AutoGeneratedInstanceCollectObjects))]
        //public static Exception FixAutoGeneratedInstanceCollectObjects(ref Exception __exception)
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

        [HarmonyPrefix]
        [HarmonyPatch(nameof(SetInitialValuesFromCharacter))]

        public static bool SetInitialValuesFromCharacter(ref Hero __instance, ref float ____defaultAge, ref CampaignTime ____birthDay, CharacterObject characterObject)
        {
            try
            {
                if (characterObject == null)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        Console.WriteLine("Debugger Hero");
                    }
                    characterObject = CharacterObject.All.GetRandomElement();
                }

                //foreach (SkillObject all in Skills.All)
                //{
                //    __instance.SetSkillValue(all, characterObject.GetSkillValue(all));
                //}
                //foreach (TraitObject traitObject in TraitObject.All)
                //{
                //    __instance.SetTraitLevel(traitObject, characterObject.GetTraitLevel(traitObject));
                //}
                //__instance.Level = characterObject.Level;
                //__instance.SetName(characterObject.Name, characterObject.Name);
                //__instance.Culture = characterObject.Culture;
                //__instance.HairTags = characterObject.HairTags;
                //__instance.BeardTags = characterObject.BeardTags;
                //__instance.TattooTags = characterObject.TattooTags;
                //____defaultAge = characterObject.Age;
                //____birthDay = HeroHelper.GetRandomBirthDayForAge(____defaultAge);
                //__instance.HitPoints = characterObject.MaxHitPoints();
                //SetIsFemaleProperty.Invoke(__instance, new object[] { characterObject.IsFemale });
                //SetOccupationProperty.Invoke(__instance, new object[] { __instance.CharacterObject.GetDefaultOccupation() });
                //List<Equipment> list = characterObject.AllEquipments.Where<Equipment>((Equipment t) =>
                //{
                //    if (t.IsEmpty())
                //    {
                //        return false;
                //    }
                //    return !t.IsCivilian;
                //}).ToList<Equipment>();
                //List<Equipment> equipment = characterObject.AllEquipments.Where<Equipment>((Equipment t) =>
                //{
                //    if (t.IsEmpty())
                //    {
                //        return false;
                //    }
                //    return t.IsCivilian;
                //}).ToList<Equipment>();
                //if (list.IsEmpty<Equipment>())
                //{
                //    CultureObject obj = Game.Current.ObjectManager.GetObject<CultureObject>("neutral_culture");
                //    list.AddRange(obj.DefaultBattleEquipmentRoster.AllEquipments);
                //}
                //if (equipment.IsEmpty<Equipment>())
                //{
                //    CultureObject cultureObject = Game.Current.ObjectManager.GetObject<CultureObject>("neutral_culture");
                //    equipment.AddRange(cultureObject.DefaultCivilianEquipmentRoster.AllEquipments);
                //}
                //if (!list.IsEmpty<Equipment>() && !equipment.IsEmpty<Equipment>())
                //{
                //    Equipment item = list[__instance.RandomInt(list.Count)];
                //    Equipment item1 = equipment[__instance.RandomInt(equipment.Count)];
                //    Set_battleEquipmentProperty.Invoke(__instance, new object[] { item.Clone(false) });
                //    Set_civilianEquipmentProperty.Invoke(__instance, new object[] { item1.Clone(false) });
                //}
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

        //[HarmonyFinalizer]
        //[HarmonyPatch(nameof(SetInitialValuesFromCharacter))]
        //public static Exception FixSetInitialValuesFromCharacter(ref Exception __exception)
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
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//using HarmonyLib;

//using Debugger = System.Diagnostics.Debugger;

//using TaleWorlds.CampaignSystem;
//using TaleWorlds.Library;

//namespace BannerlordRestartPlus.Patches.Runtime
//{
//    public class ROTTroopRecruiterPatch : IRuntimePatch
//    {

//        static MethodInfo TraverseTreeMethod;

//        public void PatchAfterMenus(Harmony harmony)
//        {
//        }

//        public void PatchSubmoduleLoad(Harmony harmony)
//        {
//            var rot = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("ROT")).FirstOrDefault();
//            if (rot == null)
//            {
//                return;
//            }
//            var type = rot.GetType("ROT.CampaignBehaviors.ROTTroopRecruiter", false, true);
//            if (type != null)
//            {
//                TraverseTreeMethod = AccessTools.Method(type, "TraverseTree");

//                MethodInfo method = AccessTools.Method(type, nameof(ROTTroopRecruiterPatch.UpgradeTargets)).GetDeclaredMember();
//                harmony.Patch(method, prefix: new HarmonyMethod(typeof(ROTTroopRecruiterPatch), nameof(ROTTroopRecruiterPatch.UpgradeTargets)), finalizer: new HarmonyMethod(typeof(ROTTroopRecruiterPatch), nameof(ROTTroopRecruiterPatch.FixUpgradeTargets)));
//            }
//        }


//        public static bool UpgradeTargets(ref List<CharacterObject> __result, ref Dictionary<CharacterObject, List<CharacterObject>> ____upgradeTargetCache, CharacterObject troop, bool maxTierOnly = false, List<CharacterObject> template = null)
//        {
//            try
//            {
//                if (troop == null || troop.UpgradeTargets == null)
//                {
//                    if (Debugger.IsAttached)
//                    {
//                        Debugger.Break();
//                    }
//                }

//                List<CharacterObject> targets;
//                List<CharacterObject> characterObjects1;
//                Func<CharacterObject, bool> func2 = null;
//                if (!____upgradeTargetCache.ContainsKey(troop))
//                {
//                    targets = (List<CharacterObject>) TraverseTreeMethod.Invoke(null, new object[] { troop });
//                    ____upgradeTargetCache.Add(troop, targets);
//                }
//                else
//                {
//                    targets = ____upgradeTargetCache[troop];
//                }
//                if (maxTierOnly)
//                {
//                    targets = targets.Where<CharacterObject>((CharacterObject c) =>
//                    {
//                        bool flag1;


//                        if (c == null || c.UpgradeTargets == null)
//                        {
//                            if (Debugger.IsAttached)
//                            {
//                                Debugger.Break();
//                            }
//                        }

//                        if (c.UpgradeTargets.Length == 0)
//                        {
//                            flag1 = true;
//                        }
//                        else
//                        {
//                            CharacterObject[] upgradeTargets = c.UpgradeTargets;
//                            Func<CharacterObject, bool> u003cu003e9_2 = func2;
//                            if (u003cu003e9_2 == null)
//                            {
//                                Func<CharacterObject, bool> func = (CharacterObject t) =>
//                                {
//                                    bool flag;
//                                    List<CharacterObject> characterObjects = template;
//                                    flag = (characterObjects != null ? characterObjects.Contains(t) : true);
//                                    return flag;
//                                };
//                                Func<CharacterObject, bool> func1 = func;
//                                func2 = func;
//                                u003cu003e9_2 = func1;
//                            }
//                            flag1 = !((IEnumerable<CharacterObject>) upgradeTargets).Any<CharacterObject>(u003cu003e9_2);
//                        }
//                        return flag1;
//                    }).ToList<CharacterObject>();
//                }
//                characterObjects1 = (template == null ? targets : (
//                    from c in targets
//                    where template.Contains(c)
//                    select c).ToList<CharacterObject>());
//                __result = characterObjects1;

//                return false;
//            }
//            catch (Exception e)
//            {
//                Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return true;
//            }
//        }



//        public static Exception FixUpgradeTargets(ref Exception __exception)
//        {
//            if (__exception != null)
//            {
//                var e = __exception;
//                Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//            }
//            return null;
//        }
//    }
//}

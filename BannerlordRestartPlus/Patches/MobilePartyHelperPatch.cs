//using System;

//using HarmonyLib;

//using Helpers;

//using TaleWorlds.CampaignSystem;
//using TaleWorlds.CampaignSystem.Party;
//using TaleWorlds.CampaignSystem.Roster;
//using TaleWorlds.Library;

//namespace BannerlordRestartPlus.Patches
//{
//    [HarmonyPatch(typeof(MobilePartyHelper))]
//    public static class MobilePartyHelperPatch
//    {
//        [HarmonyPrefix]
//        [HarmonyPatch(nameof(CanTroopGainXp))]

//        public static bool CanTroopGainXp(ref bool __result, PartyBase owner, CharacterObject character, ref int gainableMaxXp) 
//        {
//            try
//            {
//                bool flag = false;
//                gainableMaxXp = 0;
//                int num = owner.MemberRoster.FindIndexOfTroop(character);
//                int elementNumber = owner.MemberRoster.GetElementNumber(num);
//                int elementXp = owner.MemberRoster.GetElementXp(num);
//                for (int i = 0; i < (int) character.UpgradeTargets.Length; i++)
//                {
//                    int upgradeXpCost = character.GetUpgradeXpCost(owner, i);
//                    if (elementXp < upgradeXpCost * elementNumber)
//                    {
//                        flag = true;
//                        int num1 = upgradeXpCost * elementNumber - elementXp;
//                        if (num1 > gainableMaxXp)
//                        {
//                            gainableMaxXp = num1;
//                        }
//                    }
//                }
//                __result = flag;
//                return false;
//            } 
//            catch(Exception e)
//            {
//                Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return true;
//            }
//        }

//        [HarmonyFinalizer]
//        [HarmonyPatch(nameof(CanTroopGainXp))]
//        public static Exception FixCanTroopGainXp(ref Exception __exception)
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

//        [HarmonyPrefix]
//        [HarmonyPatch(nameof(GetMaximumXpAmountPartyCanGet))]

//        public static bool GetMaximumXpAmountPartyCanGet(ref int __result, MobileParty party) 
//        {
//            try
//            {
//                int num;
//                TroopRoster memberRoster = party.MemberRoster;
//                int num1 = 0;
//                for (int i = 0; i < memberRoster.Count; i++)
//                {
//                    TroopRosterElement elementCopyAtIndex = memberRoster.GetElementCopyAtIndex(i);
//                    if (MobilePartyHelper.CanTroopGainXp(party.Party, elementCopyAtIndex.Character, out num))
//                    {
//                        num1 += num;
//                    }
//                }
//                __result = num1;
//                return false;
//            } 
//            catch(Exception e)
//            {
//                Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return true;
//            }
//        }

//        [HarmonyFinalizer]
//        [HarmonyPatch(nameof(GetMaximumXpAmountPartyCanGet))]
//        public static Exception FixGetMaximumXpAmountPartyCanGet(ref Exception __exception)
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
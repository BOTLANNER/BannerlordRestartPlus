//using System;
//using System.Collections.Generic;
//using System.Reflection;

//using HarmonyLib;

//using SandBox.View.Map;

//using TaleWorlds.CampaignSystem;
//using TaleWorlds.CampaignSystem.CampaignBehaviors;
//using TaleWorlds.Core;
//using TaleWorlds.Library;
//using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;

//namespace BannerlordRestartPlus.Patches
//{
//    [HarmonyPatch(typeof(MarriageOfferCampaignBehavior))]
//    public static class MarriageOfferCampaignBehaviorPatch
//    {
//        static MethodInfo _method = AccessTools.Method(typeof(MarriageOfferCampaignBehavior), nameof(DailyTickClan));

//        static MethodInfo CanOfferMarriageForClanMethod = AccessTools.Method(typeof(MarriageOfferCampaignBehavior), "CanOfferMarriageForClan");
//        static bool CanOfferMarriageForClan(this MarriageOfferCampaignBehavior current, Clan c) => (bool) CanOfferMarriageForClanMethod.Invoke(current, new object[] { c });

//        static MethodInfo ConsiderMarriageForPlayerClanMemberMethod = AccessTools.Method(typeof(MarriageOfferCampaignBehavior), "ConsiderMarriageForPlayerClanMember");
//        static bool ConsiderMarriageForPlayerClanMember(this MarriageOfferCampaignBehavior current, Hero h, Clan c) => (bool) ConsiderMarriageForPlayerClanMemberMethod.Invoke(current, new object[] { h, c });


//        [HarmonyPrefix]
//        [HarmonyPatch(nameof(DailyTickClan))]

//        public static bool DailyTickClan(ref MarriageOfferCampaignBehavior __instance, Clan consideringClan)
//        {
//            try
//            {
//                if (__instance.CanOfferMarriageForClan(consideringClan))
//                {
//                    float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(Clan.PlayerClan.FactionMidSettlement, consideringClan.FactionMidSettlement);
//                    if (MBRandom.RandomFloat >= distance / Campaign.MaximumDistanceBetweenTwoSettlements - 0.5f)
//                    {
//                        foreach (Hero hero in Clan.PlayerClan.Heroes)
//                        {
//                            if (hero == Hero.MainHero || !hero.CanMarry() || !__instance.ConsiderMarriageForPlayerClanMember(hero, consideringClan))
//                            {
//                                continue;
//                            }
//                            return false;
//                        }
//                    }
//                }
//                return false;
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

//        [HarmonyFinalizer]
//        [HarmonyPatch(nameof(DailyTickClan))]
//        public static Exception FixDailyTickClan(ref Exception __exception)
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
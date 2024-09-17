//using System;
//using System.Collections.Generic;
//using System.Linq;

//using HarmonyLib;

//using Helpers;

//using TaleWorlds.CampaignSystem;
//using TaleWorlds.Core;
//using TaleWorlds.Library;
//using TaleWorlds.Localization;

//namespace BannerlordRestartPlus.Patches
//{
//    [HarmonyPatch(typeof(FactionHelper))]
//    public static class FactionHelperPatch
//    {
//        [HarmonyPostfix]
//        [HarmonyPatch(nameof(IsClanNameApplicable))]

//        public static void IsClanNameApplicable(ref Tuple<bool, string> __result, string name)
//        {
//            string empty = String.Empty;
//            List<TextObject> textObjects = new List<TextObject>();
//            IsFactionNameApplicable(ref textObjects, name);
//            if (Clan.All.Any<Clan>((Clan x) => {
//                if (x == Clan.PlayerClan)
//                {
//                    return false;
//                }
//                return String.Equals(x.Name.ToString(), name, StringComparison.InvariantCultureIgnoreCase);
//            }))
//            {
//                textObjects.Add(GameTexts.FindText("str_clan_name_invalid_already_exist", null));
//            }
//            bool count = textObjects.Count == 0;
//            if (textObjects.Count == 1)
//            {
//                empty = textObjects[0].ToString();
//            }
//            else if (textObjects.Count > 1)
//            {
//                TextObject item = textObjects[0];
//                for (int i = 1; i < textObjects.Count; i++)
//                {
//                    item = GameTexts.FindText("str_string_newline_newline_string", null).SetTextVariable("STR1", item.ToString()).SetTextVariable("STR2", textObjects[i].ToString());
//                }
//                empty = item.ToString();
//            }
//            __result = new Tuple<bool, string>(count, empty);
//        }

//        [HarmonyPrefix]
//        [HarmonyPatch(nameof(IsFactionNameApplicable))]

//        public static bool IsFactionNameApplicablePre(ref List<TextObject> __result, string name)
//        {
//            List<TextObject> textObjects = new List<TextObject>();
//            if ((name.Length > 50 ? true : name.Length < 1))
//            {
//                TextObject textObject = GameTexts.FindText("str_faction_name_invalid_character_count", null).SetTextVariable("MIN", 1).SetTextVariable("MAX", 50);
//                textObjects.Add(textObject);
//            }
//            if (Common.TextContainsSpecialCharacters(name))
//            {
//                textObjects.Add(GameTexts.FindText("str_faction_name_invalid_characters", null));
//            }
//            if (name.StartsWith(" ") || name.EndsWith(" "))
//            {
//                textObjects.Add(new TextObject("{=LCOZZMta}Faction name cannot start or end with a white space", null));
//            }
//            if (name.Contains("  "))
//            {
//                textObjects.Add(new TextObject("{=CtsdrQ9N}Faction name cannot contain consecutive white spaces", null));
//            }
//            __result = textObjects;
//            return false;
//        }

//        [HarmonyPostfix]
//        [HarmonyPatch(nameof(IsFactionNameApplicable))]

//        public static void IsFactionNameApplicable(ref List<TextObject> __result, string name)
//        {
//            List<TextObject> textObjects = __result;
//            //textObjects = new List<TextObject>();
//            //if ((name.Length > 50 ? true : name.Length < 1))
//            //{
//            //    TextObject textObject = GameTexts.FindText("str_faction_name_invalid_character_count", null).SetTextVariable("MIN", 1).SetTextVariable("MAX", 50);
//            //    textObjects.Add(textObject);
//            //}
//            //if (Common.TextContainsSpecialCharacters(name))
//            //{
//            //    textObjects.Add(GameTexts.FindText("str_faction_name_invalid_characters", null));
//            //}
//            //if (name.StartsWith(" ") || name.EndsWith(" "))
//            //{
//            //    textObjects.Add(new TextObject("{=LCOZZMta}Faction name cannot start or end with a white space", null));
//            //}
//            //if (name.Contains("  "))
//            //{
//            //    textObjects.Add(new TextObject("{=CtsdrQ9N}Faction name cannot contain consecutive white spaces", null));
//            //}
//            __result = textObjects;
//        }
//    }
//}
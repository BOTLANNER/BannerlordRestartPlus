using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HarmonyLib;

using Helpers;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerlordRestartPlus.Patches
{
    [HarmonyPatch(typeof(HeroCreator))]
    public static class HeroCreatorPatch
    {
        static MethodInfo _createNewHeroMethod = AccessTools.Method(typeof(HeroCreator), nameof(CreateNewHero));

        private static Hero InvokeCreateNewHero(CharacterObject template, int age = -1)
        {
            return (Hero) _createNewHeroMethod.Invoke(null, new object[] { template, age });
        }

        static MethodInfo _staticBodyPropertiesPropSetter = AccessTools.Property(typeof(Hero), "StaticBodyProperties").GetSetMethod(true);
        private static void SetStaticBodyProperties(this Hero hero, StaticBodyProperties staticBodyProperties)
        {
            _staticBodyPropertiesPropSetter.Invoke(hero, new object[] { staticBodyProperties });
        }


        static MethodInfo _decideBornSettlementMethod = AccessTools.Method(typeof(HeroCreator), nameof(DecideBornSettlement));
        private static Settlement DecideBornSettlement(Hero child)
        {
            return (Settlement) _decideBornSettlementMethod.Invoke(null, new object[] { child });
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(DeliverOffSpring))]

        public static bool DeliverOffSpring(ref Hero __result, Hero mother, Hero father, bool isOffspringFemale)
        {
            try
            {
                if (father.StringId.Contains("player") || mother.StringId.Contains("player") || mother.StringId.Contains(HeroPatch.StringIdWatch) || father.StringId.Contains(HeroPatch.StringIdWatch))
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        Console.WriteLine("Debugger");
                    }
                }

                //TextObject textObject;
                //TextObject textObject1;
                //int num;
                //Debug.SilentAssert(mother.CharacterObject.Race == father.CharacterObject.Race, "", false, "BannerlordRestartPlus\\Patches\\HeroCreatorPatch.cs", "DeliverOffSpring", 40);
                //Hero hero = InvokeCreateNewHero((isOffspringFemale ? mother.CharacterObject : father.CharacterObject), 0);
                //hero.ClearTraits();
                //float randomFloat = MBRandom.RandomFloat;
                //int num1 = 0;
                //if ((double) randomFloat < 0.1)
                //{
                //    num1 = 0;
                //}
                //else if ((double) randomFloat >= 0.5)
                //{
                //    num1 = ((double) randomFloat >= 0.9 ? 3 : 2);
                //}
                //else
                //{
                //    num1 = 1;
                //}
                //List<TraitObject> list = DefaultTraits.Personality.ToList<TraitObject>();
                //list.Shuffle<TraitObject>();
                //for (int i = 0; i < Math.Min(list.Count, num1); i++)
                //{
                //    num = ((double) MBRandom.RandomFloat < 0.5 ? MBRandom.RandomInt(list[i].MinValue, 0) : MBRandom.RandomInt(1, list[i].MaxValue + 1));
                //    hero.SetTraitLevel(list[i], num);
                //}
                //foreach (TraitObject traitObject in TraitObject.All.Except<TraitObject>(DefaultTraits.Personality))
                //{
                //    hero.SetTraitLevel(traitObject, ((double) MBRandom.RandomFloat < 0.5 ? mother.GetTraitLevel(traitObject) : father.GetTraitLevel(traitObject)));
                //}
                //hero.SetNewOccupation((isOffspringFemale ? mother.Occupation : father.Occupation));
                //int becomeChildAge = Campaign.Current.Models.AgeModel.BecomeChildAge;
                //hero.CharacterObject.IsFemale = isOffspringFemale;
                //hero.Mother = mother;
                //hero.Father = father;
                //MBEquipmentRoster randomElementInefficiently = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentRostersForDeliveredOffspring(hero).GetRandomElementInefficiently<MBEquipmentRoster>();
                //if (randomElementInefficiently == null)
                //{
                //    Debug.FailedAssert("Equipment template not found", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\HeroCreator.cs", "DeliverOffSpring", 549);
                //}
                //else
                //{
                //    Equipment equipment = randomElementInefficiently.GetCivilianEquipments().GetRandomElementInefficiently<Equipment>();
                //    EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipment);
                //    Equipment equipment1 = new Equipment(false);
                //    equipment1.FillFrom(equipment, false);
                //    EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipment1);
                //}
                //NameGenerator.Current.GenerateHeroNameAndHeroFullName(hero, out textObject, out textObject1, false);
                //hero.SetName(textObject1, textObject);
                //hero.HeroDeveloper.InitializeHeroDeveloper(true, null);
                //BodyProperties bodyProperties = mother.BodyProperties;
                //BodyProperties bodyProperty = father.BodyProperties;
                //int num2 = MBRandom.RandomInt();
                //string str = (isOffspringFemale ? mother.HairTags : father.HairTags);
                //string str1 = (isOffspringFemale ? mother.TattooTags : father.TattooTags);
                //BodyProperties randomBodyProperties = BodyProperties.GetRandomBodyProperties(mother.CharacterObject.Race, isOffspringFemale, bodyProperties, bodyProperty, 1, num2, str, father.BeardTags, str1);
                //hero.SetStaticBodyProperties( randomBodyProperties.StaticProperties);
                //hero.BornSettlement = DecideBornSettlement(hero);
                //if (mother == Hero.MainHero || father == Hero.MainHero)
                //{
                //    hero.Clan = Clan.PlayerClan;
                //    hero.Culture = Hero.MainHero.Culture;
                //}
                //else
                //{
                //    hero.Clan = father.Clan;
                //    hero.Culture = ((double) MBRandom.RandomFloat < 0.5 ? father.Culture : mother.Culture);
                //}
                //CampaignEventDispatcher.Instance.OnHeroCreated(hero, true);
                //int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
                //if (hero.Age > (float) becomeChildAge || hero.Age == (float) becomeChildAge && hero.BirthDay.GetDayOfYear < CampaignTime.Now.GetDayOfYear)
                //{
                //    CampaignEventDispatcher.Instance.OnHeroGrowsOutOfInfancy(hero);
                //}
                //if (hero.Age > (float) heroComesOfAge || hero.Age == (float) heroComesOfAge && hero.BirthDay.GetDayOfYear < CampaignTime.Now.GetDayOfYear)
                //{
                //    CampaignEventDispatcher.Instance.OnHeroComesOfAge(hero);
                //}
                //__result = hero;
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
        //[HarmonyPatch(nameof(DeliverOffSpring))]
        //public static Exception? FixDeliverOffSpring(ref Exception? __exception)
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

        public static FieldInfo DefaultCharacterSkills = AccessTools.Field(typeof(BasicCharacterObject), "DefaultCharacterSkills");

        [HarmonyPrefix]
        [HarmonyPatch(nameof(CreateNewHero))]

        public static bool CreateNewHero(ref Hero __result, CharacterObject template, int age = -1)
        {
            try
            {
                if (DefaultCharacterSkills.GetValue(template) == null)
                {
                    DefaultCharacterSkills.SetValue(template, DefaultCharacterSkills.GetValue(Hero.MainHero.CharacterObject));
                }
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
        //[HarmonyPatch(nameof(CreateNewHero))]
        //public static Exception? FixCreateNewHero(ref Exception? __exception)
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
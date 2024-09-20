
using System;

using BannerlordRestartPlus.Behaviours;
using BannerlordRestartPlus.Patches;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace BannerlordRestartPlus.Saves
{
    public class PlayerCharacter
    {
        static Color Error = new(178 * 255, 34 * 255, 34 * 255);

        [SaveableProperty(1)]
        public Hero? Hero { get; set; }

        [SaveableProperty(2)]
        public CharacterObject? CharacterObject { get; set; }

        [SaveableProperty(3)]
        public BasicCharacterObject? BasicCharacterObject { get; set; }

        [SaveableProperty(4)]
        public CultureObject? Culture { get; set; }

        [SaveableProperty(5)]
        public CharacterObject[]? UpgradeTargets { get; set; }

        public PlayerCharacter()
        {
        }

        public PlayerCharacter(Hero hero)
        {
            Hero = hero;
            PopulateValues(hero.CharacterObject);
        }

        public PlayerCharacter(CharacterObject character)
        {
            PopulateValues(character);
        }

        public void PopulateValues(CharacterObject character)
        {
            if (character.HeroObject != null)
            {
                Hero = character.HeroObject;
            }
            CharacterObject = character;
            BasicCharacterObject = character as BasicCharacterObject;
            //DefaultCharacterSkills = HeroCreatorPatch.DefaultCharacterSkills.GetValue(character) as MBCharacterSkills;
            //if (DefaultCharacterSkills == null)
            //{
            //    DefaultCharacterSkills = MBObjectManager.Instance.CreateObject<MBCharacterSkills>((character as BasicCharacterObject).StringId);
            //    HeroCreatorPatch.DefaultCharacterSkills.SetValue(character, DefaultCharacterSkills);
            //}
            Culture = character.Culture;
            //BasicCulture = (character as BasicCharacterObject).Culture;
            UpgradeTargets = character.UpgradeTargets;
            if (UpgradeTargets == null)
            {
                UpgradeTargets = new CharacterObject[0];
                RestartPlusBehaviour.SetUpgradeTargets(character, UpgradeTargets);
            }
        }

        public void OnLoad()
        {
            try
            {
                Hero hero = this.Hero!;
                if (hero!.CharacterObject == null)
                {
                   HeroPatch.CharacterObjectFieldOnHero.SetValue(hero, this.CharacterObject);
                }

                var c = hero.CharacterObject;
                if (!c!.IsRegistered())
                {
                    MBObjectManager.Instance.RegisterObject(c);
                }


                //if (!CharacterObject.All.Contains(c))
                //{
                //    CharacterObject.All.Add(c);
                //}

                if (c!.UpgradeTargets == null)
                {
                    RestartPlusBehaviour.SetUpgradeTargets(c, this.UpgradeTargets ?? new CharacterObject[0]);
                }

                if (HeroCreatorPatch.DefaultCharacterSkills.GetValue(c) == null)
                {
                    HeroCreatorPatch.DefaultCharacterSkills.SetValue(hero.CharacterObject, HeroCreatorPatch.DefaultCharacterSkills.GetValue(Hero.MainHero.CharacterObject));
                }

                if (c.Culture == null)
                {
                    c.Culture = this.Culture;
                }

                if ((c as BasicCharacterObject).Culture == null)
                {
                    (c as BasicCharacterObject).Culture = c.Culture ?? this.Culture;
                }

                if (!c.IsReady)
                {
                    Debug.Print(String.Concat("Previous Player: Unready object reference found with ID: ", c.StringId), 0, Debug.DebugColor.White, 17592186044416L);
                    c.IsReady = true;
                }
            }
            catch (Exception e)
            {
                Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
                InformationManager.DisplayMessage(new InformationMessage(e.ToString(), Error));
            }
        }
    }
}

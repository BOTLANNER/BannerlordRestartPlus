using System;
using System.Linq;
using System.Reflection;

using BannerlordRestartPlus.Patches;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BannerlordRestartPlus
{
    public class RestartPlusBehaviour : CampaignBehaviorBase
    {
        public static bool SuggestGameRestart = false;

        static MethodInfo? UpgradeTargetsSetMethod;
        static void SetUpgradeTargets(CharacterObject current, CharacterObject[] value) => UpgradeTargetsSetMethod?.Invoke(current, new object[] { value });

        static Color Error = new(178 * 255, 34 * 255, 34 * 255);
        static Color Warn = new(189 * 255, 38 * 255, 0);

        static RestartPlusBehaviour()
        {
            try
            {
                UpgradeTargetsSetMethod = AccessTools.Property(typeof(CharacterObject), "UpgradeTargets").GetSetMethod(true);
            }
            catch (Exception e)
            {
                Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
                InformationManager.DisplayMessage(new InformationMessage(e.ToString(), Warn));
            }
        }

        #region Overrides
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, this.DailyTick);
            CampaignEvents.OnGameEarlyLoadedEvent.AddNonSerializedListener(this, this.EarlyLoad);
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, this.Loaded);
            CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, this.LoadFinished);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }
        #endregion

        #region Event Handlers

        private void EarlyLoad(CampaignGameStarter obj)
        {
            FixBrokenHeroes();
        }

        private void Loaded(CampaignGameStarter obj)
        {
            CheckRestart();
            FixBrokenHeroes();
        }

        private void OnSessionLaunched(CampaignGameStarter starter)
        {
            CheckRestart();
            FixBrokenHeroes();
        }

        private void LoadFinished()
        {
            CheckRestart();
            FixBrokenHeroes();
        }

        private void DailyTick()
        {
            FixBrokenHeroes();
        }

        public static void CheckRestart(Action? beforeRestart = null, Action? onCancel = null)
        {
            if (SuggestGameRestart == true)
            {
                SuggestGameRestart = false;
                var restartPlusTitle = new TextObject("{=restart_plus_n_01}Restart+");
                var confirm = new TextObject("{=restart_plus_03}The game needs to be restarted to avoid potential crashes. Do you want to close the game now?");

                InformationManager.ShowInquiry(new InquiryData(restartPlusTitle.ToString(), confirm.ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(),
                    () =>
                    {
                        try
                        {
                            InformationManager.HideInquiry();
                            beforeRestart?.Invoke();
                            Utilities.QuitGame();
                        }
                        catch (Exception e)
                        {
                            Debug.WriteDebugLineOnScreen(e.ToString());
                        }
                    },
                    () =>
                    {
                        // Cancelled. Do nothing.
                        onCancel?.Invoke();
                    }), true, true);
            }
        }

        private void FixBrokenHeroes()
        {
            try
            {
                foreach (var hero in Hero.FindAll(hero => hero.CharacterObject != null && hero.CharacterObject.UpgradeTargets == null))
                {
                    try
                    {
                        if (!hero.CharacterObject.IsRegistered())
                        {
                            MBObjectManager.Instance.RegisterObject(hero.CharacterObject);
                        }

                        SetUpgradeTargets(hero.CharacterObject, new CharacterObject[0]);
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

                foreach (var p in MobileParty.All.Where(p => p.LeaderHero != null && p.LeaderHero.CharacterObject != null && (p.LeaderHero.CharacterObject.UpgradeTargets == null || !CharacterObject.All.Contains(p.LeaderHero.CharacterObject))))
                {
                    try
                    {
                        if (!p.LeaderHero.CharacterObject.IsRegistered())
                        {
                            MBObjectManager.Instance.RegisterObject(p.LeaderHero.CharacterObject);
                        }

                        if (!CharacterObject.All.Contains(p.LeaderHero.CharacterObject))
                        {
                            CharacterObject.All.Add(p.LeaderHero.CharacterObject);
                        }

                        if (p.LeaderHero.CharacterObject.UpgradeTargets == null)
                        {
                            SetUpgradeTargets(p.LeaderHero.CharacterObject, new CharacterObject[0]);
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

                foreach (var c in CharacterObject.All.Where(c => c != null && c.UpgradeTargets == null))
                {
                    try
                    {
                        if (!c.IsRegistered())
                        {
                            MBObjectManager.Instance.RegisterObject(c);
                        }

                        SetUpgradeTargets(c, new CharacterObject[0]);
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

                var defaultSkills = HeroCreatorPatch.DefaultCharacterSkills.GetValue(Hero.MainHero.CharacterObject);
                
                foreach (var hero in Hero.FindAll(hero => hero.CharacterObject != null && HeroCreatorPatch.DefaultCharacterSkills.GetValue(hero.CharacterObject) == null))
                {
                    try
                    {

                        if (!hero.CharacterObject.IsRegistered())
                        {
                            MBObjectManager.Instance.RegisterObject(hero.CharacterObject);
                        }
                        HeroCreatorPatch.DefaultCharacterSkills.SetValue(hero.CharacterObject, defaultSkills);
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

                foreach (var p in MobileParty.All.Where(p => p.LeaderHero != null && p.LeaderHero.CharacterObject != null && HeroCreatorPatch.DefaultCharacterSkills.GetValue(p.LeaderHero.CharacterObject) == null))
                {
                    try
                    {

                        if (!p.LeaderHero.CharacterObject.IsRegistered())
                        {
                            MBObjectManager.Instance.RegisterObject(p.LeaderHero.CharacterObject);
                        }

                        HeroCreatorPatch.DefaultCharacterSkills.SetValue(p.LeaderHero.CharacterObject, defaultSkills);
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

                foreach (var c in CharacterObject.All.Where(c => c != null && HeroCreatorPatch.DefaultCharacterSkills.GetValue(c) == null))
                {
                    try
                    {
                        if (!c.IsRegistered())
                        {
                            MBObjectManager.Instance.RegisterObject(c);
                        }
                        HeroCreatorPatch.DefaultCharacterSkills.SetValue(c, defaultSkills);
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
                
                foreach (var hero in Hero.FindAll(hero => hero.CharacterObject != null && hero.CharacterObject.Culture != null && (hero.CharacterObject as BasicCharacterObject).Culture == null))
                {
                    try
                    {

                        if (!hero.CharacterObject.IsRegistered())
                        {
                            MBObjectManager.Instance.RegisterObject(hero.CharacterObject);
                        }
                        (hero.CharacterObject as BasicCharacterObject).Culture = hero.CharacterObject.Culture;
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

                foreach (var p in MobileParty.All.Where(p => p.LeaderHero != null && p.LeaderHero.CharacterObject != null && p.LeaderHero.CharacterObject.Culture != null && (p.LeaderHero.CharacterObject as BasicCharacterObject).Culture == null))
                {
                    try
                    {

                        if (!p.LeaderHero.CharacterObject.IsRegistered())
                        {
                            MBObjectManager.Instance.RegisterObject(p.LeaderHero.CharacterObject);
                        }

                        (p.LeaderHero.CharacterObject as BasicCharacterObject).Culture = p.LeaderHero.CharacterObject.Culture;
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

                foreach (var c in CharacterObject.All.Where(c => c != null && c.Culture != null && (c as BasicCharacterObject).Culture == null))
                {
                    try
                    {
                        if (!c.IsRegistered())
                        {
                            MBObjectManager.Instance.RegisterObject(c);
                        }
                        (c as BasicCharacterObject).Culture = c.Culture;
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
            catch (Exception e)
            {
                Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
                InformationManager.DisplayMessage(new InformationMessage(e.ToString(), Error));
            }
        }
        #endregion
    }

    public static class RestartPlusExtensions
    {
        static PropertyInfo IsRegisteredProp = AccessTools.Property(typeof(MBObjectBase), nameof(IsRegistered));
        public static bool IsRegistered(this MBObjectBase mBObjectBase)
        {
            return (bool) IsRegisteredProp.GetValue(mBObjectBase);
        }
    }
}

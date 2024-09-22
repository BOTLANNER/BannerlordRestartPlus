using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using BannerlordRestartPlus.Patches.Optional;
using BannerlordRestartPlus.Patches.Runtime;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

using System.Linq;

using Debug = TaleWorlds.Library.Debug;
using System.Reflection;
using System;
using Bannerlord.UIExtenderEx;
using BannerlordRestartPlus.Behaviours;

namespace BannerlordRestartPlus
{
    public class Main : MBSubModuleBase
    {
        /* Semantic Versioning (https://semver.org): */
        public static readonly int SemVerMajor = 0;
        public static readonly int SemVerMinor = 3;
        public static readonly int SemVerPatch = 0;
        public static readonly string? SemVerSpecial = null;
        private static readonly string SemVerEnd = (SemVerSpecial is not null) ? "-" + SemVerSpecial : string.Empty;
        public static readonly string Version = $"{SemVerMajor}.{SemVerMinor}.{SemVerPatch}{SemVerEnd}";

        public static readonly string Name = typeof(Main).Namespace;
        public static readonly string DisplayName = "Restart+"; // to be shown to humans in-game
        public static readonly string HarmonyDomain = "com.b0tlanner.bannerlord." + Name.ToLower();

        internal static readonly Color ImportantTextColor = Color.FromUint(0x00F16D26); // orange

        internal static Settings? Settings;

        private bool _loaded;
        public static Harmony Harmony;
        private UIExtender? _extender;


        private static readonly IOptionalPatch[] HarmonyOptionalPatches;
        private static List<IRuntimePatch> HarmonyRuntimePatches = LoadRuntimePatches().ToList();

        public Main()
        {
            //Ctor
        }

        static Main()
        {
            try
            {
                HarmonyOptionalPatches = new IOptionalPatch[] { };
            }
            catch (System.Exception e)
            {
                Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
            }
        }


        protected override void OnSubModuleLoad()
        {
            try
            {
                base.OnSubModuleLoad();
                Harmony = new Harmony(HarmonyDomain);

                foreach (var patch in HarmonyOptionalPatches)
                {
                    patch.TryPatch(Harmony);
                }

                foreach (var patch in HarmonyRuntimePatches)
                {
                    patch.PatchSubmoduleLoad(Harmony);
                }


                Harmony.PatchAll();

                _extender = UIExtender.Create(HarmonyDomain);
                _extender.Register(typeof(Main).Assembly);
                _extender.Enable();
            }
            catch (System.Exception e)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
                Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            try
            {

                if (Settings.Instance is not null && Settings.Instance != Settings)
                {
                    Settings = Settings.Instance;

                    // register for settings property-changed events
                    Settings.PropertyChanged += Settings_OnPropertyChanged;
                }

                if (!_loaded)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"Loaded {DisplayName}", ImportantTextColor));
                    _loaded = true;


                    foreach (var patch in HarmonyOptionalPatches)
                    {
                        patch.MenusInitialised(Harmony);
                    }

                    foreach (var patch in HarmonyRuntimePatches)
                    {
                        patch.PatchAfterMenus(Harmony);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.PrintError(e.Message, e.StackTrace);
                Debug.WriteDebugLineOnScreen(e.ToString());
                Debug.SetCrashReportCustomString(e.Message);
                Debug.SetCrashReportCustomStack(e.StackTrace);
            }
        }

        protected override void OnGameStart(Game game, IGameStarter starterObject)
        {
            try
            {
                base.OnGameStart(game, starterObject);

                if (game.GameType is Campaign)
                {
                    var initializer = (CampaignGameStarter) starterObject;
                    AddBehaviors(initializer);
                }
            }
            catch (System.Exception e) { Debug.PrintError(e.Message, e.StackTrace); Debug.WriteDebugLineOnScreen(e.ToString()); Debug.SetCrashReportCustomString(e.Message); Debug.SetCrashReportCustomStack(e.StackTrace); }
        }

        private void AddBehaviors(CampaignGameStarter gameInitializer)
        {
            try
            {
                gameInitializer.AddBehavior(new RestartPlusBehaviour());
                gameInitializer.AddBehavior(new PlayAsBehaviour());
                gameInitializer.AddBehavior(new ExtendedRetirementBehaviour());
            }
            catch (System.Exception e) { Debug.PrintError(e.Message, e.StackTrace); Debug.WriteDebugLineOnScreen(e.ToString()); Debug.SetCrashReportCustomString(e.Message); Debug.SetCrashReportCustomStack(e.StackTrace); }
        }

        protected static void Settings_OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            try
            {
                if (sender is Settings settings && args.PropertyName == MCM.Abstractions.Base.BaseSettings.SaveTriggered)
                {
                    var extendedRetirementBehaviour = ExtendedRetirementBehaviour.Instance;
                    var campaignGameStarter = Campaign.Current != null && Game.Current != null ?  SandBoxManager.Instance?.GameStarter : null;
                    if (extendedRetirementBehaviour != null && campaignGameStarter != null)
                    {
                        extendedRetirementBehaviour.SetupGameMenus(campaignGameStarter);
                    }
                }
            }
            catch (System.Exception e) { Debug.PrintError(e.Message, e.StackTrace); Debug.WriteDebugLineOnScreen(e.ToString()); Debug.SetCrashReportCustomString(e.Message); Debug.SetCrashReportCustomStack(e.StackTrace); }
        }

        static IEnumerable<IRuntimePatch> LoadRuntimePatches()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IRuntimePatch).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        var inst = type.CreateInstance();
                        if (inst is IRuntimePatch runtimePatch)
                        {
                            yield return runtimePatch;
                        }

                    }
                }
            }
        }
    }
}

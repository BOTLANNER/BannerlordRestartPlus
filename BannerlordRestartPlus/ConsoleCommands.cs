using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

using static TaleWorlds.Library.CommandLineFunctionality;
using TaleWorlds.Core;
using StoryMode;
using BannerlordRestartPlus.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Actions;

namespace BannerlordRestartPlus
{
    class ConsoleCommands
    {

        [CommandLineArgumentFunction("help", "restart_plus")]
        public static string Help(List<string> args)
        {
            string helpResponse = @"
Commands:

restart_plus.help                              - Shows console command usage
restart_plus.restart                           - Creates a new main character in the current game world based on MCM config
restart_plus.kill_player                       - Kills the main character and if enabled in MCM config will trigger the Restart+ options.
restart_plus.retire                            - Causes the main character to retire from the game and if enabled in MCM config will trigger the Restart+ options.
restart_plus.list_kingdoms                     - List all supported kingdom names for use with restart_plus.play_as_ruler
restart_plus.play_as_ruler      kingdomName    - Play as the current ruler for the specified kingdom name (Call restart_plus.list_kingdoms for all supported kingdom names)
restart_plus.list_clans                        - List all supported clan names for use with restart_plus.play_as_leader
restart_plus.play_as_leader     clanName       - Play as the current leader for the specified clan name (Call restart_plus.list_clans for all supported clan names)


";

            return helpResponse;
        }

        [CommandLineArgumentFunction("kill_player", "restart_plus")]
        public static string KillPlayer(List<string> args)
        {
            try
            {
                if (Main.Settings != null && Main.Settings.Enabled)
                {
                    TextObject disableReason = TextObject.Empty;

                    if (Game.Current == null || Campaign.Current == null || Hero.MainHero == null)
                    {
                        disableReason = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                        return disableReason.ToString();
                    }

                    try
                    {
                        if (Game.Current?.GameType is CampaignStoryMode && !Main.Settings.AllowCampaign)
                        {
                            disableReason = new TextObject("{=restart_plus_h_02}RestartPlus: Story Campaign is not supported! Only Sandbox or modded game types are supported.");
                            return disableReason.ToString();
                        }
                        else if (Game.Current?.GameType is CampaignStoryMode && !StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted)
                        {
                            disableReason = new TextObject("{=restart_plus_h_03}RestartPlus: Story needs to have reached a later phase in the campaign.");
                            return disableReason.ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.PrintError(e.Message, e.StackTrace);
                        Debug.WriteDebugLineOnScreen(e.ToString());
                        Debug.SetCrashReportCustomString(e.Message);
                        Debug.SetCrashReportCustomStack(e.StackTrace);

                        disableReason = new TextObject(e.Message);
                        return disableReason.ToString();
                    }

                    if (Hero.MainHero.CurrentSettlement != null || MobileParty.MainParty.MapEvent != null)
                    {
                        disableReason = new TextObject("{=restart_plus_n_05}RestartPlus: Not available. Current main character is busy (either in a settlement, in an encounter, or is a prisoner).");
                        return disableReason.ToString();
                    }

                    try
                    {
                        KillCharacterAction.ApplyByDeathMark(Hero.MainHero, true);

                        return new TextObject("{=restart_plus_n_20}RestartPlus: Finalising player death.").ToString();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteDebugLineOnScreen(e.ToString());
                        return e.ToString();
                    }
                }
                else
                {
                    return new TextObject("{=restart_plus_n_08}Restart+ not enabled!").ToString();
                }
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }
        }

        [CommandLineArgumentFunction("retire", "restart_plus")]
        public static string Retire(List<string> args)
        {
            try
            {
                if (Main.Settings != null && Main.Settings.Enabled)
                {
                    TextObject disableReason = TextObject.Empty;

                    if (Game.Current == null || Campaign.Current == null || Hero.MainHero == null)
                    {
                        disableReason = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                        return disableReason.ToString();
                    }

                    try
                    {
                        if (Game.Current?.GameType is CampaignStoryMode && !Main.Settings.AllowCampaign)
                        {
                            disableReason = new TextObject("{=restart_plus_h_02}RestartPlus: Story Campaign is not supported! Only Sandbox or modded game types are supported.");
                            return disableReason.ToString();
                        }
                        else if (Game.Current?.GameType is CampaignStoryMode && !StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted)
                        {
                            disableReason = new TextObject("{=restart_plus_h_03}RestartPlus: Story needs to have reached a later phase in the campaign.");
                            return disableReason.ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.PrintError(e.Message, e.StackTrace);
                        Debug.WriteDebugLineOnScreen(e.ToString());
                        Debug.SetCrashReportCustomString(e.Message);
                        Debug.SetCrashReportCustomStack(e.StackTrace);

                        disableReason = new TextObject(e.Message);
                        return disableReason.ToString();
                    }

                    if (Hero.MainHero.CurrentSettlement != null || MobileParty.MainParty.MapEvent != null)
                    {
                        disableReason = new TextObject("{=restart_plus_n_05}RestartPlus: Not available. Current main character is busy (either in a settlement, in an encounter, or is a prisoner).");
                        return disableReason.ToString();
                    }

                    try
                    {
                        var retirementSettlement = Settlement.Find("retirement_retreat");
                        if (retirementSettlement != null)
                        {
                            retirementSettlement.IsVisible = true;
                            RetirementSettlementComponent? settlementComponent = retirementSettlement.SettlementComponent as RetirementSettlementComponent;
                            if (!(settlementComponent?.IsSpotted ?? true))
                            {
                                settlementComponent.IsSpotted = true;
                            }
                            EncounterManager.StartSettlementEncounter(MobileParty.MainParty, retirementSettlement);
                        }

                        return new TextObject("{=restart_plus_n_21}RestartPlus: Finalising player retirement.").ToString();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteDebugLineOnScreen(e.ToString());
                        return e.ToString();
                    }
                }
                else
                {
                    return new TextObject("{=restart_plus_n_08}Restart+ not enabled!").ToString();
                }
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }
        }

        [CommandLineArgumentFunction("restart", "restart_plus")]
        public static string Restart(List<string> args)
        {
            try
            {
                if (Main.Settings != null && Main.Settings.Enabled)
                {
                    TextObject disableReason = TextObject.Empty;

                    if (Game.Current == null || Campaign.Current == null || Hero.MainHero == null)
                    {
                        disableReason = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                        return disableReason.ToString();
                    }

                    try
                    {
                        if (Game.Current?.GameType is CampaignStoryMode && !Main.Settings.AllowCampaign)
                        {
                            disableReason = new TextObject("{=restart_plus_h_02}RestartPlus: Story Campaign is not supported! Only Sandbox or modded game types are supported.");
                            return disableReason.ToString();
                        }
                        else if (Game.Current?.GameType is CampaignStoryMode && !StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted)
                        {
                            disableReason = new TextObject("{=restart_plus_h_03}RestartPlus: Story needs to have reached a later phase in the campaign.");
                            return disableReason.ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.PrintError(e.Message, e.StackTrace);
                        Debug.WriteDebugLineOnScreen(e.ToString());
                        Debug.SetCrashReportCustomString(e.Message);
                        Debug.SetCrashReportCustomStack(e.StackTrace);

                        disableReason = new TextObject(e.Message);
                        return disableReason.ToString();
                    }

                    if (Hero.MainHero.CurrentSettlement != null || MobileParty.MainParty.MapEvent != null)
                    {
                        disableReason = new TextObject("{=restart_plus_n_05}RestartPlus: Not available. Current main character is busy (either in a settlement, in an encounter, or is a prisoner).");
                        return disableReason.ToString();
                    }

                    try
                    {
                        InformationManager.HideInquiry();
                        RestartPlusAction.Apply();

                        return new TextObject("{=restart_plus_n_06}Finalising new character").ToString();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteDebugLineOnScreen(e.ToString());
                        return e.ToString();
                    }
                }
                else
                {
                    return new TextObject("{=restart_plus_n_08}Restart+ not enabled!").ToString();
                }
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }
        }

        [CommandLineArgumentFunction("list_kingdoms", "restart_plus")]
        public static string ListKingdoms(List<string> args)
        {
            try
            {
                if (Campaign.Current == null || Campaign.Current.Kingdoms == null || Campaign.Current.Kingdoms.Count <= 0)
                {
                    var disableReason = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                    return disableReason.ToString();
                }


                string result = "";
                foreach (var kingdom in Campaign.Current.Kingdoms)
                {
                    if (kingdom.IsEliminated || kingdom.Leader == null)
                    {
                        continue;
                    }
                    result += "\r\n";
                    result += kingdom.InformalName.ToString() + " or " + kingdom.Name.ToString() + " or " + kingdom.GetName().ToString();
                }
                return result;
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }
        }

        [CommandLineArgumentFunction("list_clans", "restart_plus")]
        public static string ListClans(List<string> args)
        {
            try
            {
                if (Campaign.Current == null || Campaign.Current.Clans == null || Campaign.Current.Clans.Count <= 0)
                {
                    var disableReason = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                    return disableReason.ToString();
                }


                string result = "";
                foreach (var clan in Campaign.Current.Clans)
                {
                    if (clan.IsEliminated || clan.Leader == null)
                    {
                        continue;
                    }
                    result += "\r\n";
                    result += clan.InformalName.ToString() + " or " + clan.Name.ToString() + " or " + clan.GetName().ToString();
                }
                return result;
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }
        }

        [CommandLineArgumentFunction("play_as_ruler", "restart_plus")]
        public static string PlayAsRuler(List<string> args)
        {
            try
            {
                if (args.Count > 0)
                {
                    string kingdom = ArgsToString(args).Replace("\"", "").ToLower();

                    try
                    {
                        if (Main.Settings != null && Main.Settings.Enabled)
                        {
                            TextObject disableReason = TextObject.Empty;

                            if (!Main.Settings.PlayAsExisting)
                            {
                                disableReason = new TextObject("{=restart_plus_n_18}RestartPlus: Playing as existing characters is disabled.");
                                return disableReason.ToString();
                            }

                            if (Game.Current == null || Campaign.Current == null || Hero.MainHero == null)
                            {
                                disableReason = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                                return disableReason.ToString();
                            }

                            try
                            {
                                if (Game.Current?.GameType is CampaignStoryMode && !Main.Settings.AllowCampaign)
                                {
                                    disableReason = new TextObject("{=restart_plus_h_02}RestartPlus: Story Campaign is not supported! Only Sandbox or modded game types are supported.");
                                    return disableReason.ToString();
                                }
                                else if (Game.Current?.GameType is CampaignStoryMode && !StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted)
                                {
                                    disableReason = new TextObject("{=restart_plus_h_03}RestartPlus: Story needs to have reached a later phase in the campaign.");
                                    return disableReason.ToString();
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.PrintError(e.Message, e.StackTrace);
                                Debug.WriteDebugLineOnScreen(e.ToString());
                                Debug.SetCrashReportCustomString(e.Message);
                                Debug.SetCrashReportCustomStack(e.StackTrace);

                                disableReason = new TextObject(e.Message);
                                return disableReason.ToString();
                            }

                            if (Hero.MainHero.CurrentSettlement != null || MobileParty.MainParty.MapEvent != null || Hero.MainHero.IsPrisoner)
                            {
                                disableReason = new TextObject("{=restart_plus_n_05}RestartPlus: Not available. Current main character is busy (either in a settlement, in an encounter, or is a prisoner).");
                                return disableReason.ToString();
                            }

                            try
                            {
                                foreach (var k in Campaign.Current.Kingdoms)
                                {
                                    var name = k.GetName().ToString().ToLower();
                                    var name2 = k.Name.ToString().ToLower();
                                    var name3 = k.InformalName.ToString().ToLower();
                                    if (name == kingdom || name2 == kingdom || name3 == kingdom)
                                    {
                                        if (k.IsEliminated || k.Leader == null)
                                        {
                                            continue;
                                        }

                                        if (k.Leader.Age < (Campaign.Current?.Models?.AgeModel?.HeroComesOfAge ?? 18))
                                        {
                                            disableReason ??= new TextObject("{=restart_plus_n_17}RestartPlus: {CHARACTER} is too young.");
                                            disableReason.SetTextVariable("CHARACTER", k.Leader.Name);
                                        }

                                        if (k.Leader.IsDead)
                                        {
                                            disableReason ??= new TextObject("{=restart_plus_n_16}RestartPlus: {CHARACTER} is already dead.");
                                            disableReason.SetTextVariable("CHARACTER", k.Leader.Name);
                                        }

                                        if (k.Leader.CurrentSettlement != null || k.Leader.PartyBelongedTo?.MapEvent != null || k.Leader.IsPrisoner)
                                        {
                                            disableReason = new TextObject("{=restart_plus_n_09}RestartPlus: Not available. {CHARACTER} is busy (either in a settlement, in an encounter, or is a prisoner).");
                                            disableReason.SetTextVariable("CHARACTER", k.Leader.Name);
                                            return disableReason.ToString();
                                        }

                                        if (k.Leader == Hero.MainHero)
                                        {
                                            disableReason = new TextObject("{=restart_plus_n_13}RestartPlus: Already playing as {CHARACTER}");
                                            disableReason.SetTextVariable("CHARACTER", k.Leader.Name);
                                            return disableReason.ToString();
                                        }

                                        PlayAsCharacterAction.Apply(k.Leader);

                                        var result = new TextObject("{=restart_plus_n_07}RestartPlus: Finalising {CHARACTER} of {OF} as player character.");
                                        result.SetTextVariable("CHARACTER", k.Leader.Name);
                                        result.SetTextVariable("OF", k.Name);
                                        return result.ToString();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteDebugLineOnScreen(e.ToString());
                                return e.ToString();
                            }
                        }
                        else
                        {
                            return new TextObject("{=restart_plus_n_08}Restart+ not enabled!").ToString();
                        }
                    }
                    catch (System.Exception e)
                    {
                        return e.ToString();
                    }
                    return new TextObject("{=restart_plus_n_11}RestartPlus: Kingdom not found").ToString();
                }
                else
                {
                    return Help(args);
                }
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }

        }

        [CommandLineArgumentFunction("play_as_leader", "restart_plus")]
        public static string PlayAsLeader(List<string> args)
        {
            try
            {
                if (args.Count > 0)
                {
                    string clan = ArgsToString(args).Replace("\"", "").ToLower();

                    try
                    {
                        if (Main.Settings != null && Main.Settings.Enabled)
                        {
                            TextObject disableReason = TextObject.Empty;

                            if (!Main.Settings.PlayAsExisting)
                            {
                                disableReason = new TextObject("{=restart_plus_n_18}RestartPlus: Playing as existing characters is disabled.");
                                return disableReason.ToString();
                            }

                            if (Game.Current == null || Campaign.Current == null || Hero.MainHero == null)
                            {
                                disableReason = new TextObject("{=restart_plus_h_01}RestartPlus: Not in an active game!");
                                return disableReason.ToString();
                            }

                            try
                            {
                                if (Game.Current?.GameType is CampaignStoryMode && !Main.Settings.AllowCampaign)
                                {
                                    disableReason = new TextObject("{=restart_plus_h_02}RestartPlus: Story Campaign is not supported! Only Sandbox or modded game types are supported.");
                                    return disableReason.ToString();
                                }
                                else if (Game.Current?.GameType is CampaignStoryMode && !StoryModeManager.Current.MainStoryLine.IsFirstPhaseCompleted)
                                {
                                    disableReason = new TextObject("{=restart_plus_h_03}RestartPlus: Story needs to have reached a later phase in the campaign.");
                                    return disableReason.ToString();
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.PrintError(e.Message, e.StackTrace);
                                Debug.WriteDebugLineOnScreen(e.ToString());
                                Debug.SetCrashReportCustomString(e.Message);
                                Debug.SetCrashReportCustomStack(e.StackTrace);

                                disableReason = new TextObject(e.Message);
                                return disableReason.ToString();
                            }

                            if (Hero.MainHero.CurrentSettlement != null || MobileParty.MainParty.MapEvent != null || Hero.MainHero.IsPrisoner)
                            {
                                disableReason = new TextObject("{=restart_plus_n_05}RestartPlus: Not available. Current main character is busy (either in a settlement, in an encounter, or is a prisoner).");
                                return disableReason.ToString();
                            }

                            try
                            {
                                foreach (var c in Campaign.Current.Clans)
                                {
                                    var name = c.GetName().ToString().ToLower();
                                    var name2 = c.Name.ToString().ToLower();
                                    var name3 = c.InformalName.ToString().ToLower();
                                    if (name == clan || name2 == clan || name3 == clan)
                                    {
                                        if (c.IsEliminated || c.Leader == null)
                                        {
                                            continue;
                                        }

                                        if (c.Leader.Age < (Campaign.Current?.Models?.AgeModel?.HeroComesOfAge ?? 18))
                                        {
                                            disableReason ??= new TextObject("{=restart_plus_n_17}RestartPlus: {CHARACTER} is too young.");
                                            disableReason.SetTextVariable("CHARACTER", c.Leader.Name);
                                        }

                                        if (c.Leader.IsDead)
                                        {
                                            disableReason ??= new TextObject("{=restart_plus_n_16}RestartPlus: {CHARACTER} is already dead.");
                                            disableReason.SetTextVariable("CHARACTER", c.Leader.Name);
                                        }

                                        if (c.Leader.CurrentSettlement != null || c.Leader.PartyBelongedTo?.MapEvent != null || c.Leader.IsPrisoner)
                                        {
                                            disableReason = new TextObject("{=restart_plus_n_09}RestartPlus: Not available. {CHARACTER} is busy (either in a settlement, in an encounter, or is a prisoner).");
                                            disableReason.SetTextVariable("CHARACTER", c.Leader.Name);
                                            return disableReason.ToString();
                                        }

                                        if (c.Leader == Hero.MainHero)
                                        {
                                            disableReason = new TextObject("{=restart_plus_n_13}RestartPlus: Already playing as {CHARACTER}");
                                            disableReason.SetTextVariable("CHARACTER", c.Leader.Name);
                                            return disableReason.ToString();
                                        }

                                        PlayAsCharacterAction.Apply(c.Leader);

                                        var result = new TextObject("{=restart_plus_n_07}RestartPlus: Finalising {CHARACTER} of {OF} as player character.");
                                        result.SetTextVariable("CHARACTER", c.Leader.Name);
                                        result.SetTextVariable("OF", c.Name);
                                        return result.ToString();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteDebugLineOnScreen(e.ToString());
                                return e.ToString();
                            }
                        }
                        else
                        {
                            return new TextObject("{=restart_plus_n_08}Restart+ not enabled!").ToString();
                        }
                    }
                    catch (System.Exception e)
                    {
                        return e.ToString();
                    }
                    return new TextObject("{=restart_plus_n_12}RestartPlus: Clan not found").ToString();
                }
                else
                {
                    return Help(args);
                }
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }

        }

        private static string ArgsToString(List<string> args)
        {
            return string.Join(" ", args).Trim();
        }
    }
}

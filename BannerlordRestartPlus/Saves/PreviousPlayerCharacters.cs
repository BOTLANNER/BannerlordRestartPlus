using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace BannerlordRestartPlus.Saves
{
    public class PreviousPlayerCharacters
    {
        static Color Error = new(178 * 255, 34 * 255, 34 * 255);

        private List<PlayerCharacter>? _list;

        [SaveableProperty(1)]
        public List<PlayerCharacter> History
        {
            get
            {
                if (_list == null)
                {
                    _list = new List<PlayerCharacter>();
                }
                return _list;
            }
            set
            {
                _list = value;
            }
        }

        private static PreviousPlayerCharacters? _instance = null;
        public static PreviousPlayerCharacters? Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public PreviousPlayerCharacters()
        {
        }


        internal void OnLoad()
        {
            try
            {
                foreach (var player in History)
                {
                    player.OnLoad();
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

        public void CharacterChanged(Hero oldHero)
        {
            History.Add(new PlayerCharacter(oldHero));
        }
    }
}


using HarmonyLib;

namespace BannerlordRestartPlus.Patches.Runtime
{
    public interface IRuntimePatch
    {
        public void PatchSubmoduleLoad(Harmony harmony);
        public void PatchAfterMenus(Harmony harmony);
    }
}

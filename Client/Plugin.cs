using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using MakeMedsGreatAgain.Patches;

namespace MakeMedsGreatAgain
{
    // Same GUID as the 4.0 release, so existing F12 settings are kept.
    [BepInPlugin("com.vinihns.makeMedsGreatAgain", "Make Meds Great Again!", "1.3.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;
        public static ConfigEntry<bool> CanWalkInSurgery;
        public static ConfigEntry<bool> CanSprintUsingMeds;

        private void Awake()
        {
            LogSource = Logger;

            CanWalkInSurgery = Config.Bind("Config", "Can walk in surgery", true, "Allows the player to walk while performing surgical treatments.");
            CanSprintUsingMeds = Config.Bind("Config", "Can sprint using meds", true, "Allows sprinting while using meds—because pain can't always slow you down.");

            new PhysicalConditionPatch().Enable();
        }
    }
}

using System.Linq;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace MagicalAstrogy.GTFDGLAB
{
    [BepInPlugin(GUID, "GTFDGLAB", "1.1.4")]
    [BepInProcess("GTFO.exe")]
    public class EntryPoint : BasePlugin
    {
        public const string GUID = "com.MagicalAstrogy.GTFDGLAB";
        public override void Load()
        {

            Logger.LogInstance = Log;

            var harmony = new Harmony(GUID);

            harmony.PatchAll();
            
            Logger.Log($"Patched: ${string.Join(", ", harmony.GetPatchedMethods().ToList().Select(x=>x.Name))}");
        }

        private bool once = false;
    }
}
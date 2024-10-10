using BepInEx;
using BepInEx.Configuration;
using Il2CppSystem.IO;

namespace MagicalAstrogy.GTFDGLAB;

public class ConfigManager
{
    static ConfigManager()
    {
        string text = Path.Combine(Paths.ConfigPath, "GTFDGLab.cfg");
        ConfigFile configFile = new ConfigFile(text, true);
        ConfigManager._baseUrl = configFile.Bind<string>("Magical", "BaseUrl",
            "http://127.0.0.1:8920/", "Url address of service DG-Lab-Coyote-Game-Hub(https://github.com/hyperzlib/DG-Lab-Coyote-Game-Hub).");

        ConfigManager._clientId = configFile.Bind<string>("Magical", "ClientId",
            "all", "Client id of service DG-Lab-Coyote-Game-Hub(https://github.com/hyperzlib/DG-Lab-Coyote-Game-Hub).");

        ConfigManager._timeMultiplier = configFile.Bind<float>("Magical", "TimeMultiplier",
            1f / 12 * 3, "Ratio of damage received to duration.");
        
        ConfigManager._strengthMultiplier = configFile.Bind<float>("Magical", "StrengthMultiplier",
            1.5f, "Ratio of damage received to strength.");

    }

    // Token: 0x17000010 RID: 16
    // (get) Token: 0x06000036 RID: 54 RVA: 0x00002EA4 File Offset: 0x000010A4
    public static string BaseUrl => ConfigManager._baseUrl.Value;

    private static ConfigEntry<string> _baseUrl;
    
    public static string ClientId => ConfigManager._clientId.Value;

    private static ConfigEntry<string> _clientId;
    
    public static float TimeMultiplier => ConfigManager._timeMultiplier.Value;

    private static ConfigEntry<float> _timeMultiplier;
    public static float StrengthMultiplier => ConfigManager._strengthMultiplier.Value;

    private static ConfigEntry<float> _strengthMultiplier;
}
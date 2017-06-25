using System;
using System.Collections.Generic;
using System.IO;
using Discord;
using Newtonsoft.Json;

namespace PickupGameBot.Utility
{
    public class Configuration
    {
        [JsonIgnore]
        public static string FileName { get; private set; } = "Config/config.json";

        public int RelatedTagsLimit { get; set; } = 3;
        public AuthTokens Token { get; set; } = new AuthTokens();
        public CustomSearchConfig CustomSearch { get; set; } = new CustomSearchConfig();
//        public List<ulong> ChannelWhitelist { get; set; }
        
//        public Dictionary<ulong, IEnumerable<ulong>> GuildRoleMap { get; set; } = new Dictionary<ulong, IEnumerable<ulong>>
//        {
//            [104741849960316928] = new ulong[] // Fathom
//            {
//                318139200732135425    // Mod
//            }
//        };
        
        public Configuration() : this("Config/config.json") { }
        public Configuration(string fileName)
        {
            FileName = fileName;
        }
        
        public static void EnsureExists()
        {
            string file = GetFullConfigFilePath();
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                {
                    PrettyConsole.Log(LogSeverity.Warning, "Pugbot", $"Path: '{path}' not found, creating...");
                    Directory.CreateDirectory(path);
                }

                var config = new Configuration();

                PrettyConsole.Log(LogSeverity.Warning, "Pugbot", "Please enter your token: ");
                string token = Console.ReadLine();
                config.Token.Discord = token;
                
                config.SaveJson();
            }
            PrettyConsole.Log(LogSeverity.Info, "Pugbot", "Configuration Loaded");
        }

        public void SaveJson()
        {
            string file = GetFullConfigFilePath();
            File.WriteAllText(file, ToJson());
        }

        public static Configuration Load()
        {
            string file = GetFullConfigFilePath();
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(file));
        }

        private static string GetFullConfigFilePath()
            =>
                Path.Combine(AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin")), FileName);

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public class AuthTokens
    {
        public string Discord { get; set; } = "";
        public string Google { get; set; } = "";
        public string Twitch { get; set; } = "";
    }

    public class CustomSearchConfig
    {
        public string Token { get; set; } = "";
        public string EngineId { get; set; } = "";
        public int ResultCount { get; set; } = 3;
    }
}

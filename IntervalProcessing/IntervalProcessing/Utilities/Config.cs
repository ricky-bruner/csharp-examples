using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace IntervalProcessing.Utilities
{
    public static class Config
    {
        public static IConfigurationRoot GetConfigurationRoot(string jsonFileName)
        {
            return new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).AddJsonFile(jsonFileName).Build();
        }

        public static string GetString(string jsonFileName, string sectionPath)
        {
            string configValue = string.Empty;

            IConfigurationSection configurationSection = GetConfigurationRoot(jsonFileName).GetSection(sectionPath);

            if (configurationSection != null)
            {
                configValue = configurationSection.Value;
            }

            return configValue;
        }

        public static DirectoryInfo GetWorkingDirectory() => new DirectoryInfo(GetString("config.json", "workingDirectory"));

        public static string GetMongoConnectionString() => GetString("config.json", "mongoConnectionString");
        public static string GetMySqlConnectionString() => GetString("config.json", "mySqlConnectionString");

        public static string GetSqlConnectionString() => GetString("config.json", "sqlConnectionString");

        public static string GetDatabase() => GetString("config.json", "databaseName");

        public static bool GetIsServerless()
        {
            bool result = false;
            bool.TryParse(GetString("config.json", "isServerlessDB"), out result);
            return result;
        }

        public static bool GetBool(string propertyName)
        {
            bool result = false;
            bool.TryParse(GetString("config.json", propertyName), out result);
            return result;
        }
    }
}

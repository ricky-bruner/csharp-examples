using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace IntervalProcessing.Configurations
{
    public class CoreConfig : IConfig
    {
        private IConfigurationRoot _configRoot { get; set; }
        public string DatabaseName { get; }
        public string MongoConnectionString { get; }
        public string MySqlConnectionString { get; }
        public string SqlConnectionString { get; }
        public bool IsServerlessDB { get; }
        public DirectoryInfo WorkingDirectory { get; }

        public CoreConfig(string jsonFileName)
        {
            _configRoot = new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).AddJsonFile(jsonFileName).Build();
            DatabaseName = GetString("databaseName");
            MongoConnectionString = GetString("mongoConnectionString");
            MySqlConnectionString = GetString("mySqlConnectionString");
            SqlConnectionString = GetString("sqlConnectionString");
            IsServerlessDB = GetBool("isServerlessDB");
            WorkingDirectory = new DirectoryInfo(GetString("workingDirectory"));

        }

        public string GetString(string sectionPath)
        {
            string configValue = string.Empty;

            IConfigurationSection configurationSection = _configRoot.GetSection(sectionPath);

            if (configurationSection != null)
            {
                configValue = configurationSection.Value;
            }

            return configValue;
        }

        public bool GetBool(string propertyName)
        {
            bool result = false;
            bool.TryParse(GetString(propertyName), out result);
            return result;
        }
    }
}

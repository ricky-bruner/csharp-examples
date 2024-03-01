
namespace IntervalProcessing.Interfaces
{
    internal interface IConfig
    {
        string DatabaseName { get; }
        string MongoConnectionString { get; }
        string MySqlConnectionString { get; }
        string SqlConnectionString { get; }
        bool IsServerlessDB { get; }
        DirectoryInfo WorkingDirectory { get; }
        string GetString(string sectionPath);
    }
}

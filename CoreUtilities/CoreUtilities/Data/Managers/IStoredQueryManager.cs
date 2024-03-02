namespace CoreUtilities.Data.Managers
{
    public interface IStoredQueryManager
    {
        Task<string> GetQueryAsync(string queryName);
    }
}

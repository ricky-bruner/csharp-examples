
namespace IntervalProcessing.Interfaces
{
    public interface IStoredQueryManager
    {
        Task<string> GetQueryAsync(string queryName);
    }
}

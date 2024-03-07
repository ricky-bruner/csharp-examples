using CoreUtilities.Data.Enums;
using CoreUtilities.Data.Models;
using MongoDB.Bson;

namespace CoreUtilities.Data.Managers
{
    public interface IQueryAutomationManager
    {
        Task<List<DataUpdateConfig>> GetQueryBasedDataUpdateConfigsAsync(RunCadence runCadence);
    }
}
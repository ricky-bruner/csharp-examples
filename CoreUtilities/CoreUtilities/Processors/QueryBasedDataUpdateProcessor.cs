using CoreUtilities.Configurations;
using CoreUtilities.Data.Connections;
using CoreUtilities.Data.Enums;
using CoreUtilities.Data.Managers;
using CoreUtilities.Data.Models;
using CoreUtilities.ExtensionMethods;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CoreUtilities.Processors
{
    public class QueryBasedDataUpdateProcessor : IDataProcessor
    {
        private IMongoConnection<BsonDocument> _connection { get; set; }
        private IQueryAutomationManager _queryManager { get; set; }
        private DataUpdateConfig _currentQueryAutomation {  get; set; }
        private List<WriteModel<BsonDocument>> _bulkUpdateList { get; set; }

        public QueryBasedDataUpdateProcessor(IMongoConnection<BsonDocument> connection, IQueryAutomationManager queryManager) 
        {
            _connection = connection;
            _queryManager = queryManager;
            _bulkUpdateList = new List<WriteModel<BsonDocument>>();
        }

        public async Task Execute(RunCadence runCadence) 
        {
            Console.WriteLine($"{DateTime.Now} - Executing {this.GetType().Name} process...");

            List<DataUpdateConfig> queryAutomations = await _queryManager.GetQueryBasedDataUpdateConfigsAsync(runCadence);

            foreach (DataUpdateConfig queryAutomation in queryAutomations) 
            {
                Console.WriteLine($"{DateTime.Now} - Executing {queryAutomation.Name} Query Automation...");

                _currentQueryAutomation = queryAutomation;

                MongoCursor cursor = new MongoCursor(queryAutomation.Query, "{}", queryAutomation.CollectionName, _connection);

                if (queryAutomation.Query.Contains("aggregate("))
                {
                    int startIndex = queryAutomation.Query.IndexOf('(') + 1;
                    int endIndex = queryAutomation.Query.LastIndexOf(')');
                    cursor.query = queryAutomation.Query.Substring(startIndex, endIndex - startIndex);

                    await cursor.ExecuteAggregateCursor(PerformUpdates);
                }
                else
                {
                    await cursor.ExecuteCursor(PerformUpdates);
                }
                
                if (_bulkUpdateList.Count > 0) 
                {
                    await _connection.Collection.BulkWriteAsync(_bulkUpdateList);
                    _bulkUpdateList.Clear();
                }

                _currentQueryAutomation = null;
            }

            Console.WriteLine($"{DateTime.Now} - {this.GetType().Name} process complete...");
        }

        private void PerformUpdates(BsonDocument document) 
        {
            List<UpdateDefinition<BsonDocument>> updateDefinitions = new List<UpdateDefinition<BsonDocument>>();
            
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", document["_id"]);

            foreach (SingleFieldUpdateElement singleUpdate in _currentQueryAutomation.SingleFieldUpdateElements) 
            {
                updateDefinitions.Add(HandleSingleFieldUpdate(singleUpdate));
            }

            foreach (ArrayObjectFieldUpdateElement objectUpdate in _currentQueryAutomation.ArrayObjectFieldUpdateElements)
            {
                updateDefinitions.Add(HandleObjectFieldUpdate(objectUpdate));
            }

            _bulkUpdateList.Add(new UpdateOneModel<BsonDocument>(filter, Builders<BsonDocument>.Update.Combine(updateDefinitions)));
        }

        private UpdateDefinition<BsonDocument> HandleSingleFieldUpdate(SingleFieldUpdateElement singleUpdate) 
        {
            switch (singleUpdate.UpdateType)
            {
                case UpdateType.Add:
                case UpdateType.Change:
                    return Builders<BsonDocument>.Update.Set(singleUpdate.ElementName, singleUpdate.ElementValue);
                case UpdateType.Remove:
                    return Builders<BsonDocument>.Update.Set(singleUpdate.ElementName, "");
                case UpdateType.Delete:
                    return Builders<BsonDocument>.Update.Unset(singleUpdate.ElementName);
                default:
                    throw new NotImplementedException($"{singleUpdate.UpdateType} not implemented in HandleSingleFieldUpdate() of {this.GetType()}");
            }
        }

        private UpdateDefinition<BsonDocument> HandleObjectFieldUpdate(ArrayObjectFieldUpdateElement objectUpdate)
        {
            switch (objectUpdate.UpdateType)
            {
                case UpdateType.Add:
                    return Builders<BsonDocument>.Update.Push(objectUpdate.ArrayProperty, HandleObjectCreation(objectUpdate.ArrayProperty, objectUpdate));

                case UpdateType.Change:
                    string filter = $"{objectUpdate.ArrayProperty}.$[elem].{objectUpdate.MatchingPropertyName}";
                    return Builders<BsonDocument>.Update.Set(filter, objectUpdate.NewValue);
                    
                case UpdateType.Remove:
                    return Builders<BsonDocument>.Update.Set($"{objectUpdate.ArrayProperty}.$.{objectUpdate.MatchingPropertyName}", "");

                case UpdateType.Delete:
                    return Builders<BsonDocument>.Update.PullFilter(objectUpdate.ArrayProperty, Builders<BsonDocument>.Filter.Eq(objectUpdate.MatchingPropertyName, objectUpdate.NewValue));

                default:
                    throw new NotImplementedException($"{objectUpdate.UpdateType} not implemented in HandleObjectFieldUpdate() of {this.GetType()}");
            }
        }

        private BsonDocument HandleObjectCreation(string type, ArrayObjectFieldUpdateElement objectUpdate) 
        {
            BsonDocument workingObject = new BsonDocument();

            switch (type)
            {
                case "notes":
                case "internalNotes":
                    workingObject.Add(objectUpdate.MatchingPropertyName, objectUpdate.NewValue);
                    workingObject.Add("source", $"{_currentQueryAutomation.Name} Query Automation");
                    workingObject.Add("date", DateTime.Now);
                    return workingObject;
                default:
                    throw new NotImplementedException($"{type} not implemented in HandleObjectCreation() of {this.GetType()}");
            }
        }
    }
}

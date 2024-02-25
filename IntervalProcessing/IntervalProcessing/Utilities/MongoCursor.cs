using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace IntervalProcessing.Utilities
{
    public class MongoCursor<T>
    {
        // Executes a find operation based on the provided query and projection, then applies the action to each document.
        public void ExecuteCursor(MongoConnection<T> mongoConnection, string query, string projection, Action<T> processDocument)
        {
            BsonDocument bsonQuery = BsonSerializer.Deserialize<BsonDocument>(query);
            BsonDocument bsonProjection = BsonSerializer.Deserialize<BsonDocument>(projection);
            FindOptions findOptions = new FindOptions();
            //findOptions.Projection = bsonProjection;

            using (IAsyncCursor<T> cursor = mongoConnection.Collection.Find(bsonQuery, findOptions).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (T document in cursor.Current)
                    {
                        processDocument(document);
                    }
                }
            }
        }

        // Executes an aggregation pipeline operation and applies the action to each resulting document.
        public void ExecuteAggregateCursor(MongoConnection<T> mongoConnection, string collectionName, string queryPipeline, Action<T> processDocument)
        {
            AggregateOptions aggregateOptions = new AggregateOptions { AllowDiskUse = true };

            // Deserialize the JSON string into a list of BsonDocument
            List<BsonDocument> stages = BsonSerializer.Deserialize<List<BsonDocument>>(queryPipeline);

            // Convert the list of BsonDocument stages into a PipelineDefinition
            PipelineDefinition<T, BsonDocument> pipelineDefinition = PipelineDefinition<T, BsonDocument>.Create(stages);


            using (IAsyncCursor<T> cursor = mongoConnection.Collection.Aggregate(pipelineDefinition, aggregateOptions).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (T document in cursor.Current)
                    {
                        processDocument(document);
                    }
                }
            }
        }
    }

}

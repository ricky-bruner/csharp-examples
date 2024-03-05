using CoreUtilities.Writers;
using MongoDB.Bson;
using System.Text;

namespace IntervalProcessing.Writers
{
    public class WeeklyMRRInventoryFileWriter<T> : BaseWriter<T> where T : BsonDocument
    {
        private string _del = "|";

        public WeeklyMRRInventoryFileWriter(FileInfo file)
            : base(file)
        {
        }

        protected override string Parse(T document)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(document["mrrId"].ToString());
            builder.Append(_del);

            builder.Append(document["claimNumber"].AsString);
            builder.Append(_del);

            builder.Append(document["provider.number"].AsString);
            builder.Append(_del);

            builder.Append(document["provider.name"].AsString);
            builder.Append(_del);

            builder.Append(document["builtProviderAddress"].AsString);
            builder.Append(_del);

            builder.Append(document["status"].AsString);
            builder.Append(_del);

            builder.Append(document["organization"].AsString);
            builder.Append(_del);

            builder.Append(document["date"].ToUniversalTime().ToString("MM/dd/yyyy"));

            return builder.ToString();
        }

        protected override string GetHeader()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Request Id");
            builder.Append(_del);

            builder.Append("Claim Number");
            builder.Append(_del);

            builder.Append("Provider Number");
            builder.Append(_del);

            builder.Append("Provider Name");
            builder.Append(_del);

            builder.Append("Provider Full Address");
            builder.Append(_del);

            builder.Append("Status");
            builder.Append(_del);

            builder.Append("Assigned Organization");
            builder.Append(_del);

            builder.Append("Request Date");

            return builder.ToString();
        }
    }
}

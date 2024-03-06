using CoreUtilities.ExtensionMethods;
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
            builder.Append(document.GetStringValue("mrrId"));
            builder.Append(_del);

            builder.Append(document.GetStringValue("claimNumber"));
            builder.Append(_del);

            builder.Append(document.GetStringValue("provider.number"));
            builder.Append(_del);

            builder.Append(document.GetStringValue("provider.name"));
            builder.Append(_del);

            builder.Append(document.GetStringValue("builtProviderAddress"));
            builder.Append(_del);

            builder.Append(document.GetStringValue("status"));
            builder.Append(_del);

            builder.Append(document.GetStringValue("organization"));
            builder.Append(_del);

            builder.Append(document.GetElementValue("date").ToUniversalTime().ToString("MM/dd/yyyy"));

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

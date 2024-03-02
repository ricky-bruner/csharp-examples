using CoreUtilities.Writers;
using MongoDB.Bson;
using System.Text;

namespace IntervalProcessing.Writers
{
    public class WeeklyAuditInventoryFileWriter<T> : BaseWriter<T> where T : BsonDocument
    {
        private string _del = "|";

        public WeeklyAuditInventoryFileWriter(FileInfo file)
            : base(file)
        {
        }

        protected override string Parse(T document)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(document["auditId"].ToString());
            builder.Append(_del);

            builder.Append(document["claimNumber"].AsString);
            builder.Append(_del);

            builder.Append(string.Format("{0:F2}", document["amount"].AsInt32 / 100.0));
            builder.Append(_del);

            builder.Append(document["date"].ToUniversalTime().ToString("MM/dd/yyyy"));
            builder.Append(_del);

            builder.Append(document["status"].AsString);
            builder.Append(_del);

            builder.Append(document["organization"].AsString);
            builder.Append(_del);

            builder.Append(document["resourceCode"].AsString);
            builder.Append(_del);

            builder.Append(document["externalId"].AsString);

            return builder.ToString();
        }

        protected override string GetHeader()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Audit Id");
            builder.Append(_del);

            builder.Append("Claim Number");
            builder.Append(_del);

            builder.Append("Audit Amount");
            builder.Append(_del);

            builder.Append("Audit Date");
            builder.Append(_del);

            builder.Append("Status");
            builder.Append(_del);

            builder.Append("Assigned Organization");
            builder.Append(_del);

            builder.Append("Resource Code");
            builder.Append(_del);

            builder.Append("External System Org Id");
            builder.Append(_del);

            return builder.ToString();
        }
    }
}
using MongoDB.Bson;
using System.Text;

namespace IntervalProcessing.Writers
{
    public class DailyAuditInventoryFileWriter : BaseWriter<BsonDocument>
    {
        private string _del = "|";
    
        public DailyAuditInventoryFileWriter(FileInfo file) 
            : base(file) 
        {
        }

        protected override string Parse(BsonDocument document) 
        { 
            StringBuilder builder = new StringBuilder();
            builder.Append(document["auditId"].ToString());
            builder.Append(_del);

            builder.Append(document["claimNumber"].AsString);
            builder.Append(_del);

            builder.Append(document["status"].AsString);
            builder.Append(_del);

            builder.Append(document["organization"].AsString);
            builder.Append(_del);

            builder.Append(document["date"].ToUniversalTime().ToString("MM/dd/yyyy"));
            builder.Append(_del);

            builder.Append(document["amount"].ToString());

            return builder.ToString();
        }

        protected override string GetHeader() 
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Audit Id");
            builder.Append(_del);

            builder.Append("Claim Number");
            builder.Append(_del);

            builder.Append("Status");
            builder.Append(_del);

            builder.Append("Assigned Organization");
            builder.Append(_del);

            builder.Append("Audit Date");
            builder.Append(_del);

            builder.Append("Audit Amount");

            return builder.ToString();
        }
    }
}

using MongoDB.Bson;

namespace CoreUtilities.ExtensionMethods
{
    public static class BsonExtensionMethods
    {
        public static string GetStringValue(this BsonDocument document, string propertyName) => GetElementValue(document, propertyName).ToString();
        
        public static bool HasData(this BsonDocument document, string propertyName)
        {
            bool exists = false;

            if (propertyName.Contains('.'))
            {
                exists = HasNestedData(document, propertyName.Split('.'));
            }
            else
            {
                if (document.Contains(propertyName) && document[propertyName].BsonType != BsonType.Null)
                {
                    exists = true;
                }
            }

            return exists;
        }

        public static bool HasNestedData(this BsonDocument document, string[] nestedElement)
        {
            bool exists = false;

            BsonDocument workingDocument = document;

            for (int index = 0; index < nestedElement.Length; index++)
            {
                if (!workingDocument.HasData(nestedElement[index]))
                {
                    exists = false;
                    break;
                }
                else
                {
                    if (workingDocument[nestedElement[index]].BsonType == BsonType.Document)
                    {
                        workingDocument = workingDocument[nestedElement[index]].AsBsonDocument;
                    }
                }
            }

            return exists;
        }

        public static BsonValue GetElementValue(this BsonDocument document, string propertyName)
        {
            BsonValue value = BsonType.Null;

            if (propertyName.Contains('.'))
            {
                return GetNestedElementValue(document, propertyName.Split('.'));
            }
            else 
            { 
                if (document.HasData(propertyName))
                {
                    value = document[propertyName];
                }
            }

            return value;
        }

        public static BsonValue GetNestedElementValue(this BsonDocument document, string[] nestedElements)
        {
            BsonValue resultValue = BsonType.Null;

            if (nestedElements.Length == 1)
            {
                return GetElementValue(document, nestedElements[0]);
            }

            BsonDocument workingDocument = document;

            for (int index = 0; index < nestedElements.Length; index++)
            {
                if (!workingDocument.HasData(nestedElements[index]))
                {
                    break;
                }
                else
                {
                    if (workingDocument[nestedElements[index]].BsonType == BsonType.Document)
                    {
                        workingDocument = workingDocument[nestedElements[index]].AsBsonDocument;
                    }
                    else if (index == nestedElements.Length - 1)
                    {
                        resultValue = workingDocument[nestedElements[nestedElements.Length - 1]];
                    }
                }
            }

            return resultValue;
        }
    }
}

using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TriggerIntervalProcess
{
    public class Function
    {
        public async Task<string> FunctionHandler(ILambdaContext context)
        {
            using (AmazonSimpleSystemsManagementClient client = new AmazonSimpleSystemsManagementClient())
            {
                string instanceId = await GetParameterAsync(client, "/MyApp/EC2InstanceID");
                string documentName = await GetParameterAsync(client, "/MyApp/DocumentName");
                string dllPath = await GetParameterAsync(client, "/MyApp/DLLPath");
                string processingKey = await GetParameterAsync(client, "/MyApp/ProcessingKey");

                context.Logger.LogLine($"Instance ID: {instanceId}, DocumentName: {documentName}, DLL Path: {dllPath}, Processing Key: {processingKey}");

                SendCommandRequest sendCommandRequest = new SendCommandRequest
                {
                    InstanceIds = new List<string> { instanceId },
                    DocumentName = documentName,
                    Parameters = new Dictionary<string, List<string>>
                    {
                        { "commands", new List<string> { $"dotnet {dllPath} {processingKey}" } }
                    },
                };

                try
                {
                    SendCommandResponse response = await client.SendCommandAsync(sendCommandRequest);
                    return $"Command sent. Command ID: {response.Command.CommandId}";
                }
                catch (Exception ex)
                {
                    context.Logger.LogLine($"Error sending command: {ex.Message}");
                    return $"Error sending command: {ex.Message}";
                }
            }
        }

        private static async Task<string> GetParameterAsync(AmazonSimpleSystemsManagementClient ssmClient, string parameterName)
        {
            GetParameterRequest request = new GetParameterRequest
            {
                Name = parameterName,
                WithDecryption = false
            };

            GetParameterResponse response = await ssmClient.GetParameterAsync(request);
            return response.Parameter.Value;
        }
    }
}
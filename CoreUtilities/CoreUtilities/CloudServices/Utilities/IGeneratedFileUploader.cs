
namespace CoreUtilities.CloudServices.Utilities
{
    public interface IGeneratedFileUploader
    {
        Task Upload(FileInfo file, string s3Location, string source);
    }
}
using CoreUtilities.Data.Models;

namespace CoreUtilities.CloudServices.AWS
{
    public interface IS3Uploader
    {
        Task DeleteFileAsync(string bucketName, string key);
        Task DownloadFileAsync(string filePath, string key);
        Task UploadFileAsync(string filePath, GeneratedFile file);
        Task<string> GetBucketAsync();
    }
}

namespace CoreUtilities.CloudServices.AWS
{
    public interface IS3Uploader
    {
        Task DeleteFileAsync(string bucketName, string key);
        Task DownloadFileAsync(string filePath, string bucketName, string key);
        Task UploadFileAsync(string key, string filePath);
    }
}
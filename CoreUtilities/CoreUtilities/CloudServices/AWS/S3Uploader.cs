using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using CoreUtilities.Data.Managers;
using CoreUtilities.Data.Models;

namespace CoreUtilities.CloudServices.AWS
{
    public class S3Uploader : IS3Uploader
    {
        private IAmazonS3 _s3Client;
        private readonly IAWSSettingsManager _settingsManager;
        private AWSSettings _settings;

        public S3Uploader(IAWSSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public async Task UploadFileAsync(string key, string filePath)
        {
            try
            {
                TransferUtility fileTransferUtility = await GetFileTransferUtilityAsync();

                string locationKey = string.IsNullOrEmpty(key.Trim()) ? Path.GetFileName(filePath) : $"{key}/{Path.GetFileName(filePath)}";

                await fileTransferUtility.UploadAsync(filePath, _settings.RootBucket, locationKey);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
        }

        public async Task DownloadFileAsync(string filePath, string bucketName, string key)
        {
            try
            {
                TransferUtility fileTransferUtility = await GetFileTransferUtilityAsync();
                await fileTransferUtility.DownloadAsync(filePath, bucketName, key);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
        }

        public async Task DeleteFileAsync(string bucketName, string key)
        {
            try
            {
                await ConfigureS3ClientAsync();
                DeleteObjectRequest request = new DeleteObjectRequest() { BucketName = bucketName, Key = key };
                await _s3Client.DeleteObjectAsync(request);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
        }

        private async Task<TransferUtility> GetFileTransferUtilityAsync()
        {
            await ConfigureS3ClientAsync();
            return new TransferUtility(_s3Client);
        }

        private async Task ConfigureS3ClientAsync()
        {
            if (_settings == null)
            {
                _settings = await _settingsManager.GetAWSSettingsAsync();
            }

            if (_s3Client == null)
            {
                _s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, RegionEndpoint.GetBySystemName(_settings.Region));
            }
        }

        private async Task<bool> Exists(IAmazonS3 s3Client, string bucket, string key)
        {
            bool exists = false;

            try
            {
                ListObjectsRequest request = new ListObjectsRequest()
                {
                    BucketName = bucket,
                    Prefix = key
                };

                await ConfigureS3ClientAsync();

                ListObjectsResponse responseTask = await s3Client.ListObjectsAsync(request);

                if (responseTask != null && responseTask.S3Objects != null && responseTask.S3Objects.Count > 0)
                {
                    exists = true;
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                throw;
            }

            return exists;
        }
    }
}
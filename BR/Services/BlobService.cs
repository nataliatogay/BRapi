using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public class BlobService : IBlobService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly CloudBlobClient _cloudBlobClient;
        private readonly CloudBlobContainer _cloudBlobContainer;
        private readonly AzureStorageAccountOptions _options;

        public BlobService(IOptions<AzureStorageAccountOptions> options)
        {
            _options = options.Value;
            if (!CloudStorageAccount.TryParse(_options.ConnectionString, out _cloudStorageAccount))
            {
                throw new Exception("Invalid connection string for Azure Storage Account");
            }
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(_options.RootContainerName);
            Configure();
        }

        public async Task Configure()
        {
            if (await _cloudBlobContainer.CreateIfNotExistsAsync())
            {
                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                await _cloudBlobContainer.SetPermissionsAsync(permissions);
            }
        }

        public async Task<string> UploadImage(string imageString)
        {
            var blobReference = _cloudBlobContainer.GetBlockBlobReference($"{Guid.NewGuid().ToString()}.{"png"}");
            blobReference.Properties.ContentType = "image/jpeg";
            byte[] imgArr = Convert.FromBase64String(imageString);
            await blobReference.UploadFromByteArrayAsync(imgArr, 0, imgArr.Length);
            
            return blobReference.Uri.AbsoluteUri;
        }

        public async Task<bool> DeleteImage(string imagePath)
        {
            var name = imagePath.Substring(imagePath.LastIndexOf('/') + 1);
            var blobReference = _cloudBlobContainer.GetBlockBlobReference(name);
            var res = await blobReference.DeleteIfExistsAsync();
            return res;
        }
    }
}

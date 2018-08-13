using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using TodoApp.CommonServices;

namespace TodoApp.AWSServices
{
    public class StorageService
    {
        private AmazonS3Client _client {get; set;} = new AmazonS3Client();
        private string _bucketName = Environment.GetEnvironmentVariable("ArchiveBucketName");

        public async Task UploadNote(NoteModel note)
        {
            var req = new PutObjectRequest()
            {
                BucketName = _bucketName,
                Key = $"{note.Id}_{note.Name}",
                ContentBody = JsonConvert.SerializeObject(note),
                ContentType = "application/json"
            };
            var response = await _client.PutObjectAsync(req);
            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not upload Note, {response.HttpStatusCode}");
        }
    }
}
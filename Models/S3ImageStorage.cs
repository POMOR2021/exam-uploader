using Amazon.S3;
using Amazon.S3.Model;

namespace ImageUploaderApp.Models;

public class S3ImageStorage
{
    private readonly IAmazonS3 _s3;
    private readonly string _bucket;
    private readonly string _region;

    public S3ImageStorage(IAmazonS3 s3, string bucket, string region)
    {
        _s3 = s3;
        _bucket = bucket;
        _region = region;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = fileName,
            InputStream = fileStream,
            ContentType = contentType,
            CannedACL = S3CannedACL.PublicRead
        };
        await _s3.PutObjectAsync(putRequest);
        return $"https://{_bucket}.storage.yandexcloud.net/{fileName}";
    }

    public string GetUrl(string fileName)
        => $"https://{_bucket}.storage.yandexcloud.net/{fileName}";
}

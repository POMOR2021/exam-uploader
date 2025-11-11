namespace ImageUploaderApp.Models;

public class ImageMeta
{
    public int Id { get; set; }
    public string FileName { get; set; } = null!;
    public DateTime UploadDate { get; set; }
    public string Url { get; set; } = null!;
}

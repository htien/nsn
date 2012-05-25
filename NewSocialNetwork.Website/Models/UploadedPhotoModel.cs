namespace NewSocialNetwork.Website.Models
{
    public class UploadedPhotoModel
    {
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string MimeType { get; set; }
        public int UploadTimestamp { get; set; }
    }
}
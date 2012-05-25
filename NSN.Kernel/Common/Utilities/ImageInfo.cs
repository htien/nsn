using System.IO;

namespace NSN.Common.Utilities
{
    public class ImageInfo
    {
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string MimeType { get; set; }
        public string LinkAccess { get; set; }
        public int UploadTimestamp { get; set; }
        public Stream ImageStream { get; set; }
    }
}

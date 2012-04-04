
using Castle.ActiveRecord;
namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.PhotoInfo]", "dbo", Lazy = true)]
    public class PhotoInfo : ActiveRecordBase<PhotoInfo>
    {
        [Property("PhotoId", Unique = true, NotNull = true)]
        public virtual int PhotoId { get; set; }

        [Property("FileName", Length = 100, NotNull = true)]
        public virtual string FileName { get; set; }

        [Property("FileSize", NotNull = true)]
        public virtual int FileSize { get; set; }

        [Property("MimeType", Length = 150, NotNull = false)]
        public virtual string MimeType { get; set; }

        [Property("Extension", Length = 20, NotNull = true)]
        public virtual string Extension { get; set; }

        [Property("Description", Length = 255, NotNull = false)]
        public virtual string Description { get; set; }

        [Property("Width", NotNull = true)]
        public virtual short Width { get; set; }

        [Property("Height", NotNull = true)]
        public virtual short Height { get; set; }

        public PhotoInfo() { }
    }
}

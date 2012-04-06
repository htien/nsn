using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.PhotoInfo]", "dbo", Lazy = true)]
    public class PhotoInfo : ActiveRecordValidationBase<PhotoInfo>
    {
        [PrimaryKey(PrimaryKeyType.Foreign, "PhotoId")]
        public virtual int PhotoId { get; set; }

        [OneToOne]
        public virtual Photo Photo { get; set; }

        [Property("FileName", Length = 100, NotNull = true)]
        public virtual string FileName { get; set; }

        [Property("FileSize", NotNull = true, Default = "0")]
        public virtual int FileSize { get; set; }

        [Property("MimeType", Length = 150, NotNull = false)]
        public virtual string MimeType { get; set; }

        [Property("Extension", Length = 20, NotNull = true)]
        public virtual string Extension { get; set; }

        [Property("Description", Length = 255, NotNull = false)]
        public virtual string Description { get; set; }

        [Property("Width", NotNull = true, Default = "0")]
        public virtual short Width { get; set; }

        [Property("Height", NotNull = true, Default = "0")]
        public virtual short Height { get; set; }

        public PhotoInfo() { }
    }
}

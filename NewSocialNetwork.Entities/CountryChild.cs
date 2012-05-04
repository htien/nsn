using Castle.ActiveRecord;
using System.Web.Script.Serialization;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.CountryChild]", "dbo", Lazy = true)]
    public class CountryChild : ActiveRecordValidationBase<CountryChild>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "ChildId")]
        public virtual int ChildId { get; set; }

        [BelongsTo("CountryIso", UniqueKey = "UQ_NSN.CountryChild", NotNull = true)]
        public virtual Country Country { get; set; }

        [Property("Name", UniqueKey = "UQ_NSN.CountryChild", Length = 200, NotNull = true)]
        public virtual string Name { get; set; }

        [Property("Ordering", NotNull = true)]
        public virtual short Ordering { get; set; }

        #endregion

        public CountryChild() { }
    }
}
using System;
using System.Collections.Generic;
using System.Web;

using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.CountryChild]", "dbo", Lazy = true)]
    public class CountryChild : ActiveRecordBase<CountryChild>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "ChildId")]
        public virtual int ChildId { get; set; }

        [BelongsTo("CountryIso", NotNull = true)]
        public virtual Country Country { get; set; }

        [Property("Name", Length = 200, NotNull = true)]
        public virtual string Name { get; set; }

        [Property("Ordering", NotNull = true)]
        public virtual short Ordering { get; set; }

        public CountryChild() { }
    }
}
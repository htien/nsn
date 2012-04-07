using System.Collections.Generic;

using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Country]", "dbo", Lazy = true)]
    public class Country : ActiveRecordValidationBase<Country>
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Assigned, "CountryIso", Length = 2)]
        public virtual string CountryIso { get; set; }

        [Property("Name", Unique = true, Length = 80, NotNull = false)]
        public virtual string Name { get; set; }

        [Property("Ordering", NotNull = true, Default = "0")]
        public virtual short Order { get; set; }

        #endregion

        #region Relationship

        [HasMany(typeof(CountryChild), ColumnKey = "CountryIso", Lazy = true)]
        public virtual IList<CountryChild> CountryChilds { get; set; }

        [HasMany(typeof(User), ColumnKey = "CountryIso", Lazy = true)]
        public virtual IList<User> Users { get; set; }

        #endregion

        public Country() { }
    }
}
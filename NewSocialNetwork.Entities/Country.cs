using System.Collections.Generic;

using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Country]", "dbo", Lazy = true)]
    public class Country : ActiveRecordBase<Country>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "CountryIso", Length = 2)]
        public virtual string CountryIso { get; set; }

        [Property("Name", Length = 80, NotNull = false)]
        public virtual string Name { get; set; }

        [Property("Ordering", Default = "0", NotNull = true)]
        public virtual short Order { get; set; }

        [HasMany(typeof(CountryChild), Table = "[NSN.CountryChild]", ColumnKey = "CountryIso", Lazy = true)]
        public virtual IList<CountryChild> CountryChilds { get; set; }

        public Country() { }
    }
}
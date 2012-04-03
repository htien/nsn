using System.Collections.Generic;

using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Country]", "dbo")]
    public class Country : ActiveRecordBase<Country>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "CountryIso", Length = 2)]
        public string CountryIso { get; set; }

        [Property("Name", Length = 80, NotNull = false)]
        public string Name { get; set; }

        [Property("Ordering", Default = "0", NotNull = true)]
        public short Order { get; set; }

        private IList<CountryChild> _CountryChilds = new List<CountryChild>();

        [HasMany(typeof(CountryChild), Table = "[NSN.CountryChild]", ColumnKey = "CountryIso", Lazy = true)]
        public virtual IList<CountryChild> CountryChilds
        {
            get { return _CountryChilds; }
            set { _CountryChilds = value; }
        }

        public Country() { }
    }
}
using System;
using System.Collections.Generic;
using System.Web;

using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.Country]")]
    public class Country : ActiveRecordBase<Country>
    {
        [PrimaryKey("CountryIso", Length=2)]
        public string CountryIso { get; set; }

        [Property("Name", Length=80, NotNull=false)]
        public string Name { get; set; }

        [Property("Ordering", Default="0", NotNull=true)]
        public short Order { get; set; }

        private IList<CountryChild> _CountryChilds = new List<CountryChild>();

        public Country() { }

        public IList<CountryChild> CountryChilds
        {
            get { return _CountryChilds; }
            set { _CountryChilds = value; }
        }
    }
}
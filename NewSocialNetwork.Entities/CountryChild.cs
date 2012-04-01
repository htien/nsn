using System;
using System.Collections.Generic;
using System.Web;

using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    public class CountryChild
    {
        [PrimaryKey("ChildId")]
        public int ChildId { get; set; }

        [BelongsTo("CountryIso")]
        public string CountryIso { get; set; }

        [Property("Name", Length=200, NotNull=true)]
        public string Name { get; set; }

        [Property("Ordering", NotNull=true)]
        public short Ordering { get; set; }

        public CountryChild() { }
    }
}
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.Country]", "dbo", Lazy = true)]
    public class Country : ActiveRecordValidationBase<Country>, IEntity
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

        [ScriptIgnore]
        [HasMany(typeof(CountryChild), ColumnKey = "CountryIso", Lazy = true)]
        public virtual IList<CountryChild> CountryChilds { get; set; }

        [ScriptIgnore]
        [HasMany(typeof(User), ColumnKey = "CountryIso", Lazy = true)]
        public virtual IList<User> Users { get; set; }

        #endregion

        public Country() { }
    }
}
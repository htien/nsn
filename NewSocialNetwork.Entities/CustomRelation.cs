using System.Collections.Generic;
using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.CustomRelation]", "dbo", Lazy = true)]
    public class CustomRelation : ActiveRecordValidationBase<CustomRelation>
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "RelationId")]
        public virtual int RelationId { get; set; }

        [Property("PhraseVarName", Unique = true, Length = 255, NotNull = true)]
        public virtual string PhraseVarName { get; set; }

        [Property("Confirmation", NotNull = true)]
        public virtual byte Confirmation { get; set; }

        #endregion

        #region Relationship

        [HasMany(typeof(CustomRelationData), ColumnKey = "RelationId", Lazy = true)]
        public virtual IList<CustomRelationData> RelationData { get; set; }

        #endregion

        public CustomRelation() { }
    }
}

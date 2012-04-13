using Castle.ActiveRecord;

namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.CustomRelationData]", "dbo", Lazy = true)]
    public class CustomRelationData : ActiveRecordValidationBase<CustomRelationData>, INSNEntity
    {
        #region Properties

        [PrimaryKey(PrimaryKeyType.Identity, "RelationDataId")]
        public virtual int RelationDataId { get; set; }

        [BelongsTo("RelationId", UniqueKey = "UQ_NSN.CustomRelationData", NotNull = true)]
        public virtual CustomRelation Relation { get; set; }

        [BelongsTo("UserId", UniqueKey = "UQ_NSN.CustomRelationData", NotNull = true)]
        public virtual User User { get; set; }

        [BelongsTo("WithUserId", UniqueKey = "UQ_NSN.CustomRelationData", NotNull = false)]
        public virtual User WithUser { get; set; }

        [Property("Status", NotNull = false)]
        public virtual byte Status { get; set; }

        [Property("TotalComment", NotNull = true, Default = "0")]
        public virtual int TotalComment { get; set; }

        [Property("TotalLike", NotNull = true, Default = "0")]
        public virtual int TotalLike { get; set; }

        #endregion

        public CustomRelationData() { }
    }
}

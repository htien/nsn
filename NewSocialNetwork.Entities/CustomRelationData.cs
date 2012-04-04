
using Castle.ActiveRecord;
namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.CustomRelationData]", "dbo", Lazy = true)]
    public class CustomRelationData : ActiveRecordBase<CustomRelationData>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RelationDataId")]
        public virtual int RelationDataId { get; set; }

        [BelongsTo("Relation", NotNull = true)]
        public virtual CustomRelation RelationId { get; set; }

        [BelongsTo("UserId", NotNull = true)]
        public virtual User UserId { get; set; }

        [BelongsTo("WithUserId", NotNull = false)]
        public virtual User WithUserId { get; set; }

        [Property("Status", NotNull = false)]
        public virtual byte Status { get; set; }

        [Property("TotalComment", NotNull = true)]
        public virtual int TotalComment { get; set; }

        [Property("TotalLike", NotNull = true)]
        public virtual int TotalLike { get; set; }

        public CustomRelationData() { }
    }
}

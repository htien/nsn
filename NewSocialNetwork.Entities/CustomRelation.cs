

using Castle.ActiveRecord;
namespace NewSocialNetwork.Entities
{
    [ActiveRecord("[NSN.CustomRelation]", "dbo", Lazy = true)]
    public class CustomRelation : ActiveRecordBase<CustomRelation>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "RelationId")]
        public virtual int RelationId { get; set; }

        [Property("PhraseVarName", Length = 255, NotNull = true)]
        public virtual string PhraseVarName { get; set; }

        [Property("Confirmation", NotNull = true)]
        public virtual byte Confirmation { get; set; }

        public CustomRelation() { }
    }
}

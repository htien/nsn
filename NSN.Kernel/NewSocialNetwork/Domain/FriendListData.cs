using System;
using Castle.ActiveRecord;
using NSN.Framework;

namespace NewSocialNetwork.Domain
{
    [ActiveRecord("[NSN.FriendListData]", "dbo", Lazy = true)]
    public class FriendListData : ActiveRecordValidationBase<FriendListData>, IEntity
    {
        #region Properties

        [CompositeKey]
        public virtual FriendListDataPK Key { get; set; }

        [BelongsTo("ListId", NotNull = true)]
        public virtual FriendList FriendList { get; set; }

        [BelongsTo("FriendUserId", NotNull = true)]
        public virtual User FriendUser { get; set; }

        [Property("Ordering", NotNull = true, Default = "0")]
        public virtual int Ordering { get; set; }

        #endregion

        public FriendListData() { }
    }

    #region Composite Key

    [Serializable]
    public class FriendListDataPK
    {
        [KeyProperty(Column = "ListId", UniqueKey = "UQ_NSN.FriendListData_List")]
        public int ListId { get; set; }

        [KeyProperty(Column = "FriendUserId", UniqueKey = "UQ_NSN.FriendListData_List")]
        public int FriendUserId { get; set; }

        public override int GetHashCode()
        {
            return ListId ^ FriendUserId;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            FriendListDataPK key = obj as FriendListDataPK;
            if (key == null)
            {
                return false;
            }
            if (key.ListId != ListId || key.FriendUserId != FriendUserId)
            {
                return false;
            }
            return true;
        }
    }

    #endregion
}

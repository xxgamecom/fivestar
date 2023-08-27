using ETModel;
using Google.Protobuf.Collections;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    /// <summary>
    /// 亲友圈成员列表
    /// </summary>
    [BsonIgnoreExtraElements]
    public class FriendsCirleMemberInfo: Entity
    {
        // 对应的亲友圈id
        public int FriendsCircleId { get; set; }
        // 亲友圈所有成员的userId
        public RepeatedField<long> MemberList = new RepeatedField<long>();
    }
}
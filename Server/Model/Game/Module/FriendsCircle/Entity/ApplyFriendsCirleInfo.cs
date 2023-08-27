using ETModel;
using Google.Protobuf.Collections;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    /// <summary>
    /// 亲友圈申请信息
    /// </summary>
    [BsonIgnoreExtraElements]
    public class ApplyFriendsCirleInfo: Entity
    {
        // 对应的亲友圈id
        public int FriendsCirleId { get; set; }
        // 申请列表里面的玩家
        public RepeatedField<long> ApplyList = new RepeatedField<long>();
    }
}
using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public partial class MarketInfo: Entity
    {
        public long SellUserId { get; set; } //卖家的UserId
    }
}
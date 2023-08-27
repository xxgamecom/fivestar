using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class EverydayShareInfo: Entity
    {
        public long UserId { get; set; }
        public long ShareTime { get; set; }
    }
}
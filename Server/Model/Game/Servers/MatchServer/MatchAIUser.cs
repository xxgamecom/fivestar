using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class MatchAIUser: Entity
    {
        public long UserId { get; set; }
        public long BeansTotalResult { get; set; } //豆子总结果
    }
}
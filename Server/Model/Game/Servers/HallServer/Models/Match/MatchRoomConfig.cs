using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
    [BsonIgnoreExtraElements]
    public partial class MatchRoomConfig: Entity
    {
        //public long GameEntryId { get; set; }//配置游戏的Id

        //public int GameNumber { get; set; }//人数

        // public RepeatedField<int> RoomConfigs { get; set; }//房间具体配置

        public override void Dispose()
        {
            GameEntryId = 0;
            GameNumber = 0;
            RoomConfigs = null;
            base.Dispose();
        }
    }
}
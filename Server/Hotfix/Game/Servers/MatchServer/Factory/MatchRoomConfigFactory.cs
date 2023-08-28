using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class MatchRoomConfigFactory
    {
        public static MatchRoomConfig Create(int number, int roomId, long gameEntryId, RepeatedField<int> configs)
        {
            var matchRoomConfig = ComponentFactory.Create<MatchRoomConfig>();
            matchRoomConfig.GameNumber = number;
            matchRoomConfig.RoomConfigs = configs;
            matchRoomConfig.MatchRoomId = roomId;
            matchRoomConfig.GameEntryId = gameEntryId;

            return matchRoomConfig;
        }
    }
}
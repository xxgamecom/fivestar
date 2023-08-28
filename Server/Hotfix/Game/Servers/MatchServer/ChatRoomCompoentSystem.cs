using ETModel;

namespace ETHotfix
{
    public static partial class MatchRoomComponentSystem
    {
        // 玩家聊天
        public static bool UserChat(this MatchRoomComponent matchRoomComponent, long userId, ChatInfo chatInfo)
        {
            if (matchRoomComponent.UserIdInRoomIdDic.ContainsKey(userId))
            {
                // 广播聊天信息
                return matchRoomComponent.UserIdInRoomIdDic[userId].UserChat(userId, chatInfo);
            }

            return false;
        }
    }
}
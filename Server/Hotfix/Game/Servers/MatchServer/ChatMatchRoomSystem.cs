using ETModel;

namespace ETHotfix
{
    public static partial class MatchRoomSystem
    {
        // 玩家聊天
        public static bool UserChat(this MatchRoom matchRoom, long userId, ChatInfo chatInfo)
        {
            if (userId == 0)
            {
                return false;
            }

            var actorUserChatInfo = new Actor_UserChatInfo();
            actorUserChatInfo.UserId = userId;
            actorUserChatInfo.ChatInfo = chatInfo;
            for (int i = 0; i < matchRoom.PlayerInfoDic.Count; i++)
            {
                matchRoom.PlayerInfoDic[i].User.SendeSessionClientActor(actorUserChatInfo);
            }

            return true;
        }

        // 根据UserId获取玩家索引
        public static int GetSeatIndex(this MatchRoom matchRoom, long userId)
        {
            for (int i = 0; i < matchRoom.PlayerInfoDic.Count; i++)
            {
                if (matchRoom.PlayerInfoDic[i].User.UserId == userId)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
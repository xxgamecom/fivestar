using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class GateUserComponentSystem
    {
        // 获取当前网关连接User中的信息
        public static User GetUser(this GateUserComponent self, long userId)
        {
            if (!self.UserDict.TryGetValue(userId, out var user))
            {
                //Log.Error($"玩家{userId}不存在或不在游戏中");
            }

            return user;
        }

        // 玩家上线事件
        public static async Task<User> UserOnLine(this GateUserComponent self, long userId, long sessionActorId)
        {
            var user = await UserHelper.QueryUserInfo(userId);
            if (user == null)
            {
                return null;
            }

            // 改变在线状态
            user.IsOnLine = true;
            // 给其他服务器广播玩家上线消息
            self.BroadcastOnAndOffLineMessage(new G2S_UserOnline()
            {
                UserId = userId,
                SessionActorId = sessionActorId
            });
            // 记录玩家信息
            self.UserDict[userId] = user;

            return user;
        }

        // 玩家下线事件
        public static void UserOffline(this GateUserComponent self, long userId)
        {
            long playerSessionActorId = 0;
            if (self.UserDict.ContainsKey(userId))
            {
                playerSessionActorId = self.UserDict[userId].GetUserClientSession().GetComponent<SessionUserComponent>().GamerSessionActorId;
            }
            if (playerSessionActorId != 0)
            {
                // 告诉游戏服 用户下线
                ActorHelper.SendActor(playerSessionActorId, new Actor_UserOffLine());
            }
            if (self.UserDict.ContainsKey(userId))
            {
                self.UserDict.Remove(userId);
            }
            // 给其他服务器广播玩家下线消息
            self.BroadcastOnAndOffLineMessage(new G2S_UserOffline()
            {
                UserId = userId
            });
        }

        // 给其他服务器广播用户 上 下线消息
        public static void BroadcastOnAndOffLineMessage(this GateUserComponent self, IMessage iMessage)
        {
            var appType = StartConfigComponent.Instance.StartConfig.AppType;
            if (appType == AppType.AllServer)
            {
                self.MatchSession.Send(iMessage);
            }
            else
            {
                self.UserSession.Send(iMessage);
                self.MatchSession.Send(iMessage);
            }
        }
    }
}
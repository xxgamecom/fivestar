using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class UserComponentSystem
    {
        public static User GetUser(this UserComponent self, long userId)
        {
            if (!self.mOnlineUserDic.TryGetValue(userId, out var user))
            {
                //Log.Error($"玩家{userId}不存在或不在游戏中");
            }
            return user;
        }

        // 玩家上线事件
        public static async Task<User> UserOnLine(this UserComponent self, long userId, long sessionActorId)
        {
            var user = self.GetUser(userId);
            if (user != null)
            {
                user.ByCompelAccount();
                user.GetComponent<UserGateActorIdComponent>().ActorId = sessionActorId;
                return user; //如果User本就在线 发送强制下线消息 就改变一下actorid 
            }

            user = await self.Query(userId);
            if (user != null)
            {
                user.AddComponent<UserGateActorIdComponent>().ActorId = sessionActorId;
                self.mOnlineUserDic[user.UserId] = user;
                user.IsOnLine = true;
            }

            return user;
        }

        // 玩家下线事件
        public static async void UserOffline(this UserComponent self, long userId)
        {
            if (self.mOnlineUserDic.ContainsKey(userId))
            {
                self.mOnlineUserDic[userId].IsOnLine = false;
                self.mOnlineUserDic[userId].RemoveComponent<UserGateActorIdComponent>();
                await self.dbProxyComponent.Save(self.mOnlineUserDic[userId]);
                self.mOnlineUserDic.Remove(userId);
            }
        }

        // 根据userId查询User
        public static async Task<User> Query(this UserComponent self, long userId)
        {
            var userInId = self.GetUser(userId);
            if (userInId != null)
            {
                return userInId;
            }

            var userList = await self.dbProxyComponent.Query<User>(user => user.UserId == userId);
            if (userList.Count > 0)
            {
                return userList[0];
            }

            return null;
        }

        //用户登陆
        public static async Task<AccountInfo> LoginOrRegister(this UserComponent self, string dataStr, int loginType)
        {
            AccountInfo accountInfo = null;
            switch (loginType)
            {
                case LoginType.Editor:
                case LoginType.Tourist:
                    accountInfo = await self.EditorLogin(dataStr);
                    break;
                case LoginType.WeChat:
                    accountInfo = await self.WeChatLogin(dataStr);
                    break;
                case LoginType.Voucher: //检验凭证
                    accountInfo = await self.VoucherLogin(dataStr);
                    break;
                default:
                    Log.Error("不存在的登陆方式:" + loginType);
                    break;
            }

            // 更新最后登录时间
            if (accountInfo != null)
            {
                accountInfo.LastLoginTime = TimeTool.GetCurrenTimeStamp();
                await self.dbProxyComponent.Save(accountInfo);
            }

            return accountInfo;
        }

        public static async Task<User> SaveUserDB(this UserComponent self, User user)
        {
            await self.dbProxyComponent.Save(user);
            return user;
        }
    }
}
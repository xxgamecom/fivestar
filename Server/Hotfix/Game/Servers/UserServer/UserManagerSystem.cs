using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class UserManagerSystem
    {
        public static User GetUser(this UserManager self, long userId)
        {
            if (!self.OnlineUserDict.TryGetValue(userId, out var user))
            {
                //Log.Error($"玩家{userId}不存在或不在游戏中");
            }
            return user;
        }

        // 玩家上线事件
        public static async Task<User> UserOnLine(this UserManager self, long userId, long sessionActorId)
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
                self.OnlineUserDict[user.UserId] = user;
                user.IsOnLine = true;
            }

            return user;
        }

        // 玩家下线事件
        public static async void UserOffline(this UserManager self, long userId)
        {
            if (self.OnlineUserDict.ContainsKey(userId))
            {
                self.OnlineUserDict[userId].IsOnLine = false;
                self.OnlineUserDict[userId].RemoveComponent<UserGateActorIdComponent>();
                await self.dbProxyComponent.Save(self.OnlineUserDict[userId]);
                self.OnlineUserDict.Remove(userId);
            }
        }

        // 根据userId查询User
        public static async Task<User> Query(this UserManager self, long userId)
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
        public static async Task<AccountInfo> LoginOrRegister(this UserManager self, string dataStr, int loginType)
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

        public static async Task<User> SaveUserDB(this UserManager self, User user)
        {
            await self.dbProxyComponent.Save(user);
            return user;
        }

#region Login

        // 编辑状态下登陆
        public static async Task<AccountInfo> EditorLogin(this UserManager self, string account)
        {
            List<AccountInfo> accountInfos = await self.dbProxyComponent.Query<AccountInfo>(AccountInfo => AccountInfo.Account == account);
            if (accountInfos.Count == 0)
            {
                AccountInfo accountInfo = await UserFactory.EditRegisterCreatUser(account);
                accountInfos.Add(accountInfo);
            }
            return accountInfos[0];
        }

        // 微信登陆
        public static async Task<AccountInfo> WeChatLogin(this UserManager self, string accessTokenAndOpenid)
        {
            var weChatJsonData = WeChatJsonAnalysis.HttpGetUserInfoJson(accessTokenAndOpenid);
            if (weChatJsonData == null)
            {
                return null;
            }

            var accountInfos = await self.dbProxyComponent.Query<AccountInfo>(AccountInfo => AccountInfo.Account == weChatJsonData.unionid);
            if (accountInfos.Count == 0)
            {
                var accountInfo = await UserFactory.WeChatRegisterCreatUser(weChatJsonData);
                accountInfos.Add(accountInfo);
            }
            accountInfos[0].Password = TimeTool.GetCurrenTimeStamp().ToString();
            await self.dbProxyComponent.Save(accountInfos[0]);
            return accountInfos[0];
        }

        // 凭证登陆 就是 userId'|'密码
        public static async Task<AccountInfo> VoucherLogin(this UserManager self, string userIdAndPassword)
        {
            string[] userIdPassword = userIdAndPassword.Split('|');
            if (userIdPassword.Length != 2)
            {
                return null;
            }

            try
            {
                long queryUserId = long.Parse(userIdPassword[0]);
                var accountInfos =
                        await self.dbProxyComponent.Query<AccountInfo>(AccountInfo =>
                                                                               AccountInfo.UserId == queryUserId &&
                                                                               AccountInfo.Password == userIdPassword[1]);
                if (accountInfos.Count > 0)
                {
                    return accountInfos[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }

#endregion

#region Stop Seal Operate

        public static async Task StopSealOperate(this UserManager self, StopSealRecord stopSealRecord, IResponse iResponse)
        {
            var accountInfos = await self.dbProxyComponent.Query<AccountInfo>(coount => coount.UserId == stopSealRecord.StopSealUserId);
            if (accountInfos.Count > 0)
            {
                accountInfos[0].IsStopSeal = stopSealRecord.IsStopSeal;
                await self.dbProxyComponent.Save(accountInfos[0]);
            }
            else
            {
                iResponse.Message = "用户不存在";
            }
            stopSealRecord.Time = TimeTool.GetCurrenTimeStamp();
            await self.dbProxyComponent.Save(stopSealRecord);
        }

#endregion

#region Goods

        public static async Task<User> UserGetGoods(this UserManager self, long userId, List<GetGoodsOne> goodsOnes, int goodsChangeType, bool isShowHintPanel)
        {
            bool userOnline = true;
            User user = self.GetUser(userId);
            if (user == null)
            {
                user = await self.Query(userId);
                userOnline = false;
            }
            if (user == null)
            {
                Log.Error("要增加物品的玩家更本不存在UserId:" + userId);
                return null;
            }

            // 更新物品数量
            user.UpdateGoods(goodsOnes);
            self.SaveGoodsDealRecord(user, goodsOnes, goodsChangeType); //存储物品 变化记录 只会存储钻石的
            await self.SaveUserDB(user);
            if (userOnline)
            {
                var actorUserGetGoods = new Actor_UserGetGoods();

                foreach (var goods in goodsOnes)
                {
                    switch (goods.GoodsId)
                    {
                        case GoodsId.Besans:
                            goods.NowAmount = user.Beans;
                            break;
                        case GoodsId.Jewel:
                            goods.NowAmount = user.Jewel;
                            break;
                    }
                }
                actorUserGetGoods.GetGoodsList.AddRange(goodsOnes.ToArray());
                actorUserGetGoods.IsShowHintPanel = isShowHintPanel;

                // 向User发送Actor
                user.SendSessionClientActor(actorUserGetGoods);
            }
            return user;
        }

        // 存储物品 变化记录 只会存储钻石的
        public static async void SaveGoodsDealRecord(this UserManager self, User user, List<GetGoodsOne> goodsOnes, int changeType)
        {
            for (int i = 0; i < goodsOnes.Count; i++)
            {
                if (goodsOnes[i].GoodsId != GoodsId.Jewel)
                {
                    continue;
                }

                var goodsDealRecord = ComponentFactory.Create<GoodsDealRecord>();
                goodsDealRecord.UserId = user.UserId;
                goodsDealRecord.Amount = goodsOnes[i].GetAmount;
                goodsDealRecord.Type = changeType;
                goodsDealRecord.Time = TimeTool.GetCurrenTimeStamp();
                goodsDealRecord.FinishNowAmount = (int)user.Jewel;
                await self.dbProxyComponent.Save(goodsDealRecord);
            }
        }

#endregion
    }
}
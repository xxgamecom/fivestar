using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class UserSystem
    {
        // 发送消息给User对应的客户端 发给网关Session会有拦截器 自动转发给客户端
        public static void SendSessionClientActor(this User self, IActorMessage iActorMessage)
        {
            if (self.GetComponent<UserGateActorIdComponent>() != null)
            {
                long actorId = self.GetComponent<UserGateActorIdComponent>().ActorId;
                ActorHelper.SendActor(actorId, iActorMessage);
            }
        }

        // 获取User所在连接的网关Session
        public static void SendUserGateSession(this User self, IMessage iMessage)
        {
            long actorId = self.GetComponent<UserGateActorIdComponent>().ActorId;
            if (actorId == 0)
            {
                return;
            }

            // 发送消息给用户所在的网关
            var ipEndPoint = StartConfigComponent.Instance.GetInnerAddress(IdGenerater.GetAppId(actorId));
            Game.Scene.GetComponent<NetInnerComponent>().Get(ipEndPoint).Send(iMessage);
        }

        // 改变用户物品
        public static async Task GoodsChange(this User self, long goodId, int amount, int changeType, bool isShowHint = false, bool isSynchronizationGoods = true)
        {
            var newUser = await UserHelper.GoodsChange(self.UserId, goodId, amount, changeType, isShowHint);
            if (isSynchronizationGoods && newUser != null)
            {
                self.Beans = newUser.Beans;
                self.Jewel = newUser.Jewel;
            }
        }

        // 改变用户物品
        public static async Task GoodsChange(this User self, List<GetGoodsOne> getGoodsOnes, int changeType, bool isShowHint = false)
        {
            var newUser = await UserHelper.GoodsChange(self.UserId, getGoodsOnes, changeType, isShowHint);
            if (newUser != null)
            {
                self.Beans = newUser.Beans;
                self.Jewel = newUser.Jewel;
            }
        }

        // 刷新 同步 物品数量
        public static void RefreshGoods(this User self, IList<GetGoodsOne> getGoodsOnes)
        {
            for (int i = 0; i < getGoodsOnes.Count; i++)
            {
                switch (getGoodsOnes[i].GoodsId)
                {
                    case GoodsId.Besans:
                        self.Beans = getGoodsOnes[i].NowAmount;
                        break;
                    case GoodsId.Jewel:
                        self.Jewel = getGoodsOnes[i].NowAmount;
                        break;
                }
            }
        }

        // 更新物品 防止不同 只能用户服调用这个
        public static void UpdateGoods(this User self, List<GetGoodsOne> getGoodsOnes)
        {
            for (int i = 0; i < getGoodsOnes.Count; i++)
            {
                switch (getGoodsOnes[i].GoodsId)
                {
                    case GoodsId.Besans:
                        self.Beans += getGoodsOnes[i].GetAmount;
                        break;
                    case GoodsId.Jewel:
                        self.Jewel += getGoodsOnes[i].GetAmount;
                        break;
                }
            }
            // UserId小于1000 肯定是AI机器人 豆子可以为负数
            if (self.Beans < 0 && self.UserId > 1000)
            {
                self.Beans = 0;
            }
            if (self.Jewel < 0)
            {
                self.Jewel = 0;
            }
        }

        // 得到用户所对应的客户端Session
        public static Session GetUserClientSession(this User self)
        {
            return self.GetComponent<UserClientSessionComponent>().Session;
        }

        // 网关通知客户端玩家被挤号了
        public static void ByCompelAccount(this User self)
        {
            // 客户端收到这条消息后要主动断开连接
            self.SendSessionClientActor(new Actor_CompelAccount()
            {
                Message = "账号在别处登陆"
            }); 
        }
    }

}
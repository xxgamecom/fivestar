using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class FriendsCircleComponentSystem
    {
        // 创建亲友圈
        public static async Task<FriendsCircle> CreatorFriendsCircle(this FriendsCircleComponent self, long creatorUserId, string name, string announcement, RepeatedField<int> roomConfigs, long gameEntryId, IResponse iResponse)
        {
            User creatorUser = await UserHelper.QueryUserInfo(creatorUserId);
            if (creatorUser.Jewel < 200)
            {
                iResponse.Message = "钻石少于200无法创建亲友圈";
                return null;
            }

            // 效验配置 如果配置错误 会使用默认配置
            if (!RoomConfigIntended.IntendedRoomConfigParameter(roomConfigs, gameEntryId))
            {
                iResponse.Message = "玩法配置错误 无法创建";
                return null;
            }

            var friendsCircle = FriendsCircleFactory.Create(name, creatorUserId, roomConfigs, announcement);
            await friendsCircle.SucceedJoinFriendsCircle(creatorUserId); //成功加入到亲友圈 这个方法和保存亲友圈数据到数据库
            return friendsCircle;
        }

        // 获取推荐亲友圈信息
        public static async Task<List<FriendsCircle>> GetRecommendFriendsCircle(this FriendsCircleComponent self, int startIndex, int count)
        {
            if (self.RecommendFriendsCircleList.Count > startIndex + count)
            {
                return self.RecommendFriendsCircleList.GetRange(startIndex, count);
            }
            
            if (self.RecommendFriendsCircleList.Count >= count)
            {
                return self.RecommendFriendsCircleList.GetRange(self.RecommendFriendsCircleList.Count - count, count);
            }
            else
            {
                return self.RecommendFriendsCircleList;
            }
        }

        // 修改推荐亲友圈配置
        public static void AlterRecommend(this FriendsCircleComponent self, FriendsCircle friendsCircle, bool isRecommend)
        {
            if (isRecommend)
            {
                if (!FriendsCircleComponent.Ins.RecommendFriendsCircleList.Contains(friendsCircle))
                {
                    FriendsCircleComponent.Ins.RecommendFriendsCircleList.Add(friendsCircle);
                }
            }
            else
            {
                if (FriendsCircleComponent.Ins.RecommendFriendsCircleList.Contains(friendsCircle))
                {
                    FriendsCircleComponent.Ins.RecommendFriendsCircleList.Remove(friendsCircle);
                }
            }
        }

        // 玩家退出亲友圈
        public static async void UserOutFriendsCircle(this FriendsCircleComponent self, long userId, int friendsCircleId)
        {
            var friendsCircle = await self.QueryFriendsCircle(friendsCircleId);
        }

        // 存储到数据库
        public static async Task SaveDB(this FriendsCircleComponent self, ComponentWithId component)
        {
            await self.dbProxyComponent.Save(component);
        }

        // 获得新创建的亲友圈Id
        public static int GetNewFriendsCircleId(this FriendsCircleComponent self)
        {
            return ++self.FriendsCircleMaxId;
        }
    }
}
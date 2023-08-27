using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取推荐亲友圈列表
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_GetRecommendFriendsCircleHandler: AMRpcHandler<C2F_GetRecommendFriendsCircle, F2C_GetRecommendFriendsCircle>
    {
        protected override async void Run(Session session, C2F_GetRecommendFriendsCircle message, Action<F2C_GetRecommendFriendsCircle> reply)
        {
            var response = new F2C_GetRecommendFriendsCircle();
            try
            {
                var friendsCircle = await FriendsCircleComponent.Ins.GetRecommendFriendsCircle(message.StartIndex, 5);
                response.FriendsCircleInfos.Add(friendsCircle.ToArray());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取亲友圈排行榜信息
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_GetRankingListInfoHandler: AMRpcHandler<C2F_GetRankingListInfo, F2C_GetRankingListInfo>
    {
        protected override async void Run(Session session, C2F_GetRankingListInfo message, Action<F2C_GetRankingListInfo> reply)
        {
            var response = new F2C_GetRankingListInfo();
            try
            {
                var ranKingPlayerInfos = await FriendsCircleComponent.Ins.QueryRankingInfo(message.FriendsCrircleId);
                response.PlayerInfos.Add(ranKingPlayerInfos.ToArray());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
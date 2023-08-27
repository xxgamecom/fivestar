using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 每周一 整点刷新消息
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class L2S_WeekRefreshHandler: AMHandler<L2S_WeekRefresh>
    {
        protected override async void Run(Session session, L2S_WeekRefresh message)
        {
            try
            {
                // 每周整点刷新排行榜
                var ranKingPlayerInfos = await FriendsCircleComponent.Ins.dbProxyComponent.Query<RanKingPlayerInfo>(info => true);
                for (int i = 0; i < ranKingPlayerInfos.Count; i++)
                {
                    ranKingPlayerInfos[i].TotalNumber = 0;
                    ranKingPlayerInfos[i].TotalScore = 0;
                }
                await FriendsCircleComponent.Ins.dbProxyComponent.Save(ranKingPlayerInfos);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
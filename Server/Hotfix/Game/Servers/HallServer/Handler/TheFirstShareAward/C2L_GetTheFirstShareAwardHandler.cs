using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取每日首次分享朋友圈的奖励钻石数量
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetTheFirstShareAwardHandler: AMRpcHandler<C2L_GetTheFirstShareAward, L2C_GetTheFirstShareAward>
    {
        protected override void Run(Session session, C2L_GetTheFirstShareAward message, Action<L2C_GetTheFirstShareAward> reply)
        {
            var response = new L2C_GetTheFirstShareAward();
            try
            {
                response.JeweleAmount = GameLobby._TheFirstShareRewardNum;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
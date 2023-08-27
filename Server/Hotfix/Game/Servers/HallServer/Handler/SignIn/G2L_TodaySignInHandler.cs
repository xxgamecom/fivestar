using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class G2L_TodaySignInHandler: AMRpcHandler<C2L_TodaySignIn, L2C_TodaySignIn>
    {
        protected override async void Run(Session session, C2L_TodaySignIn message, Action<L2C_TodaySignIn> reply)
        {
            var response = new L2C_TodaySignIn();
            try
            {
                bool isSucceed = await SingInActivityComponent.Ins.UserTodaySingIn(message.UserId);
                if (isSucceed)
                {
                }
                else
                {
                    response.Message = "今日已经签到请勿重复签到";
                }
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

    }
}
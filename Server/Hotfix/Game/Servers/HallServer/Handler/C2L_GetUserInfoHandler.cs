using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 查询用户信息
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetUserInfoHandler: AMRpcHandler<C2L_GetUserInfo, L2C_GetUserInfo>
    {
        protected override async void Run(Session session, C2L_GetUserInfo message, Action<L2C_GetUserInfo> reply)
        {
            var response = new L2C_GetUserInfo();
            try
            {
                List<User> users = await UserHelper.QueryUserInfo(message.QueryUserIds);
                response.UserInfos.Add(users.ToArray());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
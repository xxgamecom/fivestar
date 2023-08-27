using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 查询用户所在的所有亲友id列表
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_GetSelfFriendsListHandler: AMRpcHandler<C2F_GetSelfFriendsList, F2C_GetSelfFriendsList>
    {
        protected override async void Run(Session session, C2F_GetSelfFriendsList message, Action<F2C_GetSelfFriendsList> reply)
        {
            var response = new F2C_GetSelfFriendsList();
            try
            {
                response.FriendsCrircleIds = (await FriendsCircleComponent.Ins.QueryUserInFriendsCircle(message.UserId)).FriendsCircleIdList;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
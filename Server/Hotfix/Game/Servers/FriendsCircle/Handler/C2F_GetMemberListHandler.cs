using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取亲友圈成员列表
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_GetMemberListHandler: AMRpcHandler<C2F_GetMemberList, F2C_GetMemberList>
    {
        protected override async void Run(Session session, C2F_GetMemberList message, Action<F2C_GetMemberList> reply)
        {
            var response = new F2C_GetMemberList();
            try
            {
                var friendsCircle = await FriendsCircleComponent.Ins.QueryFriendsCircle(message.FriendsCrircleId);
                if (friendsCircle == null)
                {
                    response.Message = "亲友圈不存在";
                    reply(response);
                    return;
                }
                if (!friendsCircle.ManageUserIds.Contains(message.UserId))
                {
                    response.Message = "管理权限不足";
                    reply(response);
                    return;
                }

                var memberInfo = await FriendsCircleComponent.Ins.QueryFriendsCircleMember(message.FriendsCrircleId);
                response.MemberUserIdList = memberInfo.MemberList;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
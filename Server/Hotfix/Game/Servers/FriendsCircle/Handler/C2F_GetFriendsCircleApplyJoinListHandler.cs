﻿using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取亲友圈申请列表
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_GetFriendsCircleApplyJoinListHandler: AMRpcHandler<C2F_GetFriendsCircleApplyJoinList, F2C_GetFriendsCircleApplyJoinList>
    {
        protected override async void Run(Session session, C2F_GetFriendsCircleApplyJoinList message, Action<F2C_GetFriendsCircleApplyJoinList> reply)
        {
            var response = new F2C_GetFriendsCircleApplyJoinList();
            try
            {
                var applyFriendsCirleInfo = await FriendsCircleComponent.Ins.QueryFriendsCircleApply(message.FriendsCrircleId);
                response.ApplyJoinUserIdList = applyFriendsCirleInfo.ApplyList;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
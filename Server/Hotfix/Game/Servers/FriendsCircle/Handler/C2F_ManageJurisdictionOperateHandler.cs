﻿using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 操作管理权限
    /// </summary>
    [MessageHandler(AppType.FriendsCircle)]
    public class C2F_ManageJurisdictionOperateHandler: AMRpcHandler<C2F_ManageJurisdictionOperate, F2C_ManageJurisdictionOperate>
    {
        protected override async void Run(Session session, C2F_ManageJurisdictionOperate message, Action<F2C_ManageJurisdictionOperate> reply)
        {
            var response = new F2C_ManageJurisdictionOperate();
            try
            {
                var friendsCircle = await FriendsCircleComponent.Ins.QueryFriendsCircle(message.FriendsCrircleId);
                if (friendsCircle == null)
                {
                    response.Message = "亲友圈不存在";
                    reply(response);
                    return;
                }
                if (friendsCircle.CreateUserId != message.UserId)
                {
                    response.Message = "管理权限不足";
                    reply(response);
                    return;
                }
                
                await friendsCircle.OperateManageJurisdiction(message.OperateUserId, message.IsSetManage, response);
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
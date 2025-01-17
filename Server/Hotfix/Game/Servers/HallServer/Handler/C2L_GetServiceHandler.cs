﻿using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class C2L_GetServiceHandler: AMRpcHandler<C2L_GetService, L2C_GetService>
    {
        protected override void Run(Session session, C2L_GetService message, Action<L2C_GetService> reply)
        {
            var response = new L2C_GetService();
            try
            {
                response.ServiceInfos = GameLobby.Ins.ServiceInfosRepeatedField;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
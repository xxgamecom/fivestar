﻿using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Manager)]
    public class C2M_ReloadHandler: AMRpcHandler<C2M_Reload, M2C_Reload>
    {
        protected override async void Run(Session session, C2M_Reload message, Action<M2C_Reload> reply)
        {
            var response = new M2C_Reload();
            if (message.Account != AdminHelper.Account && message.Password != AdminHelper.Password)
            {
                Log.Error($"error reload account and password: {MongoHelper.ToJson(message)}");
                return;
            }

            try
            {
                var startConfigComponent = Game.Scene.GetComponent<StartConfigComponent>();
                var netInnerComponent = Game.Scene.GetComponent<NetInnerComponent>();
                foreach (StartConfig startConfig in startConfigComponent.GetAll())
                {
                    var innerConfig = startConfig.GetComponent<InnerConfig>();
                    var serverSession = netInnerComponent.Get(innerConfig.IPEndPoint);
                    await serverSession.Call(new M2A_Reload());
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
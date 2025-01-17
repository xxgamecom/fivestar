﻿using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.DB)]
    public class DBQueryRequestHandler: AMRpcHandler<DBQueryRequest, DBQueryResponse>
    {
        protected override async void Run(Session session, DBQueryRequest message, Action<DBQueryResponse> reply)
        {
            var response = new DBQueryResponse();
            try
            {
                var dbCacheComponent = Game.Scene.GetComponent<DBCacheComponent>();
                var component = await dbCacheComponent.Get(message.CollectionName, message.Id);

                response.Component = component;

                if (message.NeedCache && component != null)
                {
                    dbCacheComponent.AddToCache(component, message.CollectionName);
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
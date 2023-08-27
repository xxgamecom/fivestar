using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.DB)]
    public class DBQueryBatchRequestHandler: AMRpcHandler<DBQueryBatchRequest, DBQueryBatchResponse>
    {
        protected override async void Run(Session session, DBQueryBatchRequest message, Action<DBQueryBatchResponse> reply)
        {
            var response = new DBQueryBatchResponse();
            try
            {
                var dbCacheComponent = Game.Scene.GetComponent<DBCacheComponent>();
                var components = await dbCacheComponent.GetBatch(message.CollectionName, message.IdList);

                response.Components = components;

                if (message.NeedCache)
                {
                    foreach (ComponentWithId component in components)
                    {
                        dbCacheComponent.AddToCache(component, message.CollectionName);
                    }
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
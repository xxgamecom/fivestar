using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.DB)]
    public class DBQueryJsonRequestHandler: AMRpcHandler<DBQueryJsonRequest, DBQueryJsonResponse>
    {
        protected override async void Run(Session session, DBQueryJsonRequest message, Action<DBQueryJsonResponse> reply)
        {
            var response = new DBQueryJsonResponse();
            try
            {
                var dbCacheComponent = Game.Scene.GetComponent<DBCacheComponent>();
                List<ComponentWithId> components = await dbCacheComponent.GetJson(message.CollectionName, message.Json);
                response.Components = components;

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
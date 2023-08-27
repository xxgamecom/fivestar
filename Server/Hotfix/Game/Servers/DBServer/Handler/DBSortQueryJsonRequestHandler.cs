using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.DB)]
    public class DBSortQueryJsonRequestHandler: AMRpcHandler<DBSortQueryJsonRequest, DBQueryJsonResponse>
    {
        private string[] strs = new string[3];
        protected override async void Run(Session session, DBSortQueryJsonRequest message, Action<DBQueryJsonResponse> reply)
        {
            var response = new DBQueryJsonResponse();
            try
            {
                var dbCacheComponent = Game.Scene.GetComponent<DBCacheComponent>();
                strs[0] = message.CollectionName;
                strs[1] = message.QueryJson;
                strs[2] = message.SortJson;
                var components = await dbCacheComponent.GetJson(strs, message.Count);
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
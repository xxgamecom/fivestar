using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class R2G_GetLoginKeyHandler: AMRpcHandler<R2G_GetLoginKey, G2R_GetLoginKey>
    {
        protected override void Run(Session session, R2G_GetLoginKey message, Action<G2R_GetLoginKey> reply)
        {
            var response = new G2R_GetLoginKey();
            try
            {
                long accessToken = RandomHelper.RandInt64();
                Game.Scene.GetComponent<GateSessionKeyComponent>().Add(accessToken, message.UserId);
                response.Key = accessToken;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
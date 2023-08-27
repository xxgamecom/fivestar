using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class G2L_GetMatchRoomConfigsHandler: AMRpcHandler<C2L_GetMatchRoomConfigs, L2C_GetMatchRoomConfigs>
    {
        protected override void Run(Session session, C2L_GetMatchRoomConfigs message, Action<L2C_GetMatchRoomConfigs> reply)
        {
            var response = new L2C_GetMatchRoomConfigs();
            try
            {
                var matchRoomConfigs = Game.Scene.GetComponent<GameMatchRoomConfigComponent>().GetMatachRoomConfigs(message.ToyGameId);
                response.MatchRoomConfigs.AddRange(matchRoomConfigs.ToArray());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

    }
}
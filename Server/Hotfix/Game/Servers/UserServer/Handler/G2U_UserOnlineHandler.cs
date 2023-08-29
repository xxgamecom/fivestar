using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.User)]
    public class G2U_UserOnlineHandler: AMHandler<G2S_UserOnline>
    {
        protected override void Run(Session session, G2S_UserOnline message)
        {
            try
            {
                Game.Scene.GetComponent<UserManager>().UserOnLine(message.UserId, message.SessionActorId);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
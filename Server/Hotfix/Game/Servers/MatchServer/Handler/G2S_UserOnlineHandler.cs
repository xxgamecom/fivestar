using System;
using ETModel;

namespace ETHotfix.GameGather.Common.Handler.User
{
    /// <summary>
    /// 用户上线
    /// </summary>
    [MessageHandler(AppType.Match)]
    public class G2S_UserOnlineHandler: AMHandler<G2S_UserOnline>
    {
        protected override void Run(Session session, G2S_UserOnline message)
        {
            try
            {
                if (MatchRoomComponent.Ins.JudgeUserIsGameIn(message.UserId, message.SessionActorId))
                {
                    Game.Scene.GetComponent<MatchRoomComponent>().PlayerOnLine(message.UserId, message.SessionActorId); //通知其他玩家 用户上线
                }
                else
                {
                    // 通知客户端 用户不在游戏中
                    ActorHelp.SendActor(message.SessionActorId, new Actor_BeingInGame()
                    {
                        IsGameBeing = false
                    });
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
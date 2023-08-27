using ETModel;

namespace ETHotfix
{
    public static class ActorHelper
    {
        public static void SendActor(long actorId, IActorMessage iActorMessage)
        {
            if (actorId == 0)
            {
                return;
            }

            var actorSender = Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(actorId);
            actorSender.Send(iActorMessage);
        }
    }
}
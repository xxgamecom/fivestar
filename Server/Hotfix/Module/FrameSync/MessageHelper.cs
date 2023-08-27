using ETModel;

namespace ETHotfix
{
    public static class MessageHelper
    {
        public static void Broadcast(IActorMessage message)
        {
            Unit[] units = Game.Scene.GetComponent<UnitComponent>().GetAll();
            var actorLocationSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
            foreach (Unit unit in units)
            {
                var unitGateComponent = unit.GetComponent<UnitGateComponent>();
                if (unitGateComponent.IsDisconnect)
                {
                    continue;
                }

                ActorHelper.SendActor(unitGateComponent.GateSessionActorId, message);
            }
        }
    }
}
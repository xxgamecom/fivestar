using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class SessionUserComponentDestroySystem: DestroySystem<SessionUserComponent>
    {
        public override void Destroy(SessionUserComponent self)
        {
            // 被挤号后 user会置 就不要做下线处理 因为 挤的玩家在线上
            if (self.User != null)
            {
                Game.Scene.GetComponent<GateUserManager>().UserOffline(self.UserId);
                self.User.Dispose();
                Log.Info($"玩家{self.UserId}下线SessionDisconnect");
            }
            self.GamerSessionActorId = 0;
        }
    }

    public static class SessionUserComponentSystem
    {

    }
}
namespace ETModel
{
    [ObjectSystem]
    public class GateUserManagerAwakeSystem: AwakeSystem<GateUserManager>
    {
        public override void Awake(GateUserManager self)
        {
            self.Awake();
        }
    }
}
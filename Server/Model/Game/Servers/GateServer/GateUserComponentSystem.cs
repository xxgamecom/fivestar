namespace ETModel
{
    [ObjectSystem]
    public class GateUserComponentAwakeSystem: AwakeSystem<GateUserComponent>
    {
        public override void Awake(GateUserComponent self)
        {
            self.Awake();
        }
    }
}
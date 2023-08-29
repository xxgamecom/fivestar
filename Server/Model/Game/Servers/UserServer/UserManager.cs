using System.Collections.Generic;
using ETHotfix;

namespace ETModel
{
    public class UserManager: Component
    {
        public static UserManager Ins { private set; get; }

        public DBProxyComponent dbProxyComponent;

        public readonly Dictionary<long, User> OnlineUserDict = new Dictionary<long, User>();

        public void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
        }
    }

    [ObjectSystem]
    public class UserManagerAwakeSystem: AwakeSystem<UserManager>
    {
        public override void Awake(UserManager self)
        {
            self.Awake();
        }
    }
}
using System.Collections.Generic;
using ETHotfix;

namespace ETModel
{
    [ObjectSystem]
    public class UserComponentAwakeSystem: AwakeSystem<UserComponent>
    {
        public override void Awake(UserComponent self)
        {
            self.Awake();
        }
    }

    public class UserComponent: Component
    {
        public static UserComponent Ins { private set; get; }
        
        public DBProxyComponent dbProxyComponent;
        public readonly Dictionary<long, User> mOnlineUserDic = new Dictionary<long, User>();
        
        public void Awake()
        {
            Ins = this;
            dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
        }

    }
}
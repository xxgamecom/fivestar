using System.Collections.Generic;
using ETHotfix;

namespace ETModel
{

    /// <summary>
    /// 网关用户
    /// </summary>
    public class GateUserComponent: Component
    {
        public static GateUserComponent Ins { private set; get; }

        public readonly Dictionary<long, User> UserDict = new Dictionary<long, User>();

        private Session userSession;
        private Session matchSession;

        public Session UserSession
        {

            get
            {
                if (userSession == null)
                {
                    userSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.User);
                }

                return userSession;
            }
        }

        public Session MatchSession
        {
            get
            {
                if (matchSession == null)
                {
                    matchSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.Match);
                }

                return matchSession;
            }
        }

        public void Awake()
        {
            Ins = this;
        }
    }
}
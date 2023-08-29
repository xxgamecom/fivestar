using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_GateLoginHandler: AMRpcHandler<C2G_GateLogin, G2C_GateLogin>
    {
        protected override async void Run(Session session, C2G_GateLogin message, Action<G2C_GateLogin> reply)
        {
            var response = new G2C_GateLogin();
            try
            {
                long userId = Game.Scene.GetComponent<GateAccessTokenManager>().Get(message.Key);

                // 添加收取Actor消息组件 并且本地化一下 就是所有服务器都能向这个对象发 并添加一个消息拦截器
                await session.AddComponent<MailBoxComponent, string>(ActorInterceptType.GateSession).AddLocation();

                var gateUserManager = Game.Scene.GetComponent<GateUserManager>();
                
                // 通知GateUserComponent组件和用户服玩家上线 并获取User实体
                var user = await gateUserManager.UserOnLine(userId, session.Id);
                if (user == null)
                {
                    response.Message = "用户信息查询不到";
                    reply(response);
                    return;
                }

                // user <-> session 互相关联
                user.AddComponent<UserClientSessionComponent>().Session = session; // 记录客户端session在User中
                session.AddComponent<SessionUserComponent>().User = user;          // 给Session组件添加下线监听组件和添加User实体

                // 返回客户端User信息和 当前服务器时间
                response.User = user;
                response.ServerTime = TimeTool.GetCurrenTimeStamp();
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

    }
}
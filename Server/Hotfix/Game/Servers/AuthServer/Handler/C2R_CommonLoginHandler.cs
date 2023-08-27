using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_CommonLoginHandler: AMRpcHandler<C2R_CommonLogin, R2C_CommonLogin>
    {
        protected override async void Run(Session session, C2R_CommonLogin message, Action<R2C_CommonLogin> reply)
        {
            var response = new R2C_CommonLogin();
            try
            {
                // 向用户服验证（注册/登陆）并获得一个用户ID
                var userSession = Game.Scene.GetComponent<NetInnerSessionComponent>().Get(AppType.User);
                var u2RVerifyUser = await userSession.Call(new R2U_VerifyUser()
                {
                    LoginType = message.LoginType,
                    PlatformType = message.PlatformType,
                    DataStr = message.DataStr,
                    // IpAddress=session.RemoteAddress.Address.ToString(),
                }) as U2R_VerifyUser;
                
                // 如果Message不为空 说明 验证失败
                if (!string.IsNullOrEmpty(u2RVerifyUser.Message))
                {
                    response.Message = u2RVerifyUser.Message;
                    reply(response);
                    return;
                }

                // 随机分配一个Gate
                var config = Game.Scene.GetComponent<RealmGateAddressComponent>().GetGateAddress();
                var innerAddress = config.GetComponent<InnerConfig>().IPEndPoint;
                var gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(innerAddress);
                // 向gate请求一个key,客户端可以拿着这个key连接gate
                var g2RGetLoginKey = await gateSession.Call(new R2G_GetLoginKey()
                {
                    UserId = u2RVerifyUser.UserId
                }) as G2R_GetLoginKey;

                string outerAddress = config.GetComponent<OuterConfig>().Address2;
                response.Address = outerAddress;
                response.Key = g2RGetLoginKey.Key;
                response.LoginVoucher = u2RVerifyUser.UserId.ToString() + '|' + u2RVerifyUser.Password;

                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
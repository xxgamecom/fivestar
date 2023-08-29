using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取在线人数
    /// </summary>
    [MessageHandler(AppType.User)]
    public class C2U_GetOnLineNumberHandler : AMRpcHandler<C2U_GetOnLineNumber, U2C_GetOnLineNumber>
    {
        protected override async void Run(Session session, C2U_GetOnLineNumber message, Action<U2C_GetOnLineNumber> reply)
        {
            var response = new U2C_GetOnLineNumber();
            try
            {

                response.OnLineNumber = UserManager.Ins.OnlineUserDict.Count;
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
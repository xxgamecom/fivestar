using System;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 发起购买商品的请求
    /// </summary>
    [MessageHandler(AppType.Lobby)]
    public class C2L_BuyCommodityHandler: AMRpcHandler<C2L_BuyCommodity, L2C_BuyCommodity>
    {
        protected override async void Run(Session session, C2L_BuyCommodity message, Action<L2C_BuyCommodity> reply)
        {
            var response = new L2C_BuyCommodity();
            try
            {
                var weChatOrderInfo = await TopUpComponent.Ins.RequestTopUp(message.UserId, message.CommodityId, response);
                if (weChatOrderInfo != null)
                {
                    response.PrepayId = weChatOrderInfo.prepayId;
                    response.NonceStr = weChatOrderInfo.nonceStr;
                }
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}
using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Lobby)]
    public class G2L_GetCommodityListHandler: AMRpcHandler<C2L_GetCommodityList, L2C_GetCommodityList>
    {
        protected override void Run(Session session, C2L_GetCommodityList message, Action<L2C_GetCommodityList> reply)
        {
            var response = new L2C_GetCommodityList();
            try
            {
                var shoppingCommodity = Game.Scene.GetComponent<ShoppingCommodityComponent>();

                response.BeansList.AddRange(shoppingCommodity.GetBeansList());
                response.JewelList.AddRange(shoppingCommodity.GetJewelList());
                reply(response);
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }

    }
}
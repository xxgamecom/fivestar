using System.Collections.Generic;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class UserComponentStopSealSystem
    {
        public static async Task StopSealOperate(this UserComponent self, StopSealRecord stopSealRecord, IResponse iResponse)
        {
            var accountInfos = await self.dbProxyComponent.Query<AccountInfo>(coount => coount.UserId == stopSealRecord.StopSealUserId);
            if (accountInfos.Count > 0)
            {
                accountInfos[0].IsStopSeal = stopSealRecord.IsStopSeal;
                await self.dbProxyComponent.Save(accountInfos[0]);
            }
            else
            {
                iResponse.Message = "用户不存在";
            }
            stopSealRecord.Time = TimeTool.GetCurrenTimeStamp();
            await self.dbProxyComponent.Save(stopSealRecord);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    public static class UserComponentLoginSystem
    {
        // 编辑状态下登陆
        public static async Task<AccountInfo> EditorLogin(this UserComponent self, string account)
        {
            List<AccountInfo> accountInfos = await self.dbProxyComponent.Query<AccountInfo>(AccountInfo => AccountInfo.Account == account);
            if (accountInfos.Count == 0)
            {
                AccountInfo accountInfo = await UserFactory.EditRegisterCreatUser(account);
                accountInfos.Add(accountInfo);
            }
            return accountInfos[0];
        }

        // 微信登陆
        public static async Task<AccountInfo> WeChatLogin(this UserComponent self, string accessTokenAndOpenid)
        {
            var weChatJsonData = WeChatJsonAnalysis.HttpGetUserInfoJson(accessTokenAndOpenid);
            if (weChatJsonData == null)
            {
                return null;
            }

            var accountInfos = await self.dbProxyComponent.Query<AccountInfo>(AccountInfo => AccountInfo.Account == weChatJsonData.unionid);
            if (accountInfos.Count == 0)
            {
                var accountInfo = await UserFactory.WeChatRegisterCreatUser(weChatJsonData);
                accountInfos.Add(accountInfo);
            }
            accountInfos[0].Password = TimeTool.GetCurrenTimeStamp().ToString();
            await self.dbProxyComponent.Save(accountInfos[0]);
            return accountInfos[0];
        }

        // 凭证登陆 就是 userId'|'密码
        public static async Task<AccountInfo> VoucherLogin(this UserComponent self, string userIdAndPassword)
        {
            string[] userIdPassword = userIdAndPassword.Split('|');
            if (userIdPassword.Length != 2)
            {
                return null;
            }

            try
            {
                long queryUserId = long.Parse(userIdPassword[0]);
                var accountInfos = await self.dbProxyComponent.Query<AccountInfo>(AccountInfo => AccountInfo.UserId == queryUserId && AccountInfo.Password == userIdPassword[1]);
                if (accountInfos.Count > 0)
                {
                    return accountInfos[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return null;
        }
    }
}
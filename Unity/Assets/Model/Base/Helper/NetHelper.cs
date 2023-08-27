using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace ETModel
{
    public static class NetHelper
    {
        // 获取本地的IP地址
        public static string[] GetLocalIps()
        {
            var localIps = new List<string>();

            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in adapters)
            {
                if (adapter.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                {
                    continue;
                }

                foreach (var uni in adapter.GetIPProperties().UnicastAddresses)
                {
                    // 得到IPv4的地址, AddressFamily.InterNetwork指的是IPv4
                    // if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIps.Add(uni.Address.ToString());
                    }
                }
            }
            

            return localIps.ToArray();
        }
    }
}
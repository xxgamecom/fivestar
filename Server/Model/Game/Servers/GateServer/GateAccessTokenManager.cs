using System.Collections.Generic;

namespace ETModel
{
    public class GateAccessTokenManager: Component
    {
        private readonly Dictionary<long, long> accessTokenDict = new Dictionary<long, long>();

        public void Add(long accessToken, long userId)
        {
            accessTokenDict[accessToken] = userId;
            TimeoutRemoveKey(accessToken);
        }

        public long Get(long accessToken)
        {
            accessTokenDict.TryGetValue(accessToken, out var userId);
            return userId;
        }

        public void Remove(long accessToken)
        {
            accessTokenDict.Remove(accessToken);
        }

        private async void TimeoutRemoveKey(long accessToken)
        {
            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(20000);
            accessTokenDict.Remove(accessToken);
        }
    }
}
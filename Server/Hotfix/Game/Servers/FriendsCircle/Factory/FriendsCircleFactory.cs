using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public class FriendsCircleFactory
    {
        public static FriendsCircle Create(string name, long createUserId, RepeatedField<int> wanFaCofigs, string announcement)
        {
            var friendsCircle = ComponentFactory.Create<FriendsCircle>();
            friendsCircle.FriendsCircleId = Game.Scene.GetComponent<FriendsCircleComponent>().GetNewFriendsCircleId();
            friendsCircle.Name = name;
            friendsCircle.CreateUserId = createUserId;
            friendsCircle.TotalNumber = 0; //总人数0
            friendsCircle.DefaultWanFaCofigs = wanFaCofigs;
            friendsCircle.ManageUserIds.Add(createUserId);
            friendsCircle.Announcement = announcement;
            friendsCircle.IsRecommend = false;
            return friendsCircle;
        }
    }
}
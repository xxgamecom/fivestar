using ETHotfix;

namespace ETModel
{
    /// <summary>
    /// Session上关联的用户
    /// </summary>
    public class SessionUserComponent: Component
    {
        // Session所对应的User
        public User User { get; set; }

        public long UserId => User.UserId;

        public long GamerSessionActorId { get; set; }
    }

}
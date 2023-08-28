using System;

namespace ETModel
{
    /// <summary>
    /// 游戏类型标签, 可以根据游戏类型(GameEntryAttribute)来获取不同的通道(AGameEntry)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GameEntryAttribute: BaseAttribute
    {
        public long Type { get; protected set; }

        public GameEntryAttribute(long type)
        {
            this.Type = type;
        }
    }
}
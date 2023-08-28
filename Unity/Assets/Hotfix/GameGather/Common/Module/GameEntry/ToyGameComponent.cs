using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class ToyGameComponentAwakeSystem: AwakeSystem<ToyGameComponent>
    {
        public override void Awake(ToyGameComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 游戏类型管理组件
    /// </summary>
    public class ToyGameComponent: Component
    {
        public long CurrToyGame = ToyGameId.None;

        // 游戏通道字典, { key: ToyGameId, value: 游戏通道 }
        private readonly Dictionary<long, AGameEntry> _gameEntryDict = new Dictionary<long, AGameEntry>();

        public void Awake()
        {
            _gameEntryDict.Clear();
            
            var types = Game.EventSystem.GetTypes();
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(GameEntryAttribute), false);

                if (attrs.Length == 0)
                {
                    continue;
                }

                //
                var toyGameAttribute = attrs[0] as GameEntryAttribute;
                var gameEntry = Activator.CreateInstance(type) as AGameEntry;
                gameEntry.Awake(toyGameAttribute.Type);
                _gameEntryDict.Add(toyGameAttribute.Type, gameEntry);
            }
        }

        public void StartGame(long gameType, params object[] objs)
        {
            if (_gameEntryDict.ContainsKey(gameType))
            {
                if (CurrToyGame != ToyGameId.None)
                {
                    _gameEntryDict[CurrToyGame].EndAndStartOtherGame();
                }
                _gameEntryDict[gameType].StartGame(objs);
            }
            else
            {
                Log.Error("想要进入的游戏不存在:" + gameType);
            }
        }

        public void EndGame()
        {
            if (_gameEntryDict.ContainsKey(CurrToyGame))
            {
                _gameEntryDict[CurrToyGame].EndGame();
            }
            else
            {
                Log.Error("系统错误,目前状态游戏不存在:" + CurrToyGame);
            }
        }

    }
}
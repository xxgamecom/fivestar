﻿using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameEntryComponentAwakeSystem: AwakeSystem<GameEntryComponent>
    {
        public override void Awake(GameEntryComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 游戏类型管理组件
    /// </summary>
    public class GameEntryComponent: Component
    {
        public long CurrentGameId = GameEntryId.None;

        // 游戏通道字典, { key: GameEntryId, value: 游戏通道 }
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
                if (CurrentGameId != GameEntryId.None)
                {
                    _gameEntryDict[CurrentGameId].EndAndStartOtherGame();
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
            if (_gameEntryDict.ContainsKey(CurrentGameId))
            {
                _gameEntryDict[CurrentGameId].EndGame();
            }
            else
            {
                Log.Error("系统错误,目前状态游戏不存在:" + CurrentGameId);
            }
        }

    }
}
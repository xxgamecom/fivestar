using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class AIAndCollocationSystem
    {
        // 设置托管的状态
        public static void SetCollocation(this FiveStarPlayer self, bool isCollocation)
        {
            if (self.IsCollocation == isCollocation)
            {
                return; //状态本来相同 就不做后续事件了
            }
            if (self.FiveStarRoom == null)
            {
                return;
            }
            // 有超时才能 进入托管
            if (!self.FiveStarRoom.RoomConfig.IsHaveOverTime)
            {
                return;
            }
            
            if (self.FiveStarRoom.CurrRoomStateType == RoomStateType.ReadyIn)
            {
                isCollocation = false; //准备状态下 只能是 不托管状态
            }
            self.IsCollocation = isCollocation;
            if (self.IsCollocation)
            {
                self.CollocationAIOperate();
            }
            self.SendMessageUser(new Actor_FiveStar_CollocationChange()
            {
                IsCollocation = self.IsCollocation
            });
        }

        //托管和AI状态下出牌 判断出牌
        public static void AICollcationPlayCard(this FiveStarPlayer self, int playCard)
        {
            if (self.IsLiangDao)
            {
                self.PlayCard(self.MoEndHand); //如果亮倒 只能出最后摸的牌
                return;
            }
            if (!self.Hands.Contains(playCard) || self.FiveStarRoom.LiangDaoCanHuCards.Contains(playCard))
            {
                for (int i = 0; i < self.Hands.Count; i++)
                {
                    if (!self.FiveStarRoom.LiangDaoCanHuCards.Contains(self.Hands[i]))
                    {
                        self.PlayCard(self.Hands[i]); //如果手牌中没有 最后摸的牌 或者摸的牌是放炮的牌 就出第一张手牌
                        return;
                    }
                }
                self.PlayCard(playCard);
                Log.Error("AI托管 手中的牌全都是放炮的牌");
            }
            else
            {
                self.PlayCard(playCard);
            }
        }

        // 托管的默认操作
        public static void CollocationAIOperate(this FiveStarPlayer self)
        {
            if (self.FiveStarRoom.CurrRoomStateType == RoomStateType.GameIn)
            {
                self.boolData = true;
                if (self.FiveStarRoom.IsDaPiaoBeing && (!self.IsAlreadyDaPiao)) //如果在打漂中 并且自己没有打漂就打漂
                {
                    if (self.IsAI) //如果是AI就随便漂
                    {
                        self.DaPiao(RandomTool.Random(0, self.FiveStarRoom.RoomConfig.MaxPiaoNum + 1)); //随机打漂
                    }
                    else
                    {
                        self.DaPiao(0); //默认是不漂
                    }
                }
                else if (self.IsCanPlayCard) //如果可以出牌 直接出 最后摸到的牌
                {
                    if (self.IsAI) //如果是AI出牌 保留手牌中有多张相同的
                    {
                        self.Hands.Sort();                                 //手牌排序
                        self.intData = self.Hands.IndexOf(self.MoEndHand); //获取摸到的牌 首次出现的位置

                        if (self.intData < self.Hands.Count - 2 && self.Hands[self.intData + 1] == self.MoEndHand)
                        {
                            self.AICollcationPlayCard(self.Hands[self.Hands.Count - 1]); //摸到的牌有相同的 出牌组里最后一张牌
                        }
                        else
                        {
                            self.AICollcationPlayCard(self.MoEndHand); //没有相同的出摸到的牌 
                        }
                    }
                    else
                    {
                        self.AICollcationPlayCard(self.MoEndHand); //不是AI出牌直接出 最后摸到的牌 
                    }
                }
                else if (self.FiveStarRoom.CanOperatePlayerIndex.Contains(self.SeatIndex)) //如果玩家可操作索引列表里面有自己 则直接操作
                {
                    FiveStarOperateInfo fiveStarOperateInfo;
                    if (self.canOperateLists.Contains(FiveStarOperateType.FangChongHu)) //如果可以胡 就胡
                    {
                        fiveStarOperateInfo = FiveStarOperateInfoFactory.Create(0, FiveStarOperateType.FangChongHu, 0);
                    }
                    else
                    {
                        fiveStarOperateInfo = FiveStarOperateInfoFactory.Create(0, FiveStarOperateType.None, 0); //不能胡就放弃
                    }
                    if (self.IsAI) //如果是AI 就是能碰就碰 能杠就杠 因为发的牌会做特殊手脚
                    {
                        if (self.canOperateLists.Contains(FiveStarOperateType.Peng)) //如果可以胡 就胡
                        {
                            fiveStarOperateInfo.OperateType = FiveStarOperateType.Peng;
                        }
                        else if (self.canOperateLists.Contains(FiveStarOperateType.MingGang))
                        {
                            //杠牌 不仅需要传递 操作类型 还是传杠那张牌 之前记录亮了
                            foreach (var canGang in self.canGangCards)
                            {
                                fiveStarOperateInfo.Card = canGang.Key;
                                fiveStarOperateInfo.OperateType = canGang.Value;
                            }
                        }
                    }
                    self.OperatePengGangHu(fiveStarOperateInfo); //执行操作
                    //不能销毁 self 因为多人操作会保留一段时间
                }
                else
                {
                    self.boolData = false;
                }
                if (self.boolData)
                {
                    self.SetCollocation(true); //如果进行了 托管操作 就进入托管
                }
            }
        }

        public const int LiangDaoMoCount = 7; //在摸第几张牌的时候 亮倒

        public const int HuMoCount = 10; //在摸第几张牌的时候胡牌

        //玩家摸牌处理 摸牌之前调用
        public static int AIMoPaiDispose(this FiveStarPlayer self, int card)
        {
            if (!self.IsAI)
            {
                return card;
            }

            self.MoCardCount++;
            if (self.MoCardCount == HuMoCount)
            {
                int wincard = self.FiveStarRoom.ResidueCards[self.FiveStarRoom.ResidueCards.Count - 1]; //获取必赢牌的 最后摸的牌
                self.FiveStarRoom.ResidueCards.Remove(wincard);
                self.FiveStarRoom.ResidueCards.Add(card);
                return wincard;
            }
            return card;
        }

        //玩家出牌 出完牌后调用
        public static void AIPlayCardDispose(this FiveStarPlayer self)
        {
            if (!self.IsAI)
            {
                return;
            }

            if (!self.IsLiangDao && self.MoCardCount == LiangDaoMoCount)
            {
                //替换手牌

                List<int> newHands = self.FiveStarRoom.ResidueCards.GetRange(self.FiveStarRoom.ResidueCards.Count - 1 - self.Hands.Count, self.Hands.Count); //获取隐藏在 剩余牌尾部必赢的牌
                int wincard = self.FiveStarRoom.ResidueCards[self.FiveStarRoom.ResidueCards.Count - 1];                                                      //获取必赢牌的 最后摸的牌

                self.FiveStarRoom.ResidueCards.Remove(wincard); //删除必赢摸的牌
                for (int i = 0; i < newHands.Count; i++)
                {
                    self.FiveStarRoom.ResidueCards.Remove(newHands[i]); //删除必赢的牌
                }
                self.FiveStarRoom.ResidueCards.AddRange(self.Hands); //把现有的手牌添加到剩余牌数组里面
                self.FiveStarRoom.ResidueCards.Add(wincard);         //添加必赢摸的牌 到最后

                self.Hands.Clear();       //清除当前手牌
                self.Hands.Add(newHands); //添加必赢的牌
                self.LiangDao();          //正常情况下决定可以亮倒
            }
        }

        public static List<int> GetRange(this RepeatedField<int> repFieldInt, int index, int count)
        {
            List<int> array = new List<int>();
            for (int i = index; i < index + count; i++)
            {
                array.Add(repFieldInt[i]);
            }

            return array;
        }

        // 延迟打漂AI打漂
        public static async void AIDelayDaPiao(this FiveStarPlayer self)
        {
            if (!self.IsAI) //不是AI直接 返回
            {
                return;
            }

            await Game.Scene.GetComponent<TimerComponent>().WaitAsync(RandomTool.Random(1, 4) * 1000);
            self.CollocationAIOperate();
        }
    }
}
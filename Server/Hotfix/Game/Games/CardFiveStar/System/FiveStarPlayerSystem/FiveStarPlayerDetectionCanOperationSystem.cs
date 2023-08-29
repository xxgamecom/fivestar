using ETModel;

namespace ETHotfix
{
    public static class FiveStarPlayerDetectionCanOperationSystem
    {

        //检测能不能进行操作 
        public static bool IsCanOperate(this FiveStarPlayer self, int playCard = 0, int playCardIndex = 0)
        {
            if (self.IsRestIn) //如果是在休息中 直接不能操作
            {
                return false;
            }

            self.canOperateLists.Clear();              //可操作列表清空
            self.canGangCards.Clear();                 //可杠列表清空
            if (self.IsCanHu(playCard, playCardIndex)) //检测能不能胡牌
            {
                self.canOperateLists.Add(FiveStarOperateType.FangChongHu);
            }
            if (playCard > 0)
            {
                //别人打牌的时候
                self.intData = self.IsCanPengAndGang(playCard);
                if (self.intData != 0) //检测能不能碰和暗杆
                {
                    if (self.IsLiangDao && self.intData == FiveStarOperateType.MingGang && self.LiangDaoNoneCards.Contains(playCard))
                    {
                        self.AddCanGangOpearte();
                    }
                    else if (self.intData == FiveStarOperateType.Peng)
                    {
                        self.canOperateLists.Add(FiveStarOperateType.Peng);
                    }
                    else if (self.intData == FiveStarOperateType.MingGang)
                    {
                        self.canOperateLists.Add(FiveStarOperateType.Peng);
                        self.AddCanGangOpearte();
                    }
                }
            }
            else
            {
                //自己摸牌的时候
                if (self.IsCanCaGang() || self.IsCanAnGang())
                {
                    self.AddCanGangOpearte();
                }
            }
            //广播可操作消息
            return self.canOperateLists.Count > 0;
        }
        //添加可以杠的操作
        public static void AddCanGangOpearte(this FiveStarPlayer self)
        {
            if (self.FiveStarRoom.ResidueCards.Count > 0) //摸的最后一张牌不能杠所以 不显示
            {
                self.canOperateLists.Add(FiveStarOperateType.MingGang);
            }
        }
        //添加可以杠的牌
        public static void AddCanGangCard(this FiveStarPlayer self, int cardSize, int gangType)
        {
            self.canGangCards[cardSize] = gangType;
        }

        //检测能不能暗杆
        public static bool IsCanAnGang(this FiveStarPlayer self)
        {
            self.intData = 0;
            for (int i = 0; i < self.Hands.Count - 1; i++)
            {
                if (self.Hands[i] == self.Hands[i + 1])
                {
                    self.intData++;
                }
                else
                {
                    if (self.intData >= 3) //3次相同 就表示有4张一样的
                    {
                        self.AddCanAnGangCard(self.Hands[i]);
                    }
                    self.intData = 0;
                }
            }
            if (self.intData >= 3) //3次相同 就表示有4张一样的
            {
                self.AddCanAnGangCard(self.Hands[self.Hands.Count - 1]);
            }
            return self.canGangCards.Count > 0;
        }
        //添加可以暗杠的牌
        public static void AddCanAnGangCard(this FiveStarPlayer self, int card)
        {
            if (self.IsLiangDao)
            {
                if (!self.LiangDaoNoneCards.Contains(card) || self.MoEndHand != card)
                {
                    return; //如果玩家 亮倒了  而且 亮倒无关牌中 没有这张牌 他就不能暗杠这张牌 而且 只能暗杠杠摸的那种牌
                }
            }
            self.AddCanGangCard(card, FiveStarOperateType.AnGang);
        }
        //检测能不能碰或者杠
        public static int IsCanPengAndGang(this FiveStarPlayer self, int card)
        {
            self.intData = 0;
            for (int i = 0; i < self.Hands.Count; i++)
            {
                if (self.Hands[i] == card)
                {
                    self.intData++;
                }
            }
            if (self.intData == 2)
            {
                if (self.IsLiangDao)
                {
                    return FiveStarOperateType.None;
                }
                return FiveStarOperateType.Peng;
            }
            else if (self.intData == 3)
            {
                if (self.IsLiangDao && !self.LiangDaoNoneCards.Contains(card))
                {
                    return FiveStarOperateType.None;
                }
                self.AddCanGangCard(card, FiveStarOperateType.MingGang);
                return FiveStarOperateType.MingGang;
            }
            return FiveStarOperateType.None;
        }

        //检测能不能擦杠
        public static bool IsCanCaGang(this FiveStarPlayer self)
        {
            for (int i = 0; i < self.OperateInfos.Count; i++)
            {
                if (self.OperateInfos[i].OperateType == FiveStarOperateType.Peng)
                {
                    if (self.Hands.Contains(self.OperateInfos[i].Card))
                    {
                        //如果亮倒了 擦杠的牌 只能是刚才自己摸的牌
                        if (self.IsLiangDao)
                        {
                            if (self.MoEndHand == self.OperateInfos[i].Card)
                            {
                                self.AddCanGangCard(self.OperateInfos[i].Card, FiveStarOperateType.CaGang);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        self.AddCanGangCard(self.OperateInfos[i].Card, FiveStarOperateType.CaGang);
                        return true;
                    }
                }
            }
            return false;
        }

        //检测能不能胡
        public static bool IsCanHu(this FiveStarPlayer self, int card = 0, int playCardIndex = 0)
        {
            if (card > 0)
            {
                self.Hands.Add(card);
            }
            self.boolData = CardFiveStarHuPaiLogic.IsHuPai(self.Hands);
            //不是自摸要判断 是否是平胡
            if (self.boolData && card > 0)
            {
                //如果有一家亮倒 可以胡
                if (self.IsLiangDao || self.FiveStarRoom.FiveStarPlayerDic[playCardIndex].IsLiangDao)
                {
                }
                //如果没有亮倒 并且胡牌的倍数小于 最小放冲胡的倍数就不能胡
                else if (CardFiveStarHuPaiLogic.GetMultiple(self.Hands, self.PengGangs, card, self.FiveStarRoom.IsGangShangCard) < FiveStarRoom.FaangChongHuMinHuCardMultiple)
                {
                    self.boolData = false;
                }
            }
            if (card > 0)
            {
                self.Hands.Remove(card);
            }
            return self.boolData;
        }
        //检测能不能听牌
        public static bool IsTingCard(this FiveStarPlayer self)
        {
            return CardFiveStarHuPaiLogic.IsCanTingPai(self.Hands);
        }

        //移除指定牌数量 如果不够移除 会还原被移除的牌
        public static bool RemoveCardCount(this FiveStarPlayer self, int card, int count)
        {
            self.boolData = true;
            for (int i = 0; i < count; i++)
            {
                if (self.Hands.Contains(card))
                {
                    self.Hands.Remove(card);
                }
                else
                {
                    self.boolData = false;
                    for (int j = 0; j < i; j++)
                    {
                        self.Hands.Add(card);
                    }
                }
            }
            return self.boolData;
        }
    }
}
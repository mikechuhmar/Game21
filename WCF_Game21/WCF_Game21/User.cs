using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static WCF_Game21.Card;

namespace WCF_Game21
{
    class User
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool IsPrepare = false;
        public bool InGame = false;
        public List<Card> cards = new List<Card>();
        public OperationContext operationContext { get; set; }
        public int Coins = 200;
        public int amountPoints()
        {
            int amount = 0;
            if (cards.Count == 0)
                return amount;
            else
            {
                foreach(Card c in cards)
                {
                    switch (c.dignity)
                    {
                        case Dignity.Two:
                        case Dignity.Jack:
                            amount += 2;
                            break;
                        case Dignity.Three:
                        case Dignity.Queen:
                            amount += 3;
                            break;
                        case Dignity.Four:
                        case Dignity.King:
                            amount += 4;
                            break;
                        case Dignity.Five:
                            amount += 5;
                            break;
                        case Dignity.Six:
                            amount += 6;
                            break;
                        case Dignity.Seven:
                            amount += 7;
                            break;
                        case Dignity.Eight:
                            amount += 8;
                            break;
                        case Dignity.Nine:
                            amount += 9;
                            break;
                        case Dignity.Ten:
                        case Dignity.Ace:
                            amount += 10;
                            break;
                    }

                }
            }
            return amount;
        }
    }
}

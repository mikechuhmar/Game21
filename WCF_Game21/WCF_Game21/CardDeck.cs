using System;
using System.Collections.Generic;
using System.Linq;
using static WCF_Game21.Card;

namespace WCF_Game21
{
    //Класс колоды карт
    class CardDeck
    {
        public List<Card> Cards = new List<Card>();
        public Card this[int i]
        {
            get { return Cards[i]; }
            set { Cards[i] = value; }
        }
        int Count { get { return Cards.Count; } }
        public CardDeck(int Count)
        {

            for (int j = 12; j >= 0; j--)
            {
                for (int k = 0; k < 4; k++)
                {
                    Cards.Add(new Card((Suit)k, (Dignity)j));

                }
                if (this.Count >= Count)
                    break;
            }
            Cards = Cards.OrderBy(x => x.dignity).ToList();
        }
        public CardDeck()
        {

        }
        public void PrintToConsole()
        {
            int i = 1;

            foreach (Card card in Cards)
            {
                i = Cards.IndexOf(card);
                Console.WriteLine("Card {0}: {1}, {2}", i + 1, card.dignity, card.suit);
            }
        }
        public void Mix()
        {
            CardDeck cardDeck = new CardDeck();
            Random random = new Random();
            for (int i = 0; i < Count; i++)
            {
                int j = random.Next(i, Count - 1);
                Card card = this[j];
                this[j] = this[i];
                this[i] = card;
            }
        }
    }
}

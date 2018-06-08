

namespace WCF_Game21
{
    public class Card
    {
        //Класс карты
        public enum Suit { Clubs, Siamonds, Hearts, Spades };
        public enum Dignity { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };
        public Suit suit;
        public Dignity dignity;
        public Card(Suit suit, Dignity dignity)
        {
            this.suit = suit;
            this.dignity = dignity;
        }
    }
}

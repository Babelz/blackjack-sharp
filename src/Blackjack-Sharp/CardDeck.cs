using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents standard 52-card French deck.
    /// </summary>
    public sealed class CardDeck 
    {
        #region Fields
        private readonly List<Card> cards;
        #endregion

        #region Properties
        public bool Empty => cards.Count == 0;
        #endregion

        /// <summary>
        /// Creates new instance of <see cref="CardDeck"/> and optionally
        /// shuffles it immediately.
        /// </summary>
        /// <param name="shuffle">boolean declaring whether the deck should be shuffled immediately</param>
        public CardDeck(bool shuffle = true)
        {
            // First, create cards in order.
            cards = new List<Card>();

            foreach (var suit in Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>())
            {
                foreach (var face in Enum.GetValues(typeof(CardFace)).Cast<CardFace>())
                    cards.Add(new Card(face, suit));
            }
            
            if (shuffle) 
                Shuffle();
        }

        /// <summary>
        /// Shuffles the cards in the deck.
        /// </summary>
        public void Shuffle()
        {
            // Shuffle deck using Fisher-Yates.
            var random = new Random();

            for (var i = 0; i < cards.Count; i++)
            {
                var index = random.Next(0, cards.Count - 1);

                // Shuffle deck my swapping cards at indices.
                (cards[i], cards[index]) = (cards[index], cards[i]);
            }
        }

        /// <summary>
        /// Removes the top most card from the deck and returns it to the caller.
        /// </summary>
        public Card Take()
        {
            // Take top most card, give ownership to caller.
            var card = cards[0];

            cards.RemoveAt(0);

            return card;
        }

        /// <summary>
        /// Joins this deck with given deck removing all cards from
        /// the other deck and placing them to this deck.
        /// </summary>
        public void Join(CardDeck other)
        {
            cards.AddRange(other.cards);

            other.cards.Clear();
        }
    }
}

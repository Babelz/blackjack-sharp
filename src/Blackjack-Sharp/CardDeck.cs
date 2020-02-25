using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents standard 52-card French deck.
    /// </summary>
    public sealed class CardDeck 
    {
        #region Static fields
        /// <summary>
        /// Static random for shuffling decks.
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// Object for synchronizing access to <see cref="random"/>.
        /// </summary>
        private static readonly object randomLock = new object();
        #endregion

        #region Fields
        private readonly List<Card> cards;
        #endregion

        #region Properties
        public bool Empty => cards.Count == 0;
        #endregion

        private CardDeck(bool shuffle)
        {
            // First, create cards in order.
            cards = new List<Card>();

            foreach (var suit in Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>())
            {
                foreach (var face in Enum.GetValues(typeof(CardFace)).Cast<CardFace>())
                    cards.Add(new Card(face, suit));
            }
            
            if (!shuffle) return;

            // Shuffle deck using Fisher-Yates.
            lock (randomLock)
            {
                for (var i = 0; i < cards.Count; i++)
                {
                    var index = random.Next(0, cards.Count - 1);

                    // Shuffle deck my swapping cards at indices.
                    (cards[i], cards[index]) = (cards[index], cards[i]);
                }
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
        /// Returns given card to the deck. Multiple cards with same suit and face
        /// can exist in single deck.
        /// </summary>
        public void Return(Card card)
            => cards.Add(card ?? throw new ArgumentNullException(nameof(card)));

        /// <summary>
        /// Creates new instance of <see cref="CardDeck"/> where
        /// all cards are shuffled.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CardDeck CreateShuffled()
            => new CardDeck(true);

        /// <summary>
        /// Creates new instance of <see cref="CardDeck"/> where
        /// all cards are order.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CardDeck CreateOrdered()
            => new CardDeck(false);
    }
}

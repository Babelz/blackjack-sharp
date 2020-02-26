using System;
using System.Collections;
using System.Collections.Generic;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents hand containing cards.
    /// </summary>
    public sealed class Hand : IEnumerable<Card>
    {
        #region Fields
        private readonly List<Card> cards;
        #endregion

        public Hand()
            => cards = new List<Card>();

        public void Add(Card card)
            => cards.Add(card ?? throw new ArgumentNullException(nameof(card)));

        /// <summary>
        /// Splits the current hand.
        /// </summary>
        public Hand Split()
        {
            // Create new hand and add the split card to it.
            var second = new Hand
            {
                cards[0]
            };

            // Remove split card from this hand.
            cards.RemoveAt(0);

            return second;
        }

        /// <summary>
        /// Clears the hand removing all cards from it.
        /// </summary>
        public void Clear()
            => cards.Clear();

        public IEnumerator<Card> GetEnumerator()
            => cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => cards.GetEnumerator();
    }
}

using System;
using System.Collections.Generic;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents players hand in standard blackjack.
    /// </summary>
    public sealed class PlayerHand
    {
        #region Fields
        private readonly List<Card> cards;
        #endregion

        #region Properties
        /// <summary>
        /// Returns boolean declaring whether the hand can be split.
        /// </summary>
        public bool CanSplit
            => cards.Count == 2 && cards[0].Face == cards[1].Face;
        #endregion

        public PlayerHand()
            => cards = new List<Card>();

        public void Add(Card card)
            => cards.Add(card ?? throw new ArgumentNullException(nameof(card)));

        /// <summary>
        /// Splits the current hand.
        /// </summary>
        public PlayerHand Split()
        {
            // Create new hand and add the split card to it.
            var second = new PlayerHand();

            second.Add(cards[0]);

            // Remove split card from this hand.
            cards.RemoveAt(0);

            return second;
        }
    }
}

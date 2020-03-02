using System;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents bet information of single hand.
    /// </summary>
    public sealed class PlayerBet
    {
        #region Properties
        /// <summary>
        /// Gets the hand this bet has been placed to.
        /// </summary>
        public Hand Hand
        {
            get;
        }

        /// <summary>
        /// Gets the bet amount in whole euros.
        /// </summary>
        public uint Amount
        {
            get;
        }
        #endregion

        public PlayerBet(Hand hand, uint amount)
        {
            Hand   = hand ?? throw new ArgumentNullException(nameof(hand));
            Amount = amount;
        }
    }
}

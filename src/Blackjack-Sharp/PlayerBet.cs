using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents bet information of players single hand.
    /// </summary>
    public sealed class PlayerBet
    {
        #region Properties
        /// <summary>
        /// Gets the player that owns this bet.
        /// </summary>
        public Player Player
        {
            get;
        }

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

        public PlayerBet(Player player, Hand hand, uint amount)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Hand   = hand ?? throw new ArgumentNullException(nameof(hand));
            Amount = amount;
        }
    }
}

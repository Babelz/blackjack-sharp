using System;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents a blackjack player defined by the name
    /// of the player, hands and wallet.
    /// </summary>
    public sealed class Player
    {
        #region Properties
        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Gets the primary (first) hand of the player.
        /// </summary>
        public Hand PrimaryHand
        {
            get;
        }

        /// <summary>
        /// Gets the second hand of the player. This value will stay
        /// null until the primary hand is split.
        /// </summary>
        public Hand SecondaryHand
        {
            get;
            private set;
        }

        public Wallet Balance
        {
            get;
        }

        public bool IsSplit
            => SecondaryHand != null;
        #endregion

        /// <summary>
        /// Creates new instance of <see cref="Player"/> with given name and 
        /// initial amount of in game cash (balance).
        /// </summary>
        public Player(string name, uint initialBalance)
        {
            Name        = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            PrimaryHand = new Hand();
            Balance     = new Wallet(initialBalance);
        }

        public void Clear()
        {
            PrimaryHand.Clear();

            SecondaryHand = null;
        }
    }
}

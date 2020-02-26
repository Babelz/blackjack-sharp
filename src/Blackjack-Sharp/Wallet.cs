namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that handles money related operations of players.
    /// </summary>
    public sealed class Wallet
    {
        #region Properties
        /// <summary>
        /// Represents whole euros where 1 is 100 cents.
        /// </summary>
        public uint Balance
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns boolean declaring whether the wallet is empty.
        /// </summary>
        public bool Empty
            => Balance == 0;
        #endregion

        public Wallet(uint initialBalance)
            => Balance = initialBalance;
        
        public void Put(uint amount)
            => Balance += amount;

        public bool TryTake(uint amount)
        {
            if (amount > Balance) return false;

            Balance -= amount;

            return true;
        }
    }
}

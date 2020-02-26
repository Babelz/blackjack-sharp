namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents a blackjack player defined by the name
    /// of the player, hands and wallet.
    /// </summary>
    public sealed class Player
    {
        #region Properties
        public string Name
        {
            get;
        }

        public Hand PrimaryHand
        {
            get;
        }

        public Hand SecondaryHand
        {
            get;
            private set;
        }

        public PlayerWallet Balance
        {
            get;
        }

        public bool IsSplit
            => SecondaryHand != null;
        #endregion

        public Player()
        {
        }

        public void Clear()
        {
            PrimaryHand.Clear();

            SecondaryHand = null;
        }
    }
}

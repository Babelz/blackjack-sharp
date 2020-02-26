namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents a blackjack player defined by the name
    /// of the player, hands and wallet.
    /// </summary>
    public sealed class BlackjackPlayer
    {
        #region Properties
        public string Name
        {
            get;
        }

        public PlayerHand Primary
        {
            get;
        }

        public PlayerHand Secondary
        {
            get;
            private set;
        }

        public PlayerWallet Balance
        {
            get;
        }
        #endregion
    }
}

namespace Blackjack_Sharp
{
    /// <summary>
    /// Interface for implementing classes that represent dealers in a card games.
    /// </summary>
    public interface IDealer
    {
        #region Properties
        /// <summary>
        /// Gets the hand of the dealer.
        /// </summary>
        Hand Hand
        {
            get;
        }
        #endregion

        /// <summary>
        /// Deals single card to given hand and returns the card to the caller,
        /// always assumed to return a card.
        /// </summary>
        Card Deal(Hand hand);

        /// <summary>
        /// Deals card to the dealer from dealers deck and returns
        /// the card to the caller.
        /// </summary>
        Card DealSelf();
    }
}
namespace Blackjack_Sharp
{
    /// <summary>
    /// Enumeration containing all blackjack related player operations.
    /// </summary>
    public enum PlayerOperations : byte
    {
        /// <summary>
        /// Stay with current hand, no further operations are to be made.
        /// </summary>
        Stay,

        /// <summary>
        /// Hit current card with another card, can result in blackjack or bust.
        /// </summary>
        Hit,

        /// <summary>
        /// Hit the current hand with one more card and double the bet on it.
        /// </summary>
        Double,

        /// <summary>
        /// Splits the current hand and place same amount of bet to the newly
        /// created hand.
        /// </summary>
        Split
    }
}

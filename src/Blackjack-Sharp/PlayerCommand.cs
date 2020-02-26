namespace Blackjack_Sharp
{
    /// <summary>
    /// Enumeration containing all blackjack related player operations.
    /// </summary>
    public enum PlayerCommand : byte
    {
        /// <summary>
        /// Stay with current hand, no further operations are to be made.
        /// </summary>
        Stay = 0,

        /// <summary>
        /// Hit current card with another card, can result in blackjack or bust.
        /// </summary>
        Hit = 1,

        /// <summary>
        /// Hit the current hand with one more card and double the bet on it.
        /// </summary>
        Double = 2,

        /// <summary>
        /// Splits the current hand and place same amount of bet to the newly
        /// created hand.
        /// </summary>
        Split = 3,

        /// <summary>
        /// Player is leaving the table.
        /// </summary>
        Leave = 4,

        /// <summary>
        /// Player is skipping current round.
        /// </summary>
        Skip = 5
    }
}

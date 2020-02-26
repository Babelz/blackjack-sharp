namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents dealer in blackjack.
    /// </summary>
    public sealed class BlackjackDealer
    {
        #region Constant fields
        /// <summary>
        /// How many additional decks are in play.
        /// </summary>
        public const uint AdditionalDecksCount = 7;
        #endregion

        #region Fields
        private readonly uint additionalDecksCount;
        
        private CardDeck deck;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the hand of the dealer.
        /// </summary>
        public Hand Hand
        {
            get;
        }
        #endregion

        /// <summary>
        /// Creates new instance of <see cref="BlackjackDealer"/> with given
        /// count of additional decks. Dealer is always guaranteed to have at least
        /// one deck in play.
        /// </summary>
        public BlackjackDealer(uint additionalDecksCount = AdditionalDecksCount)
        {
            this.additionalDecksCount = additionalDecksCount;

            PopulateDeck();
        }

        private void PopulateDeck()
        {
            // Create root deck, guarantee that there is always 
            // one deck in use.
            deck = new CardDeck(shuffle: true);

            if (additionalDecksCount == 0) return;

            // Create additional decks.
            for (var i = 0; i < additionalDecksCount - 1; i++)
            {
                deck.Join(new CardDeck(shuffle: true));

                // Shuffle for each deck.
                deck.Shuffle();
            }
        }

        public Card Take()
        {
            if (deck.Empty) PopulateDeck();

            return deck.Take();
        }
    }
}

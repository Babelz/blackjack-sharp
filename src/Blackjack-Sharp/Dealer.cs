namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents dealer in a card game.
    /// </summary>
    public sealed class Dealer
    {
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
        /// Creates new instance of <see cref="Dealer"/> with given
        /// count of additional decks. Dealer is always guaranteed to have at least
        /// one deck in play.
        /// </summary>
        public Dealer(uint additionalDecksCount = 0)
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

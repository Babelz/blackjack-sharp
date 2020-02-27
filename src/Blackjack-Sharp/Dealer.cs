using System;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Default implementation of <see cref="IDealer"/>.
    /// </summary>
    public sealed class Dealer : IDealer
    {
        #region Fields
        private readonly uint additionalDecksCount;

        private CardDeck deck;
        #endregion

        #region Properties
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

            Hand = new Hand();

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

        public Card Deal(Hand hand)
        {
            // Repopulate the deck if it is empty.
            if (deck.Empty) PopulateDeck();

            var card = deck.Take();

            hand.Add(card);

            return card;
        }

        public Card DealSelf()
            => Deal(Hand);
    }
}

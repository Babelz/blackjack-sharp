using System;

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

        /// <summary>
        /// Deals single card to given hand and returns the card to the caller,
        /// always assumed to return a card.
        /// </summary>
        public Card Deal(Hand hand)
        {
            // Repopulate the deck if it is empty.
            if (deck.Empty) PopulateDeck();

            var card = deck.Take();

            hand.Add(card);

            return card;
        }

        /// <summary>
        /// Deals card to the dealer from dealers deck and returns
        /// the card to the caller.
        /// </summary>
        public Card DealSelf()
            => Deal(Hand);
    }
}

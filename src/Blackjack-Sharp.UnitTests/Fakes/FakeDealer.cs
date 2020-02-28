using System;
using System.Collections.Generic;

namespace Blackjack_Sharp.UnitTests.Fakes
{
    /// <summary>
    /// Fake dealer class for testing purposes.
    /// </summary>
    public sealed class FakeDealer : IDealer
    {
        #region Fields
        private readonly Queue<Card> dealersDeck;
        private readonly Queue<Card> playersDeck;
        #endregion

        #region Properties
        public Hand Hand
        {
            get;
        }
        #endregion

        /// <summary>
        /// Creates new instance of <see cref="FakeDealer"/> with given card
        /// sequences.
        /// </summary>
        /// <param name="dealersDeck">cards dealt to the dealer</param>
        /// <param name="playersDeck">cards dealt to the player</param>
        public FakeDealer(IEnumerable<Card> dealersDeck, IEnumerable<Card> playersDeck)
        {
            this.dealersDeck = new Queue<Card>(dealersDeck) 
                ?? throw new ArgumentNullException(nameof(dealersDeck));
            
            this.playersDeck = new Queue<Card>(playersDeck) 
                ?? throw new ArgumentNullException(nameof(playersDeck));

            Hand = new Hand();
        }

        public Card Deal(Hand hand)
        {
            var card = playersDeck.Dequeue();

            hand.Add(card);

            return card;
        }

        public Card DealSelf()
        {
            var card = dealersDeck.Dequeue();

            Hand.Add(card);

            return card;
        }
    }
}

using Xunit;
using Blackjack_Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack_Sharp.Tests
{
    [Trait("Category", "Deck")]
    public class CardDeckTests
    {
        [Fact()]
        public void Ctor_Test()
        {
            Assert.Null(Record.Exception(() => new CardDeck(true)));
            Assert.Null(Record.Exception(() => new CardDeck(false)));
        }

        [Fact()]
        public void Shuffle_Test()
        {
            // Arrange.
            var deck = new CardDeck(false);

            // Act.
            
            // Assert.
        }

        [Fact()]
        public void Shuffle_Empty_Test()
        {
            // Arrange.
            var deck = new CardDeck(false);

            // Act.
            while (!deck.Empty) deck.Take();
            
            var exception = Record.Exception(() => deck.Shuffle()));

            // Assert.
            Assert.Null(exception);
        }

        [Fact()]
        public void Take_Empty_Test()
        {
            // Arrange.
            var deck = new CardDeck(false);

            // Act.

            // Assert.
        }

        [Fact()]
        public void Take_Test()
        {
            // Arrange.
            var deck = new CardDeck(false);

            // Act.

            // Assert.
        }

        [Fact()]
        public void Return_Test()
        {
            // Arrange.
            var deck = new CardDeck(false);

            // Act.

            // Assert.
        }

        [Fact()]
        public void Return_Null_Test()
        {
            // Arrange.
            var deck = new CardDeck(false);

            // Act.

            // Assert.
        }
    }
}
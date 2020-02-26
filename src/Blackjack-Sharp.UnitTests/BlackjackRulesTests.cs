﻿using Xunit;
using Blackjack_Sharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack_Sharp.Tests
{
    public class BlackjackRulesTests
    {
        [Fact()]
        public void ShouldStayTest()
        {
            Assert.True(BlackjackRules.ShouldStay(new Card[]
            {
                new Card(CardFace.King, CardSuit.Clubs),
                new Card(CardFace.Seven, CardSuit.Clubs)
            }));

            Assert.False(BlackjackRules.ShouldStay(new Card[]
            {
                new Card(CardFace.King, CardSuit.Clubs),
                new Card(CardFace.Six, CardSuit.Clubs)
            }));
        }

        [Fact()]
        public void CanSplitTest()
        {
            Assert.True(BlackjackRules.CanSplit(new Card[]
            {
                new Card(CardFace.Six, CardSuit.Clubs),
                new Card(CardFace.Six, CardSuit.Hearts)
            }));

            Assert.False(BlackjackRules.CanSplit(new Card[]
            {
                new Card(CardFace.Six, CardSuit.Clubs),
                new Card(CardFace.Five, CardSuit.Clubs)
            }));
        }

        [Fact()]
        public void CanDoubleTest()
        {
            // 9, 10 and 11 should allow splitting.
            Assert.True(BlackjackRules.CanDouble(new Card[]
            {
                new Card(CardFace.Six, CardSuit.Clubs),
                new Card(CardFace.Three, CardSuit.Hearts)
            }));

            Assert.True(BlackjackRules.CanDouble(new Card[]
            {
                new Card(CardFace.Six, CardSuit.Clubs),
                new Card(CardFace.Four, CardSuit.Hearts)
            }));

            Assert.True(BlackjackRules.CanDouble(new Card[]
            {
                new Card(CardFace.Six, CardSuit.Clubs),
                new Card(CardFace.Five, CardSuit.Hearts)
            }));

            // 4 and 12 should not allow splitting.
            Assert.False(BlackjackRules.CanDouble(new Card[]
            {
                new Card(CardFace.Two, CardSuit.Clubs),
                new Card(CardFace.Two, CardSuit.Clubs)
            }));

            Assert.False(BlackjackRules.CanDouble(new Card[]
            {
                new Card(CardFace.Ten, CardSuit.Clubs),
                new Card(CardFace.Two, CardSuit.Clubs)
            }));
        }

        [Fact()]
        public void IsBustedTest()
        {
            // Values above 21 should be seen as bust.
            Assert.True(BlackjackRules.IsBusted(22));
            Assert.True(BlackjackRules.IsBusted(35));

            // Values equal or below 21 should not be seen as bust.
            Assert.False(BlackjackRules.IsBusted(19));
            Assert.False(BlackjackRules.IsBusted(21));
        }

        [Fact()]
        public void IsBlackjackTest()
        {
            Assert.True(BlackjackRules.IsBlackjack(21));
            
            Assert.False(BlackjackRules.IsBlackjack(22));
            Assert.False(BlackjackRules.IsBlackjack(20));
        }

        [Fact()]
        public void ValueOfSingleCardTest()
        {
            for (var actualValue = 0; actualValue < (byte)CardFace.King; actualValue++)
            {
                var face = (CardFace)actualValue;
                
                BlackjackRules.ValueOf(new Card(face, CardSuit.Clubs), out var value, out var soft);

                if (face == CardFace.Ace)
                {
                    // Ace can be 1 or 11.
                    Assert.Equal(11, soft);
                    Assert.Equal(1, value);
                }
                else if (actualValue >= 10)
                {
                    // 10, jack, queen and king are always 10.
                    Assert.Equal(10, value);
                    Assert.Equal(10, soft);
                }
                else
                {
                    // Other cards are valued based on their face.
                    Assert.Equal(actualValue, value);
                    Assert.Equal(actualValue, soft);
                }
            }
        }

        [Fact()]
        public void ValueOfHandTest()
        {
            // Arrange.
            var deckOf18 = new Card[]
            {
                new Card(CardFace.Ace, CardSuit.Clubs),
                new Card(CardFace.Seven, CardSuit.Clubs)
            };
            
            var deckOf30 = new Card[]
            {
                new Card(CardFace.Ten, CardSuit.Clubs),
                new Card(CardFace.Nine, CardSuit.Clubs),
                new Card(CardFace.Ace, CardSuit.Clubs),
                new Card(CardFace.Jack, CardSuit.Clubs)
            };
            
            var deckOf21 = new Card[]
            {
                new Card(CardFace.Ten, CardSuit.Clubs),
                new Card(CardFace.Ace, CardSuit.Clubs)
            };

            // Act.
            BlackjackRules.ValueOf(deckOf18, out var valueOf18, out var softOf18);
            BlackjackRules.ValueOf(deckOf30, out var valueOf30, out var softOf30);
            BlackjackRules.ValueOf(deckOf21, out var valueOf21, out var softOf21);

            // Assert.
            Assert.Equal(8, valueOf18);
            Assert.Equal(18, softOf18);
            
            Assert.Equal(30, valueOf30);
            Assert.Equal(30, softOf30);

            Assert.Equal(11, valueOf21);
            Assert.Equal(21, softOf21);
        }

        [Fact()]
        public void ValueOfMultipleAcesTest()
        {
            // One ace is either 1 or 11.
            BlackjackRules.ValueOf(new Card[]
            {
                new Card(CardFace.Ace, CardSuit.Clubs)
            }, out var value, out var soft);

            Assert.Equal(1, value);
            Assert.Equal(11, soft);

            // Two aces is 2 or 12.
            BlackjackRules.ValueOf(new Card[]
            {
                new Card(CardFace.Ace, CardSuit.Clubs),
                new Card(CardFace.Ace, CardSuit.Clubs)
            }, out value, out soft);
            
            Assert.Equal(2, value);
            Assert.Equal(12, soft);

            // Three aces is 3 or 13.
            BlackjackRules.ValueOf(new Card[]
            {
                new Card(CardFace.Ace, CardSuit.Clubs),
                new Card(CardFace.Ace, CardSuit.Clubs),
                new Card(CardFace.Ace, CardSuit.Clubs)
            }, out value, out soft);

            Assert.Equal(3, value);
            Assert.Equal(13, soft);
        }
    }
}
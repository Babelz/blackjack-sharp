﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Static utility class that contains rules for blackjack.
    /// </summary>
    public static class BlackjackRules
    {
        /// <summary>
        /// Returns boolean declaring whether given cards allow
        /// splitting.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanSplit(IEnumerable<Card> cards)
            => cards.Count() == 2 && cards.ElementAt(0).Face == cards.ElementAt(1).Face;

        /// <summary>
        /// Returns boolean declaring whether given cards allow doubling.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanDouble(IEnumerable<Card> cards)
        {
            if (cards.Count() != 2) return false;

            var sum = cards.Sum(c => (byte)c.Face);

            return sum >= 9 && sum <= 11;
        }

        /// <summary>
        /// Returns boolean declaring whether value represents a bust.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBusted(int value)
            => value > 21;

        /// <summary>
        /// Returns boolean declaring whether value represents a blackjack.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBlackjack(int value )
            => value == 21;

        /// <summary>
        /// Returns value and soft value of given card.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValueOf(Card card, out int value, out int soft)
        {
            switch (card.Face)
            {
                case CardFace.Ace:
                    value = 1;
                    soft  = 11;
                    break;
                case CardFace.Jack:
                case CardFace.Queen:
                case CardFace.King:
                    value = 10;
                    soft  = value;
                    break;
                default:
                    value = (byte)card.Face;
                    soft  = value;
                    break;
            }
        }

        /// <summary>
        /// Counts value and soft value of given cards.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValueOf(IEnumerable<Card> cards, out int value, out int soft)
        {
            value = 0;
            soft  = 0;

            foreach (var card in cards)
            {
                ValueOf(card, out var cardValue, out var cardSoft);

                if (card.Face == CardFace.Ace && IsBusted(cardSoft + soft))
                    cardSoft = 1;

                value += cardValue;
                soft  += cardSoft;
            }
        }
    }
}

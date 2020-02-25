using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Static utility class that contains rules for blackjack.
    /// </summary>
    public static class BlackjackRules
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanSplit(IEnumerable<Card> cards)
            => cards.Count() == 2 && cards.ElementAt(0).Face == cards.ElementAt(1).Face;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanDouble(IEnumerable<Card> cards)
        {
            if (cards.Count() != 2) return false;

            var sum = cards.Sum(c => (byte)c.Face);

            return sum >= 9 && sum <= 11;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBlackjack(IEnumerable<Card> cards)
            => cards.Sum(c => (byte)c.Face) == 21;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValueOf(Card card, out int low, out int high)
        {
            switch (card.Face)
            {
                case CardFace.Ace:
                    low  = 1;
                    high = 11;
                    break;
                case CardFace.Jack:
                case CardFace.Queen:
                case CardFace.King:
                    low  = 10;
                    high = low;
                    break;
                default:
                    low  = (byte)card.Face;
                    high = low;
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValueOf(IEnumerable<Card> cards, out int low, out int high)
        {
            low  = 0;
            high = 0;
            
            foreach (var card in cards)
            {
                ValueOf(card, out var cardLow, out var cardHigh);

                low  += cardLow;
                high += cardHigh;
            }
        }
    }
}

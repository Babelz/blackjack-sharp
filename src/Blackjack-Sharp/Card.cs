﻿namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that represents a standard French playing card defined by it's
    /// suit and face.
    /// </summary>
    public sealed class Card
    {
        #region Properties
        /// <summary>
        /// Gets the face of the card.
        /// </summary>
        public CardFace Face
        {
            get;
        }

        /// <summary>
        /// Gets the suit of the card.
        /// </summary>
        public CardSuit Suit
        {
            get;
        }
        #endregion

        public Card(CardFace face, CardSuit suit)
        {
            Face = face;
            Suit = suit;
        }

        public sealed override string ToString()
        {
            var prefix = char.ToUpper(Suit.ToString()[0]);
            var suffix = (Face) switch
            {
                CardFace.Ace   => "ace",
                CardFace.Jack  => "jack",
                CardFace.Queen => "queen",
                CardFace.King  => "king",
                _              => ((byte)Face).ToString(),
            };

            return $"{prefix}-{suffix}";
        }
    }
}

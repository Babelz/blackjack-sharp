﻿using System.Collections.Generic;
using System.Linq;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Static utility class containing player options related thing.
    /// </summary>
    public static class PlayerOptions
    {
        #region Constant fields
        public const string OptHit    = "hit";
        public const string OptStay   = "stay";
        public const string OptDouble = "double";
        #endregion

        /// <summary>
        /// Return enumerable containing possible options for player
        /// based on hes state.
        /// </summary>
        public static IEnumerable<string> DetermineOps(Player player, Hand hand)
        {
            var opts = new List<string>()
            {
                OptHit,
                OptStay
            };

            // Do not allow doubling after split.
            if (!player.IsSplit && hand.Count() == 2)
                opts.Add(OptDouble);

            return opts;
        }
    }
}

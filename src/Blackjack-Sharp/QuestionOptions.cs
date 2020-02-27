using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Static utility class containing question options related things.
    /// </summary>
    public static class QuestionOptions
    {
        #region Constant fields
        public const string OptYes = "yes";
        public const string OptNo  = "no";
        
        public const string OptYesShort = "y";
        public const string OptNoShort  = "n";
        #endregion

        #region Properties
        /// <summary>
        /// Returns enumerable containing all possible options.
        /// </summary>
        public static IEnumerable<string> Opts
            => new string[]
            {
                OptYes,
                OptNo,

                OptYesShort,
                OptNoShort
            };
        #endregion
    }
}

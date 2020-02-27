using System;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Interface for implementing classes that provide console for blackjack game.
    /// </summary>
    public interface IBlackjackConsole
    {
        /// <summary>
        /// Attempts to ask a line from the player. Returns boolean declaring 
        /// whether line was entered.
        /// </summary>
        bool TryAskLine(string what, out string value, Func<string, bool> validation = null);

        /// <summary>
        /// Attempts to ask a signed integer from the player. Returns boolean declaring 
        /// whether value was entered successfully.
        /// </summary>
        bool TryAskSigned(string what, out int value, Func<int, bool> validation = null);

        /// <summary>
        /// Attempts to ask a unsigned integer from the player. Returns boolean declaring 
        /// whether value was entered successfully
        /// </summary>
        bool TryAskUnsigned(string what, out uint value, Func<uint, bool> validation = null);
       
        /// <summary>
        /// Writes given line as dealer info to the console.
        /// </summary>
        void WriteDealerInfo(string line);

        void WriteLine(string line);

        /// <summary>
        /// Writes given line to a player to the console.
        /// </summary>
        void WritePlayerInfo(string name, string line);

        /// <summary>
        /// Writes separator to the console.
        /// </summary>
        void WriteSeparator();

        /// <summary>
        /// Writes given warning to the console.
        /// </summary>
        void WriteWarning(string line);
    }
}
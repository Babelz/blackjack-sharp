using System;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that provides console for blackjack game.
    /// </summary>
    public sealed class BlackjackConsole 
    {
        #region Fields
        private readonly ConsoleColor playerColor;
        private readonly ConsoleColor dealerColor;
        #endregion

        public BlackjackConsole(ConsoleColor playerColor, ConsoleColor dealerColor)
        {
            this.playerColor = playerColor;
            this.dealerColor = dealerColor;
        }

        public void WriteLine(string line)
        {
            Console.Write("");

            Console.WriteLine($"{line}");
        }

        /// <summary>
        /// Writes given line as dealer info to the console.
        /// </summary>
        public void WriteDealerInfo(string line)
        {
            Console.Write("dealer: ");

            Console.ForegroundColor = dealerColor;

            Console.WriteLine($"{line}");

            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Writes given line to a player to the console.
        /// </summary>
        public void WritePlayerInfo(string name, string line)
        {
            Console.Write($"{name}: ");

            Console.ForegroundColor = playerColor;

            Console.WriteLine($"{line}");

            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Writes given warning to the console.
        /// </summary>
        public void WriteWarning(string line)
        {
            Console.Write("warning! ");

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"{line}");

            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Writes separator to the console.
        /// </summary>
        public void WriteSeparator()
            => Console.WriteLine("----------------");
        
        /// <summary>
        /// Attempts to ask a line from the player. Returns boolean declaring 
        /// whether line was entered.
        /// </summary>
        public bool TryAskLine(string what, out string value)
        {
            Console.Write($"{what}: ");

            value = Console.ReadLine();

            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Attempts to ask a signed integer from the player. Returns boolean declaring 
        /// whether value was entered successfully.
        /// </summary>
        public bool TryAskSigned(string what, out int value)
        {
            Console.Write($"{what}: ");

            return int.TryParse(Console.ReadLine(), out value);
        }

        /// <summary>
        /// Attempts to ask a unsigned integer from the player. Returns boolean declaring 
        /// whether value was entered successfully
        /// </summary>
        public bool TryAskUnsigned(string what, out uint value)
        {
            Console.Write($"{what}: ");

            return uint.TryParse(Console.ReadLine(), out value);
        }
    }
}

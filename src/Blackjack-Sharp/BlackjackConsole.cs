using System;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that provides console related blackjack operations.
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

        public void WriteDealerInfo(string line)
        {
            Console.Write("dealer: ");

            Console.ForegroundColor = dealerColor;

            Console.WriteLine($"{line}");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WritePlayerInfo(string name, string line)
        {
            Console.Write($"{name}: ");

            Console.ForegroundColor = playerColor;

            Console.WriteLine($"{line}");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteWarning(string line)
        {
            Console.Write("warning! ");

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"{line}");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteSeparator()
            => Console.WriteLine("----------------");
    
        public bool TryAskLine(string what, out string value)
        {
            Console.Write($"{what}: ");

            value = Console.ReadLine();

            return string.IsNullOrEmpty(value);
        }

        public bool TryAskSigned(string what, out int value)
        {
            Console.Write($"{what}: ");

            return int.TryParse(Console.ReadLine(), out value);
        }

        public bool TryAskUnsigned(string what, out uint value)
        {
            Console.Write($"{what}: ");

            return uint.TryParse(Console.ReadLine(), out value);
        }
    }
}

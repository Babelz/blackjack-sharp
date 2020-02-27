using System;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Default implementation of <see cref="IBlackjackConsole"/>.
    /// </summary>
    public sealed class BlackjackConsole : IBlackjackConsole
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
            => Console.WriteLine(line);

        public void WriteDealerInfo(string line)
        {
            Console.Write("dealer: ");

            Console.ForegroundColor = dealerColor;

            Console.Write($"{line}{Environment.NewLine}");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WritePlayerInfo(string name, string line)
        {
            Console.Write($"{name}: ");

            Console.ForegroundColor = playerColor;

            Console.Write($"{line}{Environment.NewLine}");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteWarning(string line)
        {
            Console.Write("warning! ");

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write($"{line}{Environment.NewLine}");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteSeparator()
            => Console.WriteLine("----------------");

        public bool TryAskLine(string what, out string value, Func<string, bool> validation = null)
        {
            Console.Write($"{what}: ");

            value = Console.ReadLine();

            return !string.IsNullOrEmpty(value) && (validation?.Invoke(value) ?? true);
        }

        public bool TryAskSigned(string what, out int value, Func<int, bool> validation = null)
        {
            Console.Write($"{what}: ");

            return int.TryParse(Console.ReadLine(), out value) && (validation?.Invoke(value) ?? true);
        }

        public bool TryAskUnsigned(string what, out uint value, Func<uint, bool> validation = null)
        {
            Console.Write($"{what}: ");

            return uint.TryParse(Console.ReadLine(), out value) && (validation?.Invoke(value) ?? true);
        }
    }
}

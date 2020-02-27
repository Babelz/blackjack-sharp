using System;

namespace Blackjack_Sharp
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var application = new BlackjackApplication(
                new Dealer(5), 
                new BlackjackConsole(ConsoleColor.Green, ConsoleColor.Cyan));

            application.Run();
        }
    }
}

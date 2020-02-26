using System;
using System.Collections.Generic;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that provides epic CLI blackjack experience.
    /// </summary>
    public sealed class BlackjackApplication
    {
        #region Constant fields
        /// <summary>
        /// Initial balance of all players in whole euros. 1000€
        /// </summary>
        public const int InitialBalance = 1000;
        #endregion

        #region Fields
        private readonly Dealer dealer;
        
        private readonly BlackjackConsole console;
        private readonly BlackjackGame game;

        private readonly List<Player> activePlayers;
        private readonly List<Player> absentPlayers;

        private bool running;
        #endregion

        public BlackjackApplication()
        {
            // Just initialize dependencies in the constructor, no need for
            // DI as we can test this class with them...
            dealer  = new Dealer(5);
            console = new BlackjackConsole(ConsoleColor.Green, ConsoleColor.Cyan);
            game    = new BlackjackGame(console);

            activePlayers = new List<Player>();
            absentPlayers = new List<Player>();
        }

        /// <summary>
        /// Runs initialization for the game.
        /// </summary>
        private void Initialize()
        {
            // Initialize initial players.
            var playersCount = game.AskPlayersCount();

            for (var i = 0; i < playersCount; i++)
            {
                var playerName = game.AskPlayerName(i + 1);

                activePlayers.Add(new Player(playerName, InitialBalance));
            }

            running = true;
        }

        /// <summary>
        /// Runs the game for one round.
        /// </summary>
        private void RunOnce()
        {
            // 
        }

        /// <summary>
        /// Begins executing the blackjack application until exit is called.
        /// </summary>
        public void Run()
        {
            Initialize();

            while (!running) RunOnce();
        }
    }
}

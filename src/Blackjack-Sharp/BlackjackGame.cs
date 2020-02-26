using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Linq;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that provides epic CLI blackjack experience.
    /// </summary>
    public sealed class BlackjackGame
    {
        #region Constant fields
        /// <summary>
        /// Initial balance of all players in whole euros. 1000€
        /// </summary>
        public const int InitialBalance = 1000;
        #endregion

        #region Fields
        private readonly Dealer dealer;

        // Lookup that associates players with their bets.
        private readonly Dictionary<Player, List<PlayerBet>> bets;

        private readonly BlackjackConsole console;

        private readonly List<Player> playing;
        private readonly List<Player> active;
        private readonly List<Player> absent;

        private bool running;
        #endregion

        public BlackjackGame()
        {
            // Just initialize dependencies in the constructor, no need for
            // DI as we can test this class with them...
            dealer  = new Dealer(5);
            console = new BlackjackConsole(ConsoleColor.Green, ConsoleColor.Cyan);

            playing = new List<Player>();
            active  = new List<Player>();
            absent  = new List<Player>();

            bets = new Dictionary<Player, List<PlayerBet>>();
        }

        /// <summary>
        /// Introduces small delay during operations of game play.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Delay()
            => Thread.Sleep(TimeSpan.FromMilliseconds(250.0d));
        
        private void Exit()
        {
            // Print player statuses.
            console.WriteLine("-- player statistics --");

            foreach (var player in absent)
            {
                console.WriteLine($"{player}");
                console.WriteLine($"balance: {player.Wallet.Balance}€\n");
            }

            running = false;
        }

        private void EndRound()
        {
            throw new NotImplementedException();
        }

        private void DealerPlay()
        {
            throw new NotImplementedException();
        }

        private void PlayerPlay()
        {
            throw new NotImplementedException();
        }

        private void RevealPlayerCards()
        {
            foreach (var player in playing)
                game.RevealCards()
        }

        private void RevealDealersFirstCard()
        {
            console.WriteDealerInfo($"first card is {dealer.Hand.}");
        }
        
        private void DealInitialCards()
        {
            const int InitialCards = 2;

            for (var i = 0; i < InitialCards; i++)
            {
                // Begin by dealing to dealer.
                dealer.DealSelf();

                foreach (var player in playing)
                    dealer.Deal(player.PrimaryHand);
            }
        }

        private void PlaceBets()
        {
            // Go trough all players, ask for initial bet before beginning the round.
            foreach (var player in active)
            {
                // Keep asking the player for the bet until he gives us valid bet value...
                var amount = 0u;

                while (!console.TryAskUnsigned($"{player.Name}, please place your bet between 1 and {player.Wallet.Balance} euros",
                                               out amount))
                {
                    console.WriteWarning("your bet is invalid!");
                }

                // Create bet and add to playing group.
                bets[player].Add(new PlayerBet(player.PrimaryHand, amount));

                playing.Add(player);

                console.WriteDealerInfo("")
            }
        }

        private void AskIfPlaying()
        {
            foreach (var player in active)
            {
                var value = string.Empty;

                while (!console.TryAskLine("are you going to play? (yes, no)", out value))
                    console.WriteWarning("invalid answer");

                switch (value)
                {
                    case "y":
                    case "yes":
                        // Make active for this round.
                        console.WriteDealerInfo($"welcome to the game {player.Name}");

                        playing.Add(player);
                        break;
                    case "n":
                    case "no":
                        console.WriteDealerInfo($"well, maybe next round, your loss...");
                        break;
                    default:
                        console.WriteWarning("sorry i could not understand you so i assume you are not playing...");
                        break;
                }
            }
        }

        /// <summary>
        /// Runs the game for one round.
        /// </summary>
        public void RunOnce()
        {
            // Ask players if they are playing.
            AskIfPlaying();

            // Start by placing bets.
            PlaceBets();

            // Deal initial cards starting from the first player, deal to the dealer last.
            DealInitialCards();

            // Reveal first card of the dealer.
            RevealDealersFirstCard();

            // Reveal cards of the players.
            RevealPlayerCards();

            // Go trough players and allow them to play starting from first player.
            PlayerPlay();

            // Allow dealer to play.
            DealerPlay();

            // Handle results based on outcome of play.
            EndRound();

            // Exit game if no active players are left.
            if (active.Count == 0)
                Exit();
        }

        /// <summary>
        /// Runs initialization for the game.
        /// </summary>
        public void Initialize()
        {
            // Initialize initial players.
            var playersCount = game.AskPlayersCount();

            for (var i = 0; i < playersCount; i++)
            {
                // Create player.
                var playerName = game.AskPlayerName(i + 1);
                var player     = new Player(playerName, InitialBalance);

                active.Add(player);

                // Create lookup.
                bets.Add(player, new List<PlayerBet>());
            }

            running = true;
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

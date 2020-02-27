using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Linq;
using System.Text;

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

        private void RevealDealersSecondCard()
        {
            throw new NotImplementedException();
        }

        private void PlayersPlay()
        {
            throw new NotImplementedException();
        }

        private void RevealPlayerCards()
        {
            foreach (var player in playing)
            {
                // Compute hand value.
                BlackjackRules.ValueOf(player.PrimaryHand, out var value, out var soft);
                
                // Construct string.
                var sb = new StringBuilder();
                
                sb.Append("my hand has the following cards ");

                sb.Append(player.PrimaryHand.First().ToString());
                sb.Append(", ");
                sb.Append(player.PrimaryHand.Last().ToString());

                sb.Append($" with value {value} and soft value of {soft}");

                console.WritePlayerInfo(player.Name, sb.ToString());

                Delay();
            }
        }

        private void RevealDealersFirstCard()
            => console.WriteDealerInfo($"my first card is {dealer.Hand.First().ToString()}");
        
        private void DealCards()
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

        private void AskPlayerBets()
        {
            // Go trough all players, ask for initial bet before beginning the round.
            foreach (var player in active)
            {
                // Keep asking the player for the bet until he gives us valid bet value...
                var amount = 0u;

                while (!console.TryAskUnsigned($"{player.Name}, please place your bet between 1 and {player.Wallet.Balance} euros",
                                               out amount,
                                               n => n >= 1 && n <= player.Wallet.Balance))
                {
                    console.WriteWarning("your bet is invalid!");
                }

                // Remove from wallet.
                player.Wallet.Take(amount);

                // Create bet and add to playing group.
                bets[player].Add(new PlayerBet(player.PrimaryHand, amount));

                Delay();
            }
        }

        private void StartRound()
        {
            console.WriteSeparator();

            console.WriteDealerInfo("new round begins, place your bets");

            foreach (var player in active)
            {
                var value = string.Empty;

                while (!console.TryAskLine($"{player.Name}, are you going to play? (yes, no)", 
                                           out value, 
                                           s => new [] { "yes", "no", "y", "n" }.Contains(s)))
                {
                    console.WriteWarning("invalid answer");
                }

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

                Delay();
            }
        }

        /// <summary>
        /// Runs the game for one round.
        /// </summary>
        public void RunOnce()
        {
            // Ask players if they are playing.
            StartRound();

            // Start by placing bets.
            AskPlayerBets();

            // Deal initial cards starting from the first player, deal to the dealer last.
            DealCards();

            // Reveal first card of the dealer.
            RevealDealersFirstCard();

            // Reveal cards of the players.
            RevealPlayerCards();

            // Go trough players and allow them to play starting from first player.
            PlayersPlay();

            // Reveal second dealer card.
            RevealDealersSecondCard();

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
            // Print epic logo.             
            console.WriteLine(@" _     _            _    _            _               _                      ");                      
            console.WriteLine(@"| |   | |          | |  (_)          | |             | |                     ");
            console.WriteLine(@"| |__ | | __ _  ___| | ___  __ _  ___| | ________ ___| |__   __ _ _ __ _ __  ");
            console.WriteLine(@"| '_ \| |/ _` |/ __| |/ / |/ _` |/ __| |/ /______/ __| '_ \ / _` | '__| '_ \ ");
            console.WriteLine(@"| |_) | | (_| | (__|   <| | (_| | (__|   <       \__ \ | | | (_| | |  | |_) |");
            console.WriteLine(@"|_.__/|_|\__,_|\___|_|\_\ |\__,_|\___|_|\_\      |___/_| |_|\__,_|_|  | .__/ ");
            console.WriteLine(@"                       _/ |                                           | |    ");
            console.WriteLine(@"                      |__/                                            |_|    ");
            console.WriteLine("\nby: babelz\n");
            
            console.WriteSeparator();

            // Initialize initial players.
            var playersCount = 0u;

            while (!console.TryAskUnsigned("number of players between 1 and 4", 
                                           out playersCount, 
                                           n => n >= 1 && n <= 4))
            {
                console.WriteWarning("expecting a number between 1 and 4!");
            }

            for (var i = 0; i < playersCount; i++)
            {
                // Create player.
                var playerName = string.Empty;

                while (!console.TryAskLine($"name of the player at seat {i + 1}", out playerName))
                    console.WriteWarning("expecting a name!");
                
                var player = new Player(playerName, InitialBalance);

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

            while (running) RunOnce();
        }
    }
}

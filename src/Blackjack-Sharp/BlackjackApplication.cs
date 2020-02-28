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
    public sealed class BlackjackApplication
    {
        #region Constant fields
        /// <summary>
        /// Initial balance of all players in whole euros. 1000e
        /// </summary>
        public const int InitialBalance = 1000;
        #endregion

        #region Fields
        // Lookup that associates players with their bets.
        private readonly Dictionary<Player, List<PlayerBet>> bets;

        private readonly IBlackjackConsole console;
        private readonly IDealer dealer;

        private readonly List<Player> playingPlayers;
        private readonly List<Player> activePlayers;
        private readonly List<Player> absentPlayers;

        private bool running;
        #endregion

        public BlackjackApplication(IDealer dealer, IBlackjackConsole console)
        {
            this.dealer  = dealer ?? throw new ArgumentNullException(nameof(dealer)); 
            this.console = console ?? throw new ArgumentNullException(nameof(console));
            
            playingPlayers = new List<Player>();
            activePlayers  = new List<Player>();
            absentPlayers  = new List<Player>();

            bets = new Dictionary<Player, List<PlayerBet>>();
        }

        /// <summary>
        /// Introduces small delay during operations of game play.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Delay()
            => Thread.Sleep(TimeSpan.FromMilliseconds(750.0d));

        /// <summary>
        /// Gets card and value information of single hand.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GetHandInfo(Hand hand, out string cards, out int value, out int soft)
        {
            // Compute hand values.
            BlackjackRules.ValueOf(hand, out value, out soft);

            // Make string representing the whole hand.
            cards = string.Join(", ", hand.Select(s => s.ToString()));
        }

        private void Exit()
        {
            // Print player statuses.
            console.WriteLine("-- player statistics --");

            foreach (var player in absentPlayers)
            {
                console.WriteLine($"{player.Name}");
                console.WriteLine($"balance: {player.Wallet.Balance}e\n");
            }

            running = false;
        }

        private void EndRound()
        {
            // Determine the outcome of player bets.
            var didDealerHitBlackjack = BlackjackRules.IsBlackjack(dealer.Hand);
            var didDealerBust         = BlackjackRules.IsBusted(dealer.Hand);

            BlackjackRules.ValueOf(dealer.Hand,
                                   out var dealerValue,
                                   out var dealerSoft);

            foreach (var betPair in bets)
            {
                var player = betPair.Key;

                console.WriteDealerInfo($"{player.Name}, checking outcome of your hands...");

                for (var i = 0; i < betPair.Value.Count; i++)
                {
                    var bet           = betPair.Value[i];
                    var didPlayerBust = BlackjackRules.IsBusted(bet.Hand);

                    GetHandInfo(bet.Hand, 
                                out var cards, 
                                out var playerValue, 
                                out var playerSoft);

                    console.WriteDealerInfo(
                        $"your hand {i + 1}/{betPair.Value.Count} has cards {cards} " +
                        $"with value {playerValue}/{playerSoft}");

                    if (didPlayerBust || didDealerHitBlackjack)
                    {
                        console.WritePlayerInfo(
                            player.Name, 
                            $"your hand {i + 1}/{betPair.Value.Count} lost total {bet.Amount}e");
                    }
                    else
                    {
                        var winAmount = 0u;

                        if (BlackjackRules.IsBlackjack(bet.Hand))
                        {
                            winAmount = bet.Amount * 2u;

                            console.WritePlayerInfo(
                                player.Name,
                                $"hand {i + 1}/{betPair.Value.Count} blackjack wins {winAmount}e");
                        }
                        else if (!didPlayerBust && didDealerBust)
                        {
                            winAmount = bet.Amount * 2u;
                            
                            console.WritePlayerInfo(
                                player.Name,
                                $"hand {i + 1}/{betPair.Value.Count} wins {winAmount}e, dealer bust");
                        }
                        else
                        {
                            if (playerValue > dealerValue || playerSoft > dealerSoft)
                            {
                                winAmount = bet.Amount * 2u;

                                console.WritePlayerInfo(
                                    player.Name,
                                    $"hand {i + 1}/{betPair.Value.Count} wins {winAmount}e");
                            }
                            else
                            {
                                console.WritePlayerInfo(
                                    player.Name, 
                                    $"your hand {i + 1}/{betPair.Value.Count} lost total {bet.Amount}e");
                            }
                        }

                        player.Wallet.Put(winAmount);
                    }

                    Delay();
                }
            }

            // Clear player and game states.
            foreach (var player in playingPlayers)
            {
                bets[player].Clear();

                player.Clear();

                if (player.Wallet.Empty)
                {
                    console.WriteDealerInfo($"{player.Name} you are out! come back when you have some money to play!");

                    activePlayers.Remove(player);
                    absentPlayers.Add(player);
                }
            }
            
            playingPlayers.Clear();

            // Clear dealer state.
            dealer.Hand.Clear();
        }

        private void DealerPlay()
        {
            // Flag to keep track of blackjack, bust or preferred value.
            var playing = true;

            while (playing)
            {
                // Get possible out comes.
                var isBlackjack = BlackjackRules.IsBlackjack(dealer.Hand);
                var isBust      = BlackjackRules.IsBusted(dealer.Hand);
                var shouldStay  = BlackjackRules.ShouldStay(dealer.Hand);

                // Act according to outcome.
                if      (isBlackjack) console.WriteDealerInfo("blackjack!");
                else if (isBust)      console.WriteDealerInfo("bust!");
                else if (shouldStay)  console.WriteDealerInfo("staying at my current hand");
                else
                {
                    // Deal self one card and reveal new hand.
                    console.WriteDealerInfo("dealing myself one");

                    dealer.DealSelf();

                    RevealDealerHand();
                }

                playing = !(isBlackjack || isBust || shouldStay);

                Delay();
            }

            console.WriteDealerInfo("my turn is over");
        }

        private void RevealDealerHand()
        {
            // Begin revealing the full hand of dealer.
            GetHandInfo(dealer.Hand, out var cards, out var value, out var soft);

            console.WriteDealerInfo($"my hand has cards {cards} and value {value}/{soft}");
        }

        private void PlayerPlayHand(Player player, Hand hand, int handIndex, int handsCount)
        {
            // Flag to keep track of blackjacks, busts and stays.
            var playing = true;

            while (playing)
            {
                var opts   = PlayerOptions.DetermineOps(player, hand);
                var option = string.Empty;

                while (!console.TryAskLine($"playing your hand {handIndex + 1}/{handsCount}, what will you do? " +
                                           $"({string.Join(", ", opts)})",
                                           out option,
                                           s => opts.Contains(s)))
                {
                    console.WriteWarning("invalid operation!");
                }

                switch (option)
                {
                    case PlayerOptions.OptStay:
                        // Nothing else to do, end turn.
                        console.WriteDealerInfo("staying, your turn is over");

                        playing = false;
                        break;
                    case PlayerOptions.OptHit:
                        // Deal one card and reveal the hand to player.
                        console.WriteDealerInfo("dealing one card...");

                        dealer.Deal(hand);

                        RevealPlayerCards(player, hand);

                        if (BlackjackRules.IsBlackjack(hand))
                        {
                            // Handle blackjack.
                            console.WriteDealerInfo("blackjack! your turn is over");

                            playing = false;
                        }
                        else if (BlackjackRules.IsBusted(hand))
                        {
                            // Handle bust.
                            console.WriteDealerInfo("busted! sorry, your hand is out");

                            playing = false;
                        }
                        break;
                    case PlayerOptions.OptDouble:
                        var bet = bets[player].First();

                        // Do not allow doubling if the player has not enough balance.
                        if (player.Wallet.Balance < bet.Amount)
                            console.WriteDealerInfo("sorry, you do not have enough balance to double");
                        else
                        {
                            // Do the actual doubling.
                            console.WriteDealerInfo("doubling your hand...");

                            // Deal single card as of doubling.
                            dealer.Deal(hand);

                            // Take double amount from wallet.
                            player.Wallet.Take(bet.Amount);

                            // Create new bet in place of the current one, we can 
                            // always assume there is only one bet in play when
                            // the player is doubling because casino does not allow
                            // doubling split hands.
                            var newBet = new PlayerBet(hand, bet.Amount * 2u);

                            bets[player] = new List<PlayerBet>
                            {
                                newBet
                            };

                            console.WriteDealerInfo($"your total bet is now {newBet.Amount}e and your total balance is now {player.Wallet.Balance}e");

                            RevealPlayerCards(player, hand);

                            playing = false;
                        }
                        break;
                    default:
                        console.WriteWarning("sorry i could not understand you so i assume you are staying...");

                        playing = false;
                        break;
                }

                Delay();
            }
        }

        private void AskPlayerSplit(Player player, out bool aceSplit)
        {
            aceSplit = false;

            var option = string.Empty;

            while (!console.TryAskLine($"{player.Name}, do you want to split? ({string.Join(", ", QuestionOptions.Opts)})",
                    out option,
                    s => QuestionOptions.Opts.Contains(s)))
            {
                console.WriteWarning("invalid answer");
            }

            switch (option)
            {
                case QuestionOptions.OptYes:
                case QuestionOptions.OptYesShort:
                    // Do not allow more plays after ace split.
                    aceSplit = player.PrimaryHand.Count(c => c.Face == CardFace.Ace) == 2;

                    console.WriteDealerInfo("splitting for you");

                    // Split the primary hand.
                    player.SecondaryHand = player.PrimaryHand.Split();

                    // Deal one card to both cards.
                    dealer.Deal(player.PrimaryHand);
                    dealer.Deal(player.SecondaryHand);

                    RevealPlayerCards(player, player.PrimaryHand);
                    RevealPlayerCards(player, player.SecondaryHand);

                    // Add secondary bet.
                    var bet = bets[player].First();

                    bets[player].Add(new PlayerBet(player.SecondaryHand, bet.Amount));

                    // Update wallet.
                    player.Wallet.Take(bet.Amount);

                    if (aceSplit)
                        console.WriteDealerInfo("split two aces, your round ended");
                    break;
                default:
                    break;
            }

            Delay();
        }

        private void PlayersPlay()
        {
            foreach (var player in playingPlayers)
            {
                console.WritePlayerInfo(player.Name, "it's my turn");

                // Handle splitting for the player if he prefers to split.
                var aceSplit = false;
                
                if (BlackjackRules.CanSplit(player.PrimaryHand) && player.Wallet.Balance >= bets[player].First().Amount)
                    AskPlayerSplit(player, out aceSplit);

                // Play the actual hands if not ace split.
                if (aceSplit) continue;

                // Determine hands.
                var hands = new List<Hand>
                {
                    player.PrimaryHand
                };

                if (player.IsSplit)
                    hands.Add(player.SecondaryHand);

                // Play the actual hands.
                for (var i = 0; i < hands.Count; i++)
                    PlayerPlayHand(player, hands[i], i, hands.Count);
            }
        }

        private void RevealPlayerCards(Player player, Hand hand)
        {
            // Get hand information for display.
            GetHandInfo(hand, out var cards, out var value, out var soft);

            // Reveal the hand.
            console.WritePlayerInfo(
                player.Name,
                $"my hand has the following cards {cards} with value {value}/{soft}");
        }

        private void RevealInitialPlayerCards()
        {
            foreach (var player in playingPlayers)
            {
                RevealPlayerCards(player, player.PrimaryHand);

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

                foreach (var player in playingPlayers)
                    dealer.Deal(player.PrimaryHand);
            }
        }

        private void AskPlayerBets()
        {
            // Go trough all players, ask for initial bet before beginning the round.
            foreach (var player in activePlayers)
            {
                // Keep asking the player for the bet until he gives us valid bet value...
                var amount = 0u;

                while (!console.TryAskUnsigned($"{player.Name}, please place your bet (1-{player.Wallet.Balance}e)",
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

            foreach (var player in activePlayers)
            {
                var option = string.Empty;

                while (!console.TryAskLine($"{player.Name}, are you going to play? ({string.Join(", ", QuestionOptions.Opts)})", 
                                           out option, 
                                           s => QuestionOptions.Opts.Contains(s)))
                {
                    console.WriteWarning("invalid answer");
                }

                switch (option)
                {
                    case QuestionOptions.OptYes:
                    case QuestionOptions.OptYesShort:
                        // Make active for this round.
                        console.WriteDealerInfo($"welcome to the game {player.Name}");

                        playingPlayers.Add(player);
                        break;
                    case QuestionOptions.OptNo:
                    case QuestionOptions.OptNoShort:
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
            RevealInitialPlayerCards();

            // Go trough players and allow them to play starting from first player.
            PlayersPlay();

            // Reveal second dealer card.
            RevealDealerHand();

            // Allow dealer to play.
            DealerPlay();

            // Handle results based on outcome of play.
            EndRound();

            // Exit game if no active players are left.
            if (activePlayers.Count == 0)
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

                activePlayers.Add(player);

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

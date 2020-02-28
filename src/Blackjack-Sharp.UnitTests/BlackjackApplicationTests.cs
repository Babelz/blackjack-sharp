using Blackjack_Sharp.UnitTests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blackjack_Sharp.UnitTests
{
    /// <summary>
    /// Class containing few simple application related tests.
    /// </summary>
    public class BlackjackApplicationTests
    {
        private static IBlackjackConsole CreateConsoleWithSetupSequence(
            IEnumerable<TryAskCallback<uint>> askUnsignedCallbackSequence = null,
            IEnumerable<TryAskCallback<int>> askSignedCallbackSequence = null,
            IEnumerable<TryAskCallback<string>> askStringCallbackSequence = null,
            IEnumerable<WriteCallback> writeCallbackSequence = null,
            IEnumerable<WritePlayerCallback> writePlayerCallbackSequence = null)
        {
            askUnsignedCallbackSequence ??= Enumerable.Empty<TryAskCallback<uint>>();
            askSignedCallbackSequence   ??= Enumerable.Empty<TryAskCallback<int>>();
            askStringCallbackSequence   ??= Enumerable.Empty<TryAskCallback<string>>();

            writeCallbackSequence       ??= Enumerable.Empty<WriteCallback>();
            writePlayerCallbackSequence ??= Enumerable.Empty<WritePlayerCallback>();

            return new FakeBlackjackConsole(
                // When game asks how many players are playing.
                askUnsignedCallbackSequence: Enumerable.Concat(
                    new TryAskCallback<uint>[] { (out uint i) => i = 1u },
                    askUnsignedCallbackSequence),
                
                askSignedCallbackSequence: askSignedCallbackSequence,

                // When game asks name from the players.
                askStringCallbackSequence: Enumerable.Concat(
                    new TryAskCallback<string>[] { (out string t) => t = "test-player" },
                    askStringCallbackSequence),

                writeCallbackSequence: writeCallbackSequence,

                writePlayerCallbackSequence: writePlayerCallbackSequence
            );   
        }

        [Fact()]
        public void PlayerBustTest()
        {
            // SUT.
            BlackjackApplication application = null;

            // Act. Allow the application to run
            // for a while.
            var test = Task.Run(() =>
            {
                // Arrange.
                var dealersDeck = new Card[]
                {
                    new Card(CardFace.King, CardSuit.Hearts),
                    new Card(CardFace.Seven, CardSuit.Hearts)
                };
            
                var playersDeck = new Card[]
                {
                    new Card(CardFace.Seven, CardSuit.Hearts),
                    new Card(CardFace.Eight, CardSuit.Diamonds),
                    new Card(CardFace.Ten, CardSuit.Clubs)
                };

                var askUnsignedSequence = new TryAskCallback<uint>[]
                {
                    // Asking for bet.
                    (out uint i) => i = 500u,
                };

                var askStringSequence = new TryAskCallback<string>[]
                {
                    // Ask if you are going to play.
                    (out string s) => s = "y",

                    // Ask for action, hit 3 times.
                    (out string s) => s = "hit",
                    (out string s) => s = "hit",
                    (out string s) => s = "hit",
                };

                application = new BlackjackApplication(
                    new FakeDealer(dealersDeck, playersDeck),
                    CreateConsoleWithSetupSequence(
                        askUnsignedCallbackSequence: askUnsignedSequence,
                        askStringCallbackSequence: askStringSequence),
                    new FakeGameClock());

                application.Run();
            });

            test.Wait(TimeSpan.FromSeconds(1.0d));

            // Assert. Player should have lost because he busted.
            Assert.Equal(500u, application.ActivePlayers.First().Wallet.Balance);
        }

        [Fact()]
        public void ExitTest()
        {
            // SUT.
            BlackjackApplication application = null;

            // Act. Allow the application to run
            // for a while.
            var test = Task.Run(() =>
            {
                // Arrange.
                var dealersDeck = new Card[]
                {
                    new Card(CardFace.King, CardSuit.Hearts),
                    new Card(CardFace.Seven, CardSuit.Hearts)
                };

                var playersDeck = new Card[]
                {
                    new Card(CardFace.Seven, CardSuit.Hearts),
                    new Card(CardFace.Eight, CardSuit.Diamonds),
                    new Card(CardFace.Ten, CardSuit.Clubs)
                };

                var askUnsignedSequence = new TryAskCallback<uint>[]
                {
                    // Asking for bet.
                    (out uint i) => i = 1000u,
                };

                var askStringSequence = new TryAskCallback<string>[]
                {
                    // Ask if you are going to play.
                    (out string s) => s = "y",

                    // Ask for action, hit 3 times.
                    (out string s) => s = "hit",
                    (out string s) => s = "hit",
                    (out string s) => s = "hit",
                };

                application = new BlackjackApplication(
                    new FakeDealer(dealersDeck, playersDeck),
                    CreateConsoleWithSetupSequence(
                        askUnsignedCallbackSequence: askUnsignedSequence,
                        askStringCallbackSequence: askStringSequence),
                    new FakeGameClock());

                application.Run();
            });

            test.Wait(TimeSpan.FromSeconds(1.0d));

            // Assert. Game should exit when no players are active.
            Assert.Empty(application.ActivePlayers);

            Assert.NotEmpty(application.AbsentPlayers);
        }
    }
}

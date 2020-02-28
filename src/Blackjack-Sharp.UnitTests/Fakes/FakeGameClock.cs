using System;

namespace Blackjack_Sharp.UnitTests.Fakes
{
    /// <summary>
    /// Fake game clock that does not do actual delaying.
    /// </summary>
    public sealed class FakeGameClock : IGameClock
    {
        public FakeGameClock()
        {
        }

        public void Delay(TimeSpan delay)
        {
            // NOP.
        }
    }
}

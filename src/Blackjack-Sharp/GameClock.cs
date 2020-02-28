using System;
using System.Threading;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Default implementation for <see cref="IGameClock"/>. 
    /// </summary>
    public sealed class GameClock : IGameClock
    {
        public GameClock()
        {
        }

        /// <summary>
        /// Delays the game by putting the calling thread to sleep
        /// for given amount of time.
        /// </summary>
        public void Delay(TimeSpan delay)
            => Thread.Sleep(delay);
    }
}

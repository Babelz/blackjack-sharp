using System;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Interface for implementing game clocks.
    /// </summary>
    public interface IGameClock
    {
        /// <summary>
        /// Delays the game by given amount of time.
        /// </summary>
        void Delay(TimeSpan delay);
    }
}

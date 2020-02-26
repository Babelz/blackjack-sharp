using System;

namespace Blackjack_Sharp
{
    /// <summary>
    /// Class that provides blackjack related operations and business logic.
    /// </summary>
    public sealed class BlackjackTable
    {
        #region Fields
        private readonly BlackjackConsole console;
        #endregion

        public BlackjackTable(BlackjackConsole console)
            => this.console = console ?? throw new ArgumentNullException(nameof(console));
        
        public int AskPlayersCount()
        {
            throw new NotImplementedException();
        }

        public string AskPlayerName(int playerIndex)
        {
            throw new NotImplementedException();
        }

        public bool ShouldAskPlayer(Player player, Hand hand)
        {
            throw new NotImplementedException();
        }

        public PlayerOperations AskPlayer(Player player, Hand hand)
        {
            throw new NotImplementedException();
        }

        public int AskBet(Player player, Hand hand)
        {
            throw new NotImplementedException();
        }

        public void Deal(BlackjackDealer dealer, Hand hand, int count)
        {
            throw new NotImplementedException();
        }

        public void RevealSecondCard(Hand hand)
        {
            throw new NotImplementedException();
        }

        public void RevealFirstCard(Hand hand)
        {
            throw new NotImplementedException();
        }

        public void RevealCards(Hand hand)
        {
            throw new NotImplementedException();
        }

        public bool Split(Player hand)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack_Sharp.UnitTests.Fakes
{
    // Delegates used for faking purposes.
    public delegate void WriteCallback(string line);
    public delegate void TryAskCallback<T>(out T value);
    public delegate void WritePlayerCallback(string name, string line);

    /// <summary>
    /// Fake console class for testing purposes.
    /// </summary>
    public sealed class FakeBlackjackConsole : IBlackjackConsole
    {
        #region Fields
        private readonly Queue<TryAskCallback<uint>> askUnsignedCallbackSequence;
        private readonly Queue<TryAskCallback<int>> askSignedCallbackSequence;
        private readonly Queue<TryAskCallback<string>> askStringCallbackSequence;

        private readonly Queue<WriteCallback> writeCallbackSequence;
        private readonly Queue<WritePlayerCallback> writePlayerCallbackSequence;
        #endregion

        public FakeBlackjackConsole(IEnumerable<TryAskCallback<uint>> askUnsignedCallbackSequence = null,
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

            this.askUnsignedCallbackSequence =
                new Queue<TryAskCallback<uint>>(askUnsignedCallbackSequence);
            
            this.askSignedCallbackSequence =
                new Queue<TryAskCallback<int>>(askSignedCallbackSequence);

            this.askStringCallbackSequence =
                new Queue<TryAskCallback<string>>(askStringCallbackSequence);

            this.writeCallbackSequence =
                new Queue<WriteCallback>(writeCallbackSequence);

            this.writePlayerCallbackSequence =
                new Queue<WritePlayerCallback>(writePlayerCallbackSequence);
        }

        public bool TryAskLine(string what, out string value, Func<string, bool> validation = null)
        {
            value = default;

            if (askStringCallbackSequence.Count == 0) return false;

            askStringCallbackSequence.Dequeue()(out value);

            return true;
        }

        public bool TryAskSigned(string what, out int value, Func<int, bool> validation = null)
        {
            value = default;

            if (askSignedCallbackSequence.Count == 0) return false;

            askSignedCallbackSequence.Dequeue()(out value);

            return true;
        }

        public bool TryAskUnsigned(string what, out uint value, Func<uint, bool> validation = null)
        {
            value = default;

            if (askUnsignedCallbackSequence.Count == 0) return false;

            askUnsignedCallbackSequence.Dequeue()(out value);

            return true;
        }

        public void WriteDealerInfo(string line)
        {
            if (writeCallbackSequence.Count == 0) return;

            writeCallbackSequence.Dequeue()(line);
        }

        public void WriteLine(string line)
            => WriteDealerInfo(line);

        public void WriteWarning(string line)
            => WriteDealerInfo(line);

        public void WritePlayerInfo(string name, string line)
        {
            if (writePlayerCallbackSequence.Count == 0) return;

            writePlayerCallbackSequence.Dequeue()(name, line);
        }

        public void WriteSeparator()
        {
            // NOP.
        }
    }
}

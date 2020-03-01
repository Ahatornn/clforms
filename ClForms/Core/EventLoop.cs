using System;
using System.Collections.Concurrent;
using System.Threading;
using ClForms.Abstractions.Core;

namespace ClForms.Core
{
    /// <summary>
    /// Message processing cycle
    /// </summary>
    internal class EventLoop: IEventLoop
    {
        private readonly int loopWaitTimeout;
        private readonly ConcurrentQueue<Action> actionQueue;
        private readonly ManualResetEventSlim wait;
        private Action<ConsoleKeyInfo> inputAction;

        public EventLoop(int loopWaitTimeout)
        {
            this.loopWaitTimeout = loopWaitTimeout;
        }

        /// <inheritdoc />
        public bool Running { get; private set; }

        /// <summary>
        /// Initialize a new instance <see cref="EventLoop"/>
        /// </summary>
        public EventLoop()
        {
            actionQueue = new ConcurrentQueue<Action>();
            wait = new ManualResetEventSlim(false);
        }

        public event EventLoopEmpty OnLoopEmpty;

        /// <inheritdoc />
        public void Enqueue(Action action)
        {
            actionQueue.Enqueue(action);
            wait.Set();
        }

        /// <inheritdoc />
        public void Start(Action<ConsoleKeyInfo> action)
        {
            inputAction = action;
            Running = true;
            Loop();
        }

        /// <inheritdoc />
        public void Stop()
        {
            Running = false;
            wait.Set();
        }

        /// <inheritdoc />
        public void Clear()
        {
            actionQueue.Clear();
            wait.Set();
        }

        /// <inheritdoc />
        public int Length => actionQueue.Count;

        /// <inheritdoc />
        public void ProcessIteration() => LoopStep();

        private void Loop()
        {
            while (Running)
            {
                LoopStep();
            }
        }

        private void LoopStep()
        {
            if (actionQueue.TryDequeue(out var action))
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Console.SetCursorPosition(0, Console.WindowHeight);
                    Console.WriteLine(ex);
                    Running = false;
                }
            }
            if (inputAction == null || !Console.KeyAvailable)
            {
                wait.Reset();
                wait.Wait(loopWaitTimeout);
            }
            else
            {
                inputAction.Invoke(Console.ReadKey(true));
            }

            if (actionQueue.Count == 0)
            {
                OnLoopEmpty?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

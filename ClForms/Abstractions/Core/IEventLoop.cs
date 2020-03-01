using System;

namespace ClForms.Abstractions.Core
{
    /// <summary>
    /// Event queue processing
    /// </summary>
    public interface IEventLoop
    {
        /// <summary>
        /// Gets <see langword="true"/> if current loop was started
        /// </summary>
        bool Running { get; }

        /// <summary>
        /// Queue empty event
        /// </summary>
        event EventLoopEmpty OnLoopEmpty;

        /// <summary>
        /// Add an action to the event queue
        /// </summary>
        /// <param name="action">Action for processing by queue</param>
        void Enqueue(Action action);

        /// <summary>
        /// Start queue processing
        /// </summary>
        /// <param name="action">Keyboard input processing action</param>
        void Start(Action<ConsoleKeyInfo> action);

        /// <summary>
        /// Stop queue processing
        /// </summary>
        void Stop();

        /// <summary>
        /// Remove all actions from queue
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets actions count in queue
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Processing current action in queue
        /// </summary>
        void ProcessIteration();
    }

    /// <summary>
    /// Queue emptying delegate
    /// </summary>
    public delegate void EventLoopEmpty(object sender, EventArgs e);
}

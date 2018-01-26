using System;

namespace CSharpSnippets.CSharpLanguage.WeakEvents
{
    internal class EventSource
    {
        public event EventHandler<SignalArgs> Signal;

        public void GiveSignal(string name)
        {
            Console.WriteLine($"EventSource emitting: {name}");
            Signal?.Invoke(this, new SignalArgs {Name = name});
        }

        ~EventSource()
        {
            Console.WriteLine("EventSource finalised");
        }
    }
}
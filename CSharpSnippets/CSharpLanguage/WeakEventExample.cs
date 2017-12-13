using System;
using System.Collections.Generic;
using System.Windows;

namespace CSharpSnippets.CSharpLanguage
{
    class WeakEventExample
    {
        public static void Run()
        {
            var source = new EventSource();

            // subscribe 2 listeners to source
            var subscribers = new List<EventSubscriber>();

            for (var i = 0; i < 10; i++)
            {
                var sub = new EventSubscriber { Name = "Sub" + i };
                subscribers.Add(sub);
                sub.Subscribe(source);
            };

            // show subscription in action
            source.GiveSignal("blah!");

            // done. remove subscribers
            subscribers.Clear();
            GC.Collect();

            // put a breakpoint here. Are we done? Are the subscribers gone?
            Console.WriteLine("done");
        }
    }

    class EventSource
    {
        public void GiveSignal(string name)
        {
            Console.WriteLine($"EventSource emitting: {name}");
            Signal(this, new SignalArgs { Name = name });
        }

        public event EventHandler<SignalArgs> Signal;
    }

    class SignalArgs : EventArgs
    {
        public string Name { get; set; }
    }

    class EventSubscriber
    {
        public string Name { get; set; }

        public void Subscribe(EventSource source)
        {
            //source.Signal += Respond;
            WeakEventManager<EventSource, SignalArgs>.AddHandler(source, nameof(source.Signal), new EventHandler<SignalArgs> (Respond));
        }

        public void Unsubscribe(EventSource source)
        {
            source.Signal -= Respond;
        }

        public void Respond(object sender, SignalArgs args)
        {
            Console.WriteLine($"Subscriber {Name} received: {args.Name}");
        }
    }
}

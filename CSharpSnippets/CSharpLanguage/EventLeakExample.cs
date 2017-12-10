using System;
using System.Collections.Generic;

namespace CSharpSnippets.CSharpLanguage
{
    class EventLeakExample
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

            // TODO: Uncomment this after examining the heap below
            // unsubscribe from event source
            foreach (var sub in subscribers)
            {
                sub.Unsubscribe(source);
            }

            // done. remove subscribers
            subscribers.Clear();
            subscribers = null;

            // put a breakpoint here. Are we done? Are the subscribers gone?
            // NOTE: one subscriber hangs around, possibly due to the debugger, or 
            //       a local variable still holds a reference to it
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

    class SignalArgs
    {
        public string Name { get; set; }
    }

    class EventSubscriber
    {
        public string Name { get; set; }

        public void Subscribe(EventSource source)
        {
            source.Signal += Respond;
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

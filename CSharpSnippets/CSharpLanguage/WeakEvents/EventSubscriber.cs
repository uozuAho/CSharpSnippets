using System;
using System.Windows;

namespace CSharpSnippets.CSharpLanguage.WeakEvents
{
    internal class EventSubscriber
    {
        public string Name { get; set; }

        public void SubscribeWithPlusEquals(EventSource source)
        {
            source.Signal += Respond;
        }

        public void SubscribeWithWeakEventManager(EventSource source)
        {
            WeakEventManager<EventSource, SignalArgs>.AddHandler(source, nameof(source.Signal), new EventHandler<SignalArgs> (Respond));
        }

        public void UnsubscribeMinusEquals(EventSource source)
        {
            source.Signal -= Respond;
        }

        public void Respond(object sender, SignalArgs args)
        {
            Console.WriteLine($"Subscriber {Name} received: {args.Name}");
        }

        ~EventSubscriber()
        {
            Console.WriteLine($"EventSubscriber '{Name}' finalised");
        }
    }
}
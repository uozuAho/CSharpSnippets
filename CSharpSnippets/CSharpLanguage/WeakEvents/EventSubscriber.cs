using System;
using System.Windows;

namespace CSharpSnippets.CSharpLanguage.WeakEvents
{
    internal class EventSubscriber
    {
        public string Name { get; set; }

        private WeakEventListener<EventSubscriber, EventSource, SignalArgs> _weakListener;

        public void SubscribeWithPlusEquals(EventSource source)
        {
            source.Signal += Respond;
        }

        public void SubscribeWithWeakEventManager(EventSource source)
        {
            WeakEventManager<EventSource, SignalArgs>.AddHandler(source, nameof(source.Signal), Respond);
        }

        public void SubscribeWithWeakEventListener(EventSource source)
        {
            _weakListener = new WeakEventListener<EventSubscriber, EventSource, SignalArgs>(this, source);
            source.Signal += _weakListener.OnEvent;
            _weakListener.OnEventAction = Respond;
            _weakListener.OnDetachAction = UnsubscribeWeakListener;
        }

        public void UnsubscribeMinusEquals(EventSource source)
        {
            source.Signal -= Respond;
        }

        public void Respond(object sender, SignalArgs args)
        {
            Console.WriteLine($"Subscriber {Name} received: {args.Name}");
        }

        public static void Respond(EventSubscriber sub, object sender, SignalArgs args)
        {
            sub.Respond(sender, args);
        }

        public static void UnsubscribeWeakListener(WeakEventListener<EventSubscriber, EventSource, SignalArgs> listener,
            EventSource source)
        {
            source.Signal -= listener.OnEvent;
            Console.WriteLine("WeakEventListener unsubscribed");
        }

        ~EventSubscriber()
        {
            Console.WriteLine($"EventSubscriber '{Name}' finalised");
        }
    }
}
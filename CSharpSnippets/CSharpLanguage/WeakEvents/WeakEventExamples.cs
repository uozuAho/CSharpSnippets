using System;

namespace CSharpSnippets.CSharpLanguage.WeakEvents
{
    internal class WeakEventExamples
    {
        public static void Run()
        {
            RunExample("Classic event handler leak example", EventLeak);
            RunExample("Fixed event leak by unsubscribing", EventLeakFixedByUnsubscribing);
            RunExample("Fixed event leak with weak manager", EventLeakFixedByWeakEventManager);
            RunExample("Fixed event leak with weak listener", EventLeakFixedByWeakEventListener);
        }

        private static void EventLeak()
        {
            var source = new EventSource();
            var subscriber = new EventSubscriber {Name = "leaky subscriber"};
            subscriber.SubscribeWithPlusEquals(source);

            Console.WriteLine("Show event subscription in action");
            source.GiveSignal("blah!");

            Console.WriteLine("Done. Remove subscriber. Naive dev will expect to see finaliser run here.");
            subscriber = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Are we done? Was the subscriber destroyed? No!");

            Console.WriteLine("The subscriber is held on to by the source until it is destroyed");
            Console.WriteLine("Destroying souce now...");
            source = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static void EventLeakFixedByUnsubscribing()
        {
            var source = new EventSource();
            var subscriber = new EventSubscriber {Name = "diligent subscriber"};
            subscriber.SubscribeWithPlusEquals(source);

            Console.WriteLine("Show event subscription in action");
            source.GiveSignal("blah!");

            Console.WriteLine("Done. Unsubscribe from source and remove subscriber.");
            Console.WriteLine("The missing unsubscribe action was what caused the classic leak");
            subscriber.UnsubscribeMinusEquals(source);
            subscriber = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Are we done? Was the subscriber destroyed?");
        }

        private static void EventLeakFixedByWeakEventManager()
        {
            var source = new EventSource();
            var subscriber = new EventSubscriber {Name = "weak subscriber"};
            subscriber.SubscribeWithWeakEventManager(source);

            Console.WriteLine("Show event subscription in action");
            source.GiveSignal("blah!");

            Console.WriteLine("Done. Remove subscriber.");
            Console.WriteLine("No need to unsubscribe, since weak reference to subscriber");
            Console.WriteLine("allows the subscriber to be collected");
            subscriber = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Are we done? Was the subscriber destroyed?");
        }

        private static void EventLeakFixedByWeakEventListener()
        {
            var source = new EventSource();
            var subscriber = new EventSubscriber {Name = "subscriber"};
            subscriber.SubscribeWithWeakEventListener(source);

            Console.WriteLine("Show event subscription in action");
            source.GiveSignal("blah!");

            Console.WriteLine("Done. Remove subscriber.");
            Console.WriteLine("No need to unsubscribe, since weak listener");
            Console.WriteLine("allows the subscriber to be collected");
            subscriber = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Are we done? Was the subscriber destroyed? What about the listener?");
            Console.WriteLine("The weak listener is held onto by the source, however this isn't");
            Console.WriteLine("too bad since the listener doesn't take up much space.");
            Console.WriteLine("");
            Console.WriteLine("Let's kill the source now");
            source = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("");
            Console.WriteLine("The weak listener will also be cleaned up if the event fires after");
            Console.WriteLine("the subscriber has been destroyed. This will only happen if you've");
            Console.WriteLine("configured the 'OnDetachAction'");
            source = new EventSource();
            subscriber = new EventSubscriber {Name = "subscriber"};
            subscriber.SubscribeWithWeakEventListener(source);
            Console.WriteLine("Show event subscription in action");
            source.GiveSignal("blah!");

            Console.WriteLine("Destroy the subscriber, then fire the event again. Is the weak listener finalised?");
            
            subscriber = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            source.GiveSignal("blah!");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("");
            Console.WriteLine("Note that this is a drawback of this listener - the source event");
            Console.WriteLine("must occur to clean up the weak listener. We could call Detach(),");
            Console.WriteLine("but that defeats the purpose - why not use regular events if you");
            Console.WriteLine("know when to unsubscribe?");
        }

        private static void RunExample(string description, Action example)
        {
            Console.WriteLine();
            Console.WriteLine("====================================================");
            Console.WriteLine(description);
            example();
            Console.WriteLine("====================================================");
        }
    }
}

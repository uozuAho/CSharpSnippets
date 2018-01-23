using System;

namespace CSharpSnippets.CSharpLanguage.WeakEvents
{
    internal class WeakEventExamples
    {
        public static void Run()
        {
            RunExample("Classic event handler leak example", EventLeak);
            Console.WriteLine("Note that the leaky subscriber is now cleaned up since the source is destroyed");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            RunExample("Fixed classic event leak by unsubscribing", EventLeakFixedByUnsubscribing);
            RunExample("Fixed classic event leak with weak manager", EventLeakFixedByWeakEventManager);
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

            Console.WriteLine("Are we done? Was the subscriber destroyed?");
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

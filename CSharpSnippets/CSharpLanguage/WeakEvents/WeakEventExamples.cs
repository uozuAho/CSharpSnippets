using System;

namespace CSharpSnippets.CSharpLanguage.WeakEvents
{
    internal class WeakEventExamples
    {
        public static void Run()
        {
            EventLeak();
        }

        private static void EventLeak()
        {
            Console.WriteLine("Classic event handler leak example");

            var source = new EventSource();
            var subscriber = new EventSubscriber {Name = "Subscriber"};
            subscriber.SubscribeWithPlusEquals(source);

            Console.WriteLine("Show event subscription in action");
            source.GiveSignal("blah!");

            Console.WriteLine("Done. Remove subscriber. Naive dev will expect to see finaliser run here.");
            subscriber = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Are we done? Was the subscriber destroyed?");
        }
    }
}

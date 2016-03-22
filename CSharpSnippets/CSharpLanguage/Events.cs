using System;

namespace CSharpSnippets.CSharpLanguage
{
    class Events
    {
        public class Metronome
        {
            public event TickHandler TickHandler1;
            public EventArgs e = null;
            public delegate void TickHandler(Metronome m, EventArgs e);

            public void Start()
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Metronome: tick");
                    if (TickHandler1 != null)
                    {
                        TickHandler1(this, e);
                    }
                }
            }
        }

        public class Listener
        {
            public string Name { get; set; }

            public Listener(string name)
            {
                Name = name;
            }

            public void Subscribe(Metronome m)
            {
                m.TickHandler1 += new Metronome.TickHandler(HeardIt);
            }

            private void HeardIt(Metronome m, EventArgs e)
            {
                Console.WriteLine(Name + ": HEARD IT");
            }
        }

        public static void Run()
        {
            Metronome m = new Metronome();
            Listener l1 = new Listener("listener 1");
            Listener l2 = new Listener("listener 2");
            l1.Subscribe(m);
            l2.Subscribe(m);
            m.Start();
        }
    }
}

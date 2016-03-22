using NUnit.Framework;
using System;

namespace CSharpSnippets.Test
{
    [TestFixture]
    public class BlahTest
    {
        [Test]
        public void DoSomething()
        {
            // TODO: VS test adapter not finding this yet. 
            // Early days for the adapter apparently.
            Console.WriteLine("mah");
        }
    }
}

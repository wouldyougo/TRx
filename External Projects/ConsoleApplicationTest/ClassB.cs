using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest
{
    class ClassB: ClassA
    {
        override public void Method1()
        {
            Console.WriteLine("B Method1");
        }
        new public void Method2()
        {
            Console.WriteLine("B Method2");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest
{
    class ClassA
    {
        public void Do()
        {
            Console.WriteLine("A Do");
            Method1();
            Method2();
        }
        virtual public void Method1()
        {
            Console.WriteLine("A Method1");
        }
        virtual public void Method2()
        {
            Console.WriteLine("A Method2");
        }
    }
}

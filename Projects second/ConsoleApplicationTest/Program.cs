using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassB B = new ClassB();
            B.Do();
            Console.WriteLine(String.Format("Press 'x' to exit..."));
            while (Console.ReadKey().KeyChar != 'x')
            {
                Console.WriteLine(String.Format("Press 'x' to exit..."));
            }
        }
    }
}

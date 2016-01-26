using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteService.Tests.Database
{
    abstract class Thing
    {
        protected static String s = "";
        protected Thing()
        {
            s += "t ";
        }
    }

    class Steel : Thing
    {
        protected Steel()
        {
            s += "s ";
        }

        public Steel(string s1)
        {
            s += s1;
            new Steel();
        }

    }

    class Tungsten : Steel
    {
        Tungsten(String s1)
        {
            s += s1;
            new Steel(s);
        }

        public static void Main()
        {
            new Tungsten("tu ");
            Console.Out.WriteLine(s);
        }
    }

    class C
    {
        static C()
        {
            Console.WriteLine("I am in Static C");
        }
        public C()
        {
            Console.WriteLine("I am in C");
        }
    }
}

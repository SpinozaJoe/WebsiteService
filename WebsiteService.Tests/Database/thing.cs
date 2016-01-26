using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteService.Tests.Database
{
    public class StaticConstructorTest
    {
        static StaticConstructorTest()
        {
            throw new Exception();
        }

        static string Work(string s)
        {
            return "tested";
        }

        public StaticConstructorTest()
        {
        }
    }

}

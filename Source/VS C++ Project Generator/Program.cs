using System;
using VS_CPP_Project_Generator.Logging;

namespace VS_CPP_Project_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new ConsoleLogger();
            logger.WriteLine("Hello World!");
        }
    }
}

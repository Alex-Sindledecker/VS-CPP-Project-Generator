using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string content)
        {
            Console.Write(content);
        }

        public void WriteLine(string content)
        {
            Console.WriteLine(content);
        }
    }
}

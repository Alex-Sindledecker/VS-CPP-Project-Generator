using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.IOStreams
{
    public class ConsoleIOStream : IIOStream
    {
        public void Write(string content)
        {
            Console.Write(content);
        }

        public void WriteLine(string content)
        {
            Console.WriteLine(content);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}

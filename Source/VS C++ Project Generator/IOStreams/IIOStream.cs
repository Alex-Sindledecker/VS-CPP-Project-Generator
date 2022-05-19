using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.IOStreams
{
    public interface IIOStream
    {
        public void WriteLine(string content);
        public void Write(string content);
        public string ReadLine();
    }
}

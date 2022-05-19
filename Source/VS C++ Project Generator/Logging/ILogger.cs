using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Logging
{
    public interface ILogger
    {
        public void WriteLine(string content);
        public void Write(string content);
    }
}

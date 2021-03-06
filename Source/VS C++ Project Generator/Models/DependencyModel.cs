using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Models
{
    public class DependencyModel
    {
        public delegate void PostOpDelegate(string sourceDirectory);

        public string Url { get; set; }
        public string IncludeDir { get; set; } //Where source files are added
        public string LibDir { get; set; } //Where .lib files are located (optional)
        public string DllDir { get; set; } //Where .dlls files are locationed (optional)
        public List<string> DebugLibNames { get; set; } //Library names for a debug config
        public List<string> ReleaseLibNames { get; set; } //Library names for a release config
        public List<string> IncludeInProject { get; set; } //List of files to include in the project

        public PostOpDelegate PostOp { get; set; } //Optional field - if set, it is called at the end of project generation. This can be used to do template specific operations
    }
}

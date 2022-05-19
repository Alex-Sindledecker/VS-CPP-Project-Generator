using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Models
{
    public class LibraryModel
    {
        public string Url;
        public string IncludeDir; //Where source files are added
        public string LibDir; //Where .lib files are located (optional)
        public string DllDir; //Where .dlls files are locationed (optional)
        public bool Submodule; //Add as a submodule (this only works if the url links to a github repo)
        public List<string> libNames;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Models
{
    public class ProjectModel
    {
        public string Name;
        public string DiskLocation;
        public string GitRepo; //If you want to create the project and add it to an existing git repo
        public List<LibraryModel> libraries;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Models
{
    public class ProjectModel
    {
        public string Name { get; set; }
        public string DiskLocation { get; set; }
        public string GitRepo { get; set; } //If you want to create the project and add it to an existing git repo
        public List<DependencyModel> Dependencies { get; set; } = new List<DependencyModel>();
    }
}

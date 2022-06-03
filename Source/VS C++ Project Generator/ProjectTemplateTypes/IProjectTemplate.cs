using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.ProjectTemplateTypes
{
    public interface IProjectTemplate
    {
        public string Name { get; }
        public void PopulateProjectModel(ProjectModel model);
    }
}

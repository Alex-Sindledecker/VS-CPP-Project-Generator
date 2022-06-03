using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Models.ModelGenerators;
using VS_CPP_Project_Generator.ProjectAssembly;

namespace VS_CPP_Project_Generator.ProjectTemplateTypes
{
    [ProjectTemplate]
    public class SFMLProjectTemplate : IProjectTemplate
    {
        public string Name => "SFML";

        public void PopulateProjectModel(ProjectModel model)
        {
            model.TemplateSourcePath = $"{PathTools.GetTemplateRootPath()}SFMLSource/";
            model.Dependencies.Add(DependencyModelGenerator.GetSFMLModel());
        }
    }
}

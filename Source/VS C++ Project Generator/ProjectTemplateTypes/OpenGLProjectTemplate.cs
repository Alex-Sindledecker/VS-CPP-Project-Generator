﻿using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Models.ModelGenerators;
using VS_CPP_Project_Generator.ProjectAssembly;

namespace VS_CPP_Project_Generator.ProjectTemplateTypes
{
    [ProjectTemplate]
    public class OpenGLProjectTemplate : IProjectTemplate
    {
        public string Name => "OpenGL";

        public void PopulateProjectModel(ProjectModel model)
        {
            model.TemplateSourcePath = $"{PathTools.GetTemplateRootPath()}OpenGLSource/";
            model.Dependencies.Add(DependencyModelGenerator.GetGLADModel());
            model.Dependencies.Add(DependencyModelGenerator.GetGLFWModel());
            model.Dependencies.Add(DependencyModelGenerator.GetGLMModel());
        }
    }
}

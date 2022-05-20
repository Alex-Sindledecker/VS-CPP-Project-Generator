using System;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Models.ModelGenerators;
using VS_CPP_Project_Generator.Prompts;

namespace VS_CPP_Project_Generator
{
    class App
    {
        static void Main(string[] args)
        {
            ProjectModel model = GetProjectModel();
        }

        static ProjectModel GetProjectModel()
        {
            ProjectModelGenerator projectModelGenerator = new ProjectModelGenerator();
            DependencyModelGenerator dependencyModelGenerator = new DependencyModelGenerator();

            projectModelGenerator.AddPrompt(new ProjectNamePrompt());
            projectModelGenerator.AddPrompt(new DiskLocationPrompt());
            projectModelGenerator.AddPrompt(new ProjectTypePrompt());

            dependencyModelGenerator.AddPrompt(new DependencyURLPrompt());
            dependencyModelGenerator.AddPrompt(new DependencyDirectoryPrompt(DependencyDirectoryType.Include));
            dependencyModelGenerator.AddPrompt(new DependencyDirectoryPrompt(DependencyDirectoryType.Library));
            dependencyModelGenerator.AddPrompt(new DependencyDirectoryPrompt(DependencyDirectoryType.Dll));
            dependencyModelGenerator.AddPrompt(new LibraryNamesPrompt(DependencyLibraryConfiguration.Debug));
            dependencyModelGenerator.AddPrompt(new LibraryNamesPrompt(DependencyLibraryConfiguration.Release));

            ProjectModel model = projectModelGenerator.RunPrompts();

            ConditionalPrompt conditionalPrompt = new ConditionalPrompt("Add another dependency?");
            PromptCommon.RunPrompt(conditionalPrompt);
            while (conditionalPrompt.Result == true)
            {
                DependencyModel dependency = dependencyModelGenerator.RunPrompts();
                model.Dependencies.Add(dependency);

                PromptCommon.RunPrompt(conditionalPrompt);
            }

            return model;
        }
    }
}

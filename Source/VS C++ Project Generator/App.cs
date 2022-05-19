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
            DependencyURLPrompt dependencyURLPrompt = new DependencyURLPrompt();
            bool test1 = dependencyURLPrompt.Validate("https://github.com/Alex-Sindledecker/VS-CPP-Project-Generator/tree/development");
            bool test2 = dependencyURLPrompt.Validate("https://github.com/canton7/Stylet");
            bool test3 = dependencyURLPrompt.Validate("https://github.com/Alex-Sindledecker/VS-CPP-Project-Generator");
            bool test4 = dependencyURLPrompt.Validate("https://www.w3schools.com/cs/cs_user_input.php");
            bool test5 = dependencyURLPrompt.Validate("https://github.com/SFML/SFML/releases/download/2.5.1/SFML-2.5.1-windows-vc15-64-bit.zip");

            ProjectModel model = GetProjectModel();
        }

        static ProjectModel GetProjectModel()
        {
            ProjectModelGenerator projectModelGenerator = new ProjectModelGenerator();
            DependencyModelGenerator dependencyModelGenerator = new DependencyModelGenerator();

            projectModelGenerator.AddPrompt(new ProjectNamePrompt());
            projectModelGenerator.AddPrompt(new DiskLocationPrompt());
            projectModelGenerator.AddPrompt(new ProjectTypePrompt());

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

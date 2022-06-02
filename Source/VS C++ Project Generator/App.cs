using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Models.ModelGenerators;
using VS_CPP_Project_Generator.Prompts;
using VS_CPP_Project_Generator.ProjectAssembly;
using System.Diagnostics;
using System;

namespace VS_CPP_Project_Generator
{
    class App
    {
        //Entry Point
        static void Main(string[] args)
        {
            //Ask user for project information in terminal window
            ProjectModel model = GetProjectModel();

            //Create the project on the users system
            ProjectBuilder projectBuilder = new ProjectBuilder("v142", "12.00");

            projectBuilder.DirectoryBuildEvent += () => PromptCommon.WriteLine("\tBuilding directories...", ConsoleColor.DarkGreen);
            projectBuilder.MoveTemplateEvent += () => PromptCommon.WriteLine("\tMoving template files...", ConsoleColor.DarkGreen);
            projectBuilder.ProjFileBuildEvent += () => PromptCommon.WriteLine("\tCreating proj files...", ConsoleColor.DarkGreen);
            projectBuilder.SlnFileBuildEvent += () => PromptCommon.WriteLine("\tCreating sln files...", ConsoleColor.DarkGreen);

            DependencyManager.DependencyAqusationEvent += (int current, int total) => {
                PromptCommon.Write("\tDownloading dependencies... [", ConsoleColor.DarkGreen);
                Console.Write($"{current}/{total}"); 
                PromptCommon.WriteLine("]", ConsoleColor.DarkGreen);
            };

            DependencyManager.DependencyExtractingEvent += (int current, int total) => {
                PromptCommon.Write("\tExtracting dependencies... [", ConsoleColor.DarkGreen);
                Console.Write($"{current}/{total}"); 
                PromptCommon.WriteLine("]", ConsoleColor.DarkGreen);
            };

            projectBuilder.BuildFromModel(model);

            //Open the folder with the newly created project
            Process.Start("explorer.exe", $"{model.DiskLocation.Replace('/', '\\')}Source\\");
        }

        //Ask user for project information in terminal window
        static ProjectModel GetProjectModel()
        {
            //Generators
            ProjectModelGenerator projectModelGenerator = new ProjectModelGenerator();
            DependencyModelGenerator dependencyModelGenerator = new DependencyModelGenerator();

            //Adding project prompts
            projectModelGenerator.AddPrompt(new ProjectNamePrompt());
            projectModelGenerator.AddPrompt(new DiskLocationPrompt());
            projectModelGenerator.AddPrompt(new ProjectTypePrompt());

            //Adding dependency prompts
            dependencyModelGenerator.AddPrompt(new DependencyURLPrompt());
            dependencyModelGenerator.AddPrompt(new DependencyDirectoryPrompt(DependencyDirectoryType.Include));
            dependencyModelGenerator.AddPrompt(new DependencyDirectoryPrompt(DependencyDirectoryType.Library));
            dependencyModelGenerator.AddPrompt(new DependencyDirectoryPrompt(DependencyDirectoryType.Dll));
            dependencyModelGenerator.AddPrompt(new LibraryNamesPrompt(DependencyLibraryConfiguration.Debug));
            dependencyModelGenerator.AddPrompt(new LibraryNamesPrompt(DependencyLibraryConfiguration.Release));
            dependencyModelGenerator.AddPrompt(new IncludeFilesPrompt());

            //Run the project prompts - This gets input from the user to create a project model
            ProjectModel model = projectModelGenerator.RunPrompts();

            //If the user wants to add custom dependencies, they do it through the dependency prompts added above. They can add as many as they want
            ConditionalPrompt conditionalPrompt = new ConditionalPrompt("Add another dependency?");
            PromptCommon.RunPrompt(conditionalPrompt);

            while (conditionalPrompt.Result == true)
            {
                //Run dependencies prompts
                DependencyModel dependency = dependencyModelGenerator.RunPrompts();
                model.Dependencies.Add(dependency);

                PromptCommon.RunPrompt(conditionalPrompt);
            }

            return model;
        }
    }
}

using System;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Models.ModelGenerators;
using VS_CPP_Project_Generator.Prompts;

namespace VS_CPP_Project_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            ProjectModelGenerator projectModelGenerator = new ProjectModelGenerator();

            //projectModelGenerator.AddPrompt(new ProjectNamePrompt());
            //projectModelGenerator.AddPrompt(new DiskLocationPrompt());
            //projectModelGenerator.AddPrompt(new ProjectTypePrompt());

            projectModelGenerator.RunPrompts();

            Console.WriteLine(projectModelGenerator.Model.DiskLocation);
        }
    }
}

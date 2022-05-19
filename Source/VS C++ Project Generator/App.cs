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
            //ProjectModelGenerator projectModelGenerator = new ProjectModelGenerator();

            //projectModelGenerator.AddPrompt(new ProjectNamePrompt());
            //projectModelGenerator.AddPrompt(new DiskLocationPrompt());
            //projectModelGenerator.AddPrompt(new ProjectTypePrompt());
            
            //ProjectModel model = projectModelGenerator.RunPrompts();

            //Console.WriteLine(model.DiskLocation);

            ConditionalPrompt conditionalPrompt = new ConditionalPrompt("Add another dependency?");
            do
            {
                PromptCommon.RunPrompt(conditionalPrompt);
            }
            while (conditionalPrompt.Result == true);
        }
    }
}

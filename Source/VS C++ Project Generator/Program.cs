using System;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Prompts;

namespace VS_CPP_Project_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            IPrompt prompt = new ProjectTypePrompt();

            string userInput;
            prompt.Show();
            userInput = Console.ReadLine();
            while (prompt.Validate(userInput) != true)
            {
                prompt.ShowFailedValidationMessage();
                prompt.Show();
                userInput = Console.ReadLine();
            }

            ProjectModel projectModel = new ProjectModel();
            prompt.Populate(projectModel);
        }
    }
}

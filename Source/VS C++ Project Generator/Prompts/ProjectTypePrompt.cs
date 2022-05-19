using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Models.ModelGenerators;

namespace VS_CPP_Project_Generator.Prompts
{
    class ProjectTypePrompt : IProjectPrompt
    {
        private int _choice;

        public void Populate(ProjectModel model)
        {
            switch (_choice)
            {
                case 1:
                    model.Dependencies.Add(DependencyModelGenerator.GetSFMLModel());
                    break;
                case 2:
                    //model.dependencies.Add(DependencyGenerator.GetOpenGLModel());
                    break;
            }
        }

        public void Show()
        {
            Console.WriteLine("Select your project type from the list below: ");
            Console.WriteLine("1. SFML");
            Console.WriteLine("2. OpenGL");
            Console.Write("Enter a number: ");
        }

        public void ShowFailedValidationMessage()
        {
            Console.WriteLine("Invalid selection! Please try again...");
        }

        public bool Validate(string userInput)
        {
            if (int.TryParse(userInput, out _choice))
            {
                switch (_choice)
                {
                    case 1:
                        //SFML
                        return true;
                    case 2:
                        //OpenGL
                        return true;
                    default:
                        return false;
                }
            }

            return false;
        }
    }
}

using System;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Models.ModelGenerators;
using VS_CPP_Project_Generator.ProjectAssembly;

namespace VS_CPP_Project_Generator.Prompts
{
    class ProjectTypePrompt : IProjectPrompt
    {
        private string _templatePath;
        private int _choice;

        public void Populate(ProjectModel model)
        {
            switch (_choice)
            {
                case 1:
                    model.TemplateSourcePath = $"{_templatePath}SFMLSource/";
                    model.Dependencies.Add(DependencyModelGenerator.GetSFMLModel());
                    break;
                case 2:
                    model.TemplateSourcePath = $"{_templatePath}OpenGLSource/";
                    model.Dependencies.Add(DependencyModelGenerator.GetGLADModel());
                    model.Dependencies.Add(DependencyModelGenerator.GetGLFWModel());
                    model.Dependencies.Add(DependencyModelGenerator.GetGLMModel());
                    break;
                case 3:
                    model.TemplateSourcePath = $"{_templatePath}ImGuiSource/";
                    model.Dependencies.Add(DependencyModelGenerator.GetGLADModel());
                    model.Dependencies.Add(DependencyModelGenerator.GetGLFWModel());
                    model.Dependencies.Add(DependencyModelGenerator.GetGLMModel());
                    model.Dependencies.Add(DependencyModelGenerator.GetImGuiModel());
                    break;
            }
        }

        public ProjectTypePrompt()
        {
            _templatePath = PathTools.GetTemplateRootPath();
        }

        public void Show()
        {
            Console.WriteLine("Select your project type from the list below: ");
            Console.WriteLine("1. SFML");
            Console.WriteLine("2. OpenGL");
            Console.WriteLine("3. OpenGL (with imgui - glfw and opengl backend files (imgui/backends/) must be moved by hand)");
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
                if (_choice >= 1 && _choice <= 3)
                    return true;
            }

            return false;
        }
    }
}

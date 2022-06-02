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
            PromptCommon.WriteLine("Select your project type from the list below: ", ConsoleColor.DarkGray);
            Console.Write("1. "); PromptCommon.WriteLine("SFML", ConsoleColor.DarkYellow);
            Console.Write("2. "); PromptCommon.WriteLine("OpenGL", ConsoleColor.DarkYellow);
            Console.Write("3. "); PromptCommon.WriteLine("OpenGL (with imgui)", ConsoleColor.DarkYellow);
            PromptCommon.Write("Enter a number: ", ConsoleColor.DarkGray);
        }

        public void ShowFailedValidationMessage()
        {
            PromptCommon.WriteLine("Invalid selection! Please try again...", ConsoleColor.Red);
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

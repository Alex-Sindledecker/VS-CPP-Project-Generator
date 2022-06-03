using System;
using System.Collections.Generic;
using System.Linq;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Models.ModelGenerators;
using VS_CPP_Project_Generator.ProjectAssembly;
using VS_CPP_Project_Generator.ProjectTemplateTypes;

namespace VS_CPP_Project_Generator.Prompts
{
    class ProjectTypePrompt : IProjectPrompt
    {
        private string _templatePath;
        private int _choice;
        private List<IProjectTemplate> _projectTemplates;

        public void Populate(ProjectModel model)
        {
            _projectTemplates[_choice - 1].PopulateProjectModel(model);
        }

        public ProjectTypePrompt()
        {
            _templatePath = PathTools.GetTemplateRootPath();
            _projectTemplates = new List<IProjectTemplate>();
            foreach (Type t in EnumerateProjectTemplateReflections())
                _projectTemplates.Add((IProjectTemplate)t.Assembly.CreateInstance(t.FullName));
        }

        public void Show()
        {
            PromptCommon.WriteLine("Select your project type from the list below: ", ConsoleColor.DarkGray);
            for (int i = 0; i < _projectTemplates.Count; i++)
            {
                Console.Write($"{i + 1}. "); PromptCommon.WriteLine(_projectTemplates[i].Name, ConsoleColor.DarkYellow);
            }
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
                if (_choice >= 1 && _choice <= _projectTemplates.Count)
                    return true;
            }

            return false;
        }

        private IEnumerable<Type> EnumerateProjectTemplateReflections()
        {
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type type in assembly.GetTypes())
                    if (type.GetCustomAttributes(typeof(ProjectTemplateAttribute), true).Length > 0)
                        yield return type;
        }
    }
}

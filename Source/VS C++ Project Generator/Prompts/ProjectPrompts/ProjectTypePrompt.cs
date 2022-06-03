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
        private List<Type> _projectTypes;

        public void Populate(ProjectModel model)
        {
            object[] args = { model };
            _projectTypes[_choice - 1].GetMethod("PopulateProjectModel").Invoke(null, args);
        }

        public ProjectTypePrompt()
        {
            _templatePath = PathTools.GetTemplateRootPath();
            _projectTypes = new List<Type>();
            foreach (Type t in EnumerateProjectTemplateReflections())
                _projectTypes.Add(t);
        }

        public void Show()
        {
            PromptCommon.WriteLine("Select your project type from the list below: ", ConsoleColor.DarkGray);
            for (int i = 0; i < _projectTypes.Count; i++)
            {
                string projectName = (string)_projectTypes[i].GetMethod("GetName").Invoke(null, null);
                Console.Write($"{i + 1}. "); PromptCommon.WriteLine(projectName, ConsoleColor.DarkYellow);
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
                if (_choice >= 1 && _choice <= _projectTypes.Count)
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

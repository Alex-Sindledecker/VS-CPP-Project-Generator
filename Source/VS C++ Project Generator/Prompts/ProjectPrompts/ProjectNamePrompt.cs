using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    class ProjectNamePrompt : IProjectPrompt
    {
        private string _name;

        public void Populate(ProjectModel model)
        {
            model.Name = _name;
        }

        public void Show()
        {
            PromptCommon.Write("Project Name: ", ConsoleColor.DarkGray);
        }

        public void ShowFailedValidationMessage()
        {
            PromptCommon.WriteLine("That is not a valid project name!", ConsoleColor.DarkRed);
        }

        public bool Validate(string userInput)
        {
            //Check to make sure windows will accept the filename
            if (userInput.Length == 0 || userInput.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {
                return false;
            }

            _name = userInput;

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    class DiskLocationPrompt : IProjectPrompt
    {
        private string _path;

        public void Populate(ProjectModel model)
        {
            model.DiskLocation = _path;
        }

        public void Show()
        {
            PromptCommon.Write("Project path: ", ConsoleColor.DarkGray);
        }

        public void ShowFailedValidationMessage()
        {
            PromptCommon.WriteLine("That is not a valid filepath!", ConsoleColor.Red);
        }

        public bool Validate(string userInput)
        {
            if (userInput.StartsWith("C:/") || userInput.StartsWith("C:\\") && PromptCommon.IsValidFilePath(userInput))
            {
                _path = userInput;
                PromptCommon.EnsureConsistentFilePath(ref _path);

                return true;
            }

            return false;
        }
    }
}

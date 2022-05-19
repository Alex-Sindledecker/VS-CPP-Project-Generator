using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    public class IncludeDirPrompt : IDependencyPrompt
    {
        private string _dir;

        public void Populate(DependencyModel model)
        {
            model.IncludeDir = _dir;
        }

        public void Show()
        {
            Console.Write("Include folder path: ");
        }

        public void ShowFailedValidationMessage()
        {
            Console.WriteLine("Invalid path entered!");
        }

        public bool Validate(string userInput)
        {
            if (ValidationCommon.IsValidFilePath(userInput))
            {
                _dir = userInput;
                return true;
            }

            return false;
        }
    }
}

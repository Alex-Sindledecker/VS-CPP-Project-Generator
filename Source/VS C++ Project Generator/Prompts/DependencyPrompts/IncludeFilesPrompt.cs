using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    class IncludeFilesPrompt : IDependencyPrompt
    {
        private List<string> _files;

        public IncludeFilesPrompt()
        {
            _files = new List<string>();
        }

        public void Populate(DependencyModel model)
        {
            model.IncludeInProject = _files;
        }

        public void Show()
        {
            Console.WriteLine("Enter a list of additional files to include in the compilation of the project (ex: dep/tools.h, dep/tools.cpp):");
        }

        public void ShowFailedValidationMessage()
        {
            Console.WriteLine("That is not a valid file input!!");
        }

        public bool Validate(string userInput)
        {
            foreach (string file in userInput.Split(','))
            {
                if (PromptCommon.IsValidFilePath(file))
                    _files.Add(file);
                else
                {
                    _files.Clear();
                    return false;
                }
            }

            return true;
        }
    }
}

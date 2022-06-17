using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    public class IncludeFilesPrompt : IDependencyPrompt
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
            PromptCommon.WriteLine("Enter a list of any additional files to include in the compilation of the project (ex: dep/tools.h, dep/tools.cpp):", ConsoleColor.DarkGray);
        }

        public void ShowFailedValidationMessage()
        {
            PromptCommon.WriteLine("That is not a valid file input!!", ConsoleColor.Red);
        }

        public bool Validate(string userInput)
        {
            if (userInput.Length == 0)
                return true;
            foreach (string file in userInput.Split(','))
            {
                if (PromptCommon.IsValidFilePath(file) && Path.GetExtension(file) != "")
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

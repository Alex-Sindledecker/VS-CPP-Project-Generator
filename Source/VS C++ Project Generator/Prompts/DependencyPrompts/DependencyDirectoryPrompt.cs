using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    public enum DependencyDirectoryType { Include, Library, Dll }

    public class DependencyDirectoryPrompt : IDependencyPrompt
    {
        private string _dir;
        private DependencyDirectoryType _dirType;

        public DependencyDirectoryPrompt(DependencyDirectoryType dirType)
        {
            _dirType = dirType;
        }

        public void Populate(DependencyModel model)
        {
            switch (_dirType)
            {
                case DependencyDirectoryType.Include:
                    model.IncludeDir = _dir;
                    break;
                case DependencyDirectoryType.Library:
                    model.LibDir = _dir;
                    break;
                case DependencyDirectoryType.Dll:
                    model.DllDir = _dir;
                    break;
            }
        }

        public void Show()
        {
            PromptCommon.Write($"{_dirType.ToString()} folder path: ", ConsoleColor.DarkGray);
        }

        public void ShowFailedValidationMessage()
        {
            PromptCommon.WriteLine("Invalid path entered!", ConsoleColor.Red);
        }

        public bool Validate(string userInput)
        {
            if ((_dirType != DependencyDirectoryType.Include && userInput.Length == 0) || PromptCommon.IsValidFilePath(userInput))
            {
                _dir = userInput;
                PromptCommon.EnsureConsistentFilePath(ref _dir);

                return true;
            }

            return false;
        }
    }
}

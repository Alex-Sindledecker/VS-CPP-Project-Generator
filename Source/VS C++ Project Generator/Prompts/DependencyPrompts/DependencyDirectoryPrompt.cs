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
            Console.Write($"{_dirType.ToString()} folder path: ");
        }

        public void ShowFailedValidationMessage()
        {
            Console.WriteLine("Invalid path entered!");
        }

        public bool Validate(string userInput)
        {
            if ((_dirType != DependencyDirectoryType.Include && userInput.Length == 0) || ValidationCommon.IsValidFilePath(userInput))
            {
                _dir = userInput;
                if (_dir.Length != 0 && _dir.EndsWith('/') == false && _dir.EndsWith('\\') == false)
                    _dir += '/';

                return true;
            }

            return false;
        }
    }
}

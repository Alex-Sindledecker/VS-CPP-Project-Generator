using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    public enum DependencyLibraryConfiguration { Debug, Release }

    public class LibraryNamesPrompt : IDependencyPrompt
    {
        private List<string> _libNames;
        private DependencyLibraryConfiguration _config;
        private string _invalidLibName;

        public LibraryNamesPrompt(DependencyLibraryConfiguration configuration)
        {
            _config = configuration;
            _libNames = new List<string>();
        }

        public void Populate(DependencyModel model)
        {
            switch (_config)
            {
                case DependencyLibraryConfiguration.Debug:
                    model.DebugLibNames = _libNames;
                    break;
                case DependencyLibraryConfiguration.Release:
                    model.ReleaseLibNames = _libNames;
                    break;
            }
        }

        public void Show()
        {
            Console.Write($"Enter {_config.ToString().ToLower()} library names (ex: sfml-system.lib, sfml-window.lib): ");
        }

        public void ShowFailedValidationMessage()
        {
            Console.WriteLine($"{_invalidLibName} is not a valid library!");
        }

        public bool Validate(string userInput)
        {
            foreach (string name in userInput.Split(',', ' '))
            {
                if (name == "")
                    continue;

                if (name.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1 && name.EndsWith(".lib") && name != ".lib")
                    _libNames.Add(name);
                else
                {
                    _invalidLibName = name;
                    _libNames.Clear();
                    return false;
                }
            }

            return true;
        }
    }
}

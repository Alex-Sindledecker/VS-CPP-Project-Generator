using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    public class DependencyURLPrompt : IDependencyPrompt
    {
        private string _url;

        public void Populate(DependencyModel model)
        {
            model.Url = _url;
        }

        public void Show()
        {
            PromptCommon.Write("Dependency url (github repo or zip file): ", ConsoleColor.DarkGray);
        }

        public void ShowFailedValidationMessage()
        {
            PromptCommon.WriteLine("Invalid url!", ConsoleColor.Red);
        }

        public bool Validate(string userInput)
        {
            if (Uri.IsWellFormedUriString(userInput, UriKind.Absolute))
            {
                Regex zipExtensionRegex = new Regex(@"^.*\.(zip)$");
                Regex githubURLRegex = new Regex(@"https:\/\/github.com\/[a-zA-Z|\-|0-9]+\/[a-zA-Z|\-|0-9]+$");
                if (zipExtensionRegex.IsMatch(userInput) == true || githubURLRegex.IsMatch(userInput))
                {
                    _url = userInput;
                    return true;
                }
            }
            return false;
        }
    }
}

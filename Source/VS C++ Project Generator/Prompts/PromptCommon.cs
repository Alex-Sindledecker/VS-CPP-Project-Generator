using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Prompts
{
    //Common functions for usings prompts
    public static class PromptCommon
    {
        //Runs the prompt loop (show, validate, if invalid: show failed validation and restart - else: return)
        public static void RunPrompt(IPrompt prompt)
        {
            string userInput;
            prompt.Show();
            userInput = Console.ReadLine();
            while (prompt.Validate(userInput) != true)
            {
                prompt.ShowFailedValidationMessage();
                prompt.Show();
                userInput = Console.ReadLine();
            }
        }

        //Checks to ensure a file path is valid
        public static bool IsValidFilePath(string path)
        {
            char[] otherInvalidChars = { '?', '*', '"', '<', '>', '|' };
            if (path.Length == 0 || path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1 || path.IndexOfAny(otherInvalidChars) != -1)
                return false;

            return true;
        }

        //Appends a / to the directory if there isn't a trailing one
        public static void EnsureConsistentFilePath(ref string path)
        {
            if (path.Length != 0 && path.EndsWith('/') == false && path.EndsWith('\\') == false)
                path += '/';
        }
    }
}

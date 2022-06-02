using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Prompts
{
    class ConditionalPrompt : IPrompt
    {
        private string _message;
        private bool _conditionalResult;

        public bool Result { get { return _conditionalResult; } }

        public ConditionalPrompt(string message)
        {
            _message = message;
        }
        public void Show()
        {
            PromptCommon.Write($"{_message} (y/n): ", ConsoleColor.DarkGray);
        }

        public void ShowFailedValidationMessage()
        {
            PromptCommon.WriteLine("That is not a valid option! Valid options are 'y' or 'n'...", ConsoleColor.Red);
        }

        public bool Validate(string userInput)
        {
            string lower = userInput.ToLower();

            if (lower == "y")
                _conditionalResult = true;
            else if (lower == "n")
                _conditionalResult = false;
            else
                return false;

            return true;
        }
    }
}

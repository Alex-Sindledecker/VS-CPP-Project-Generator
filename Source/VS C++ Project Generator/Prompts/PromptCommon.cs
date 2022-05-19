using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Prompts
{
    //Common functions for usings prompts
    public static class PromptCommon
    {
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
    }
}

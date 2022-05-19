using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    public interface IPrompt
    {
        public void Show();
        public void ShowFailedValidationMessage();
        public bool Validate(string userInput);
        public void Populate(ProjectModel model);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Prompts;

namespace VS_CPP_Project_Generator.Models.ModelGenerators
{
    class ProjectModelGenerator
    {
        private List<IPrompt> _prompts;
        private ProjectModel _projectModel;

        public ProjectModelGenerator()
        {
            _prompts = new List<IPrompt>();
            _projectModel = new ProjectModel();
        }

        public ProjectModel Model
        {
            get
            {
                return _projectModel;
            }
        }

        public void AddPrompt(IPrompt prompt)
        {
            _prompts.Add(prompt);
        }

        public void RunPrompts()
        {
            foreach (IPrompt prompt in _prompts)
            {
                RunPrompt(prompt);
                prompt.Populate(_projectModel);
            }
        }

        private void RunPrompt(IPrompt prompt)
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

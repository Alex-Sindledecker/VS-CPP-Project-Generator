using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Prompts;

namespace VS_CPP_Project_Generator.Models.ModelGenerators
{
    public class ProjectModelGenerator
    {
        private List<IProjectPrompt> _prompts;

        public ProjectModelGenerator()
        {
            _prompts = new List<IProjectPrompt>();
        }

        public void AddPrompt(IProjectPrompt prompt)
        {
            _prompts.Add(prompt);
        }

        public ProjectModel RunPrompts()
        {
            ProjectModel model = new ProjectModel();

            foreach (IProjectPrompt prompt in _prompts)
            {
                PromptCommon.RunPrompt(prompt);
                prompt.Populate(model);
            }

            return model;
        }
    }
}

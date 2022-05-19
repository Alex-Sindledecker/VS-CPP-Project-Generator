using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Logging;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    public interface IPrompt
    {
        public void Show(ILogger logger);
        public bool Validate();
        public void Populate(ProjectModel model);
    }
}

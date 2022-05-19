using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.Prompts
{
    public interface IDependencyPrompt : IPrompt
    {
        public void Populate(DependencyModel model);
    }
}

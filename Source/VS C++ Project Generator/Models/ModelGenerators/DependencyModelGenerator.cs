using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Prompts;

namespace VS_CPP_Project_Generator.Models.ModelGenerators
{
    public class DependencyModelGenerator
    {
        /*=================Create custom dependencies=================*/

        private List<IDependencyPrompt> _prompts;

        public DependencyModelGenerator()
        {
            _prompts = new List<IDependencyPrompt>();
        }

        public void AddPrompt(IDependencyPrompt prompt)
        {
            _prompts.Add(prompt);
        }

        public DependencyModel RunPrompts()
        {
            DependencyModel model = new DependencyModel();

            foreach (IDependencyPrompt prompt in _prompts)
            {
                PromptCommon.RunPrompt(prompt);
                prompt.Populate(model);
            }

            return model;
        }

        /*=================Built in dependencies=================*/

        public static DependencyModel GetSFMLModel()
        {
            return new DependencyModel
            {
                Url = "https://github.com/SFML/SFML/releases/download/2.5.1/SFML-2.5.1-windows-vc15-64-bit.zip",
                IncludeDir = "SFML-2.5.1/include/",
                LibDir = "SFML-2.5.1/lib/",
                DllDir = "SFML-2.5.1/bin/",
                DebugLibNames = new List<string> { "sfml-graphics-d.lib", "sfml-window-d.lib", "sfml-system-d.lib" },
                ReleaseLibNames = new List<string> { "sfml-graphics.lib", "sfml-window.lib", "sfml-system.lib" }
            };
        }

        /*
        public static DependencyModel GetGLADModel()
        {

        }

        public static DependencyModel GetGLFWModel()
        {

        }

        public static DependencyModel GetGLMModel()
        {

        }
        */
    }
}

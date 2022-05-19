using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.Models.ModelGenerators
{
    public static class DependencyGenerator
    {
        public static DependencyModel GetSFMLModel()
        {
            return new DependencyModel
            {
                Url = "https://github.com/SFML/SFML/releases/download/2.5.1/SFML-2.5.1-windows-vc15-64-bit.zip",
                IncludeDir = "SFML-2.5.1/include/",
                LibDir = "SFML-2.5.1/lib/",
                DllDir = "SFML-2.5.1/bin/",
                DebugLibNames = new List<string> { "sfml-graphics.lib", "sfml-window.lib", "sfml-system.lib" },
                ReleaseLibNames = new List<string> { "sfml-graphics-d.lib", "sfml-window-d.lib", "sfml-system-d.lib" }
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

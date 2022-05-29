using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
                IncludeDir = "SFML-2.5.1-windows-vc15-64-bit/SFML-2.5.1/include/",
                LibDir = "SFML-2.5.1-windows-vc15-64-bit/SFML-2.5.1/lib/",
                DllDir = "SFML-2.5.1-windows-vc15-64-bit/SFML-2.5.1/bin/",
                DebugLibNames = new List<string> { "sfml-graphics-d.lib", "sfml-window-d.lib", "sfml-system-d.lib" },
                ReleaseLibNames = new List<string> { "sfml-graphics.lib", "sfml-window.lib", "sfml-system.lib" },
                IncludeInProject = new List<string> {  }
            };
        }

        public static DependencyModel GetGLADModel()
        {
            return new DependencyModel
            {
                Url = GetGLADZipURL(),
                IncludeDir = "glad/include/",
                LibDir = "",
                DllDir = "",
                DebugLibNames = new List<string> { "opengl32.lib" },
                ReleaseLibNames = new List<string> { "opengl32.lib" },
                IncludeInProject = new List<string> { "glad/src/glad.c" }
            };
        }

        public static DependencyModel GetGLFWModel()
        {
            return new DependencyModel
            {
                Url = "https://github.com/glfw/glfw/releases/download/3.3.7/glfw-3.3.7.bin.WIN64.zip",
                IncludeDir = "glfw-3.3.7.bin.WIN64/glfw-3.3.7.bin.WIN64/include/",
                LibDir = "glfw-3.3.7.bin.WIN64/glfw-3.3.7.bin.WIN64/lib-vc2019/",
                DllDir = "glfw-3.3.7.bin.WIN64/glfw-3.3.7.bin.WIN64/lib-vc2019/",
                DebugLibNames = new List<string> { "glfw3.lib" },
                ReleaseLibNames = new List<string> { "glfw3.lib" },
                IncludeInProject = new List<string> { }
            };
        }

        public static DependencyModel GetGLMModel()
        {
            return new DependencyModel
            {
                Url = "https://github.com/g-truc/glm/releases/download/0.9.9.8/glm-0.9.9.8.zip",
                IncludeDir = "glm-0.9.9.8/glm/",
                LibDir = "",
                DllDir = "",
                DebugLibNames = new List<string> { },
                ReleaseLibNames = new List<string> { },
                IncludeInProject = new List<string> { }
            };
        }

        //There isn't a direct url to the glad zip file so we have to go through the generator which gives us a temporary url to the zip file
        private static string GetGLADZipURL()
        {
            string postData = "language=c&specification=gl&api=gl%3D4.1&api=gles1%3Dnone&api=gles2%3Dnone&api=glsc2%3Dnone&profile=core&loader=on";
            byte[] send = Encoding.Default.GetBytes(postData);

            WebRequest request = WebRequest.Create("https://glad.dav1d.de/generate");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = send.Length;

            using (Stream os = request.GetRequestStream())
                os.Write(send, 0, send.Length);

            WebResponse response = request.GetResponse();

            return $"{response.ResponseUri}glad.zip";
        }
    }
}

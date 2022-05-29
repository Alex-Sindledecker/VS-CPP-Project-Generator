using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Models.ModelGenerators;

namespace UnitTests
{
    [TestClass]
    public class DependencyModelValidation
    {
        [TestMethod]
        public void ValidateSFMLModel()
        {
            DependencyModel model = DependencyModelGenerator.GetSFMLModel();

            List<string> expectedDebugLibs = new List<string> { "sfml-graphics-d.lib", "sfml-window-d.lib", "sfml-system-d.lib" };
            List<string> expectedReleaseLibs = new List<string> { "sfml-graphics.lib", "sfml-window.lib", "sfml-system.lib" };

            Assert.AreEqual(model.Url, "https://github.com/SFML/SFML/releases/download/2.5.1/SFML-2.5.1-windows-vc15-64-bit.zip",
                "Incorrect SFML Url being used!!");
            Assert.AreEqual(model.IncludeDir, "SFML-2.5.1-windows-vc15-64-bit/SFML-2.5.1/include/", "Incorrect include directory!");
            Assert.AreEqual(model.LibDir, "SFML-2.5.1-windows-vc15-64-bit/SFML-2.5.1/lib/", "Incorrect lib directory!");
            Assert.AreEqual(model.DllDir, "SFML-2.5.1-windows-vc15-64-bit/SFML-2.5.1/bin/", "Incorrect bin directory!");
            CollectionAssert.AreEqual(model.DebugLibNames, expectedDebugLibs, "Incorrect debug lib names generated!");
            CollectionAssert.AreEqual(model.ReleaseLibNames, expectedReleaseLibs, "Incorrect release lib names generated!");
            Assert.IsTrue(model.IncludeInProject.Count == 0, "No files should be included for SFML!");
        }

        [TestMethod]
        public void ValidateGLADModel()
        {
            DependencyModel model = DependencyModelGenerator.GetGLADModel();

            Assert.IsTrue(IsValidURL(model.Url), "Bad url found for glad!");
            Assert.AreEqual(model.IncludeDir, "glad/include/");
            Assert.AreEqual(model.LibDir, "", "There isn't a lib dir for GLAD!");
            Assert.AreEqual(model.DllDir, "", "There isn't a dll dir for GLAD!");
            CollectionAssert.AreEqual(model.DebugLibNames, new List<string> { "opengl32.lib" });
            CollectionAssert.AreEqual(model.ReleaseLibNames, new List<string> { "opengl32.lib" });
            CollectionAssert.AreEqual(model.IncludeInProject, new List<string> { "glad/src/glad.c" });
        }

        [TestMethod]
        public void ValidateGLFWModel()
        {
            DependencyModel model = DependencyModelGenerator.GetGLFWModel();

            Assert.AreEqual(model.Url, "https://github.com/glfw/glfw/releases/download/3.3.7/glfw-3.3.7.bin.WIN64.zip");
            Assert.AreEqual(model.IncludeDir, "glfw-3.3.7.bin.WIN64/glfw-3.3.7.bin.WIN64/include/");
            Assert.AreEqual(model.LibDir, "glfw-3.3.7.bin.WIN64/glfw-3.3.7.bin.WIN64/lib-vc2019/");
            Assert.AreEqual(model.DllDir, "glfw-3.3.7.bin.WIN64/glfw-3.3.7.bin.WIN64/lib-vc2019/");
            CollectionAssert.AreEqual(model.DebugLibNames, new List<string> { "glfw3.lib" });
            CollectionAssert.AreEqual(model.ReleaseLibNames, new List<string> { "glfw3.lib" });
            CollectionAssert.AreEqual(model.IncludeInProject, new List<string> { });
        }

        [TestMethod]
        public void ValidateGLMModel()
        {
            DependencyModel model = DependencyModelGenerator.GetGLMModel();

            Assert.AreEqual(model.Url, "https://github.com/g-truc/glm/releases/download/0.9.9.8/glm-0.9.9.8.zip");
            Assert.AreEqual(model.IncludeDir, "glm-0.9.9.8/glm/");
            Assert.AreEqual(model.LibDir, "");
            Assert.AreEqual(model.DllDir, "");
            CollectionAssert.AreEqual(model.DebugLibNames, new List<string> {  });
            CollectionAssert.AreEqual(model.ReleaseLibNames, new List<string> {  });
            CollectionAssert.AreEqual(model.IncludeInProject, new List<string> { });
        }

        private bool IsValidURL(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 15000;

            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
